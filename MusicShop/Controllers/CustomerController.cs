using MusicShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace MusicShop.Controllers
{
    public class CustomerController : Controller
    {
        DAO dao = new DAO();

        //IsStaffActive is made available in controllers that have access
        //to adminstrative Methods, this is to ensure that no deactivated
        //staff members, who were deactivated after a successful login and 
        //still have an active session. It will be used to contole the 
        //closure of their session in such a case.
      /*  private bool IsStaffActive(string email)
        {
            Admin admin = dao.SelectOneAdmin(email);
            return admin.IsActive;
        }*/

        //Method to ensure that Session state is valid (userType is staff with an email set).
        //Also checks IsActive state from the db to ensure that staff member was not
        //deactivated. If Session state is invalid or the currently logged in staff member
        //has since been deactivated, the session will be cleared. Thus ensuring only properly
        //logged in staff members will be able to access certain pages.
     /*   private bool IsStaffSessionValid()
        {
            if (Session != null)
            {
                if (Session["userType"] != null)
                {
                    if (Session["userType"].ToString() == "staff")
                    {
                        if (Session["email"].ToString() != null)
                        {
                            if (!IsStaffActive(Session["email"].ToString()))
                            {
                                Session.Clear();
                                return false;
                            }
                        }
                        else
                        {
                            Session.Clear();
                            return false;
                        }
                    }
                }
            }
            else //If Session == null, then StaffSession is not valid
            {
                return false;
            }
            return true;
        }*/

        // GET: Customer
        public ActionResult Index()
        {
            if (Session != null && Session["userType"] != null && Session["userType"].ToString() == "customer")
            {
                Customer customer = dao.ShowOneCustomer(Session["email"].ToString());
                return View(customer);
            }
            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            int count = 0;
            if (ModelState.IsValid)
            {
                count = dao.InsertCustomer(customer);
                if (count == 1)
                {
                    Session.Clear();
                    Session["userType"] = "customer";
                    Session["email"] = customer.Email;
                    Session["userName"] = customer.Name;
                 /*   var accountSid = "AC1a2e3890b1a6e433fa1cc9ce1cbf27d8";
                    var authToken = "6e49dc86616f25c3079d338c5d3e9a67";
                    TwilioClient.Init(accountSid, authToken);

                    var to = new PhoneNumber("+353866629553");
                    var from = new PhoneNumber("+441704325091");

                    var message = MessageResource.Create(to: to, from: from, body: customer.Name + ", Thank you for registering on our site"); */
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Status = "Error! " + dao.message;
                }
            }
            else
            {
                ViewBag.Status = "You have not entered all required fields, please fix and resubmit.";
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult SignIn(Customer customer)
        {
            ModelState.Remove("Name");
            ModelState.Remove("AddressLine1");
            ModelState.Remove("Country");
            if (ModelState.IsValid)
            {
                Customer customerRecord = dao.ShowOneCustomer(customer.Email);
                if (customerRecord != null && Crypto.VerifyHashedPassword(customerRecord.Password, customer.Password))
                {
                    Session.Clear();
                    Session["userType"] = "customer";
                    Session["email"] = customer.Email;
                    
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    ViewBag.Status = "Error " + dao.message;
                    return View();
                }
            }
            else
            {
                return View(customer);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Session != null && Session["userType"] != null && Session["userType"].ToString() == "customer")
            {
                Session.Clear();
            }
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public ActionResult ListOfCustomer()
        {
            List<Customer> list = null;
            
            
                list = dao.SelectAllCustomers();
                return View(list);
            
         
        }

        [HttpGet]
        public ActionResult DeleteCustomer()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult DeleteCustomer(Customer customer)
        {
           
            dao.DeleteCustomer(customer.Email);
            return RedirectToAction("ListOfCustomer");
        }

        [HttpPost]
        public ActionResult ResetPass(Customer customer)
        {
          
            customer.Password = Crypto.HashPassword("pass");
            dao.UpdateCustomer(customer);
            return RedirectToAction("ListOfCustomer");
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPassword(FormCollection passwords)
        {
            Customer customer = dao.ShowOneCustomer(Session["email"].ToString());

            if (Crypto.VerifyHashedPassword(customer.Password, passwords["txtCurrPassword"]))
            {
                customer.Password = Crypto.HashPassword(passwords["txtNewPassword1"]);
                dao.UpdateCustomer(customer);
                ViewBag.Message = "Password Changed Successfully";
                return View();
            }
            else
            {
                ViewBag.Message = "Passwords do not match";
                return View();
            }
        }
    }
}
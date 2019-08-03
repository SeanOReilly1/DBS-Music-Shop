using MusicShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MusicShop.Controllers
{
    public class AdminController : Controller
    {
        DAO dao = new DAO();

        

        

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            
            if (ModelState.IsValid)
            {
                Admin staffRecord = dao.SelectOneAdmin(admin.Email);
                if (staffRecord != null)
                {
                    if (admin.Email == staffRecord.Email)
                    {
                        //This will only happen on first time login at which time we want to hash the password
                        if (admin.Password == staffRecord.Password)
                        {
                            staffRecord.Password = Crypto.HashPassword(admin.Password);
                            dao.UpdateAdmin(staffRecord);
                        }

                        if (Crypto.VerifyHashedPassword(staffRecord.Password, admin.Password))
                        {
                            
                            
                                Session.Clear();
                                Session["userType"] = "staff";
                                Session["email"] = admin.Email;
                                return RedirectToAction("Index");
                            
                            
                       
                        }
                     
                    }
                  
                }
              
            }
            else
            {
                ViewBag.Message = "You must enter valid data in the form.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Session["userType"] != null && Session["userType"].ToString() == "staff")
            {
                Session.Clear();
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ListOfAdmin()
        {
            List<Admin> listOfAdmin = null;
            
            
                listOfAdmin = dao.SelectAllAdmin();
                return View(listOfAdmin);
            
           
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(Admin admin)
        {
            
            admin.Password = Crypto.HashPassword(admin.Password);
            if (dao.InsertAdmin(admin) == 1)
            {
                ViewBag.Message = "Staff member successfully added.";
                return View();
            }
            else
            {
                ViewBag.Message = "Something went wrong... Staff member not added.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteAdmin()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult DeleteAdmin(Admin staff)
        {
            
            dao.DeleteAdminMember(staff.Email);
            return RedirectToAction("ListOfAdmin");
        }

        [HttpGet]
        public ActionResult EditPassword()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult EditPassword(FormCollection form)
        {
            Admin aRecord = dao.SelectOneAdmin(Session["email"].ToString());

            if (Crypto.VerifyHashedPassword(aRecord.Password, form["txtCurrPassword"]))
            {
                aRecord.Password = Crypto.HashPassword(form["txtNewPassword1"]);
                dao.UpdateAdmin(aRecord);
                ViewBag.Message = "Password Changed Successfully";
                return View();
            }
            else
            {
                ViewBag.Message = "The password you entered for Current Password does not match the stored password";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ResetPass()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult ResetPass(Admin admin)
        {
           
            admin.Password = Crypto.HashPassword("admin");
            dao.UpdateAdmin(admin);
            return RedirectToAction("ListOfAdmin");
        }
    }
}
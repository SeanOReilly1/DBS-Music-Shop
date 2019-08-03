using MusicShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MusicShop.Controllers
{
    public class OrderController : Controller
    {

        DAO dao = new DAO();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cart()
        {
            List<Guitar> list = dao.ShowAllGuitars();
            return View(list);
        }

        [HttpPost]
        public ActionResult Cart(FormCollection itemsInCart)
        {
            Response.Cookies["cart"].Expires = DateTime.Now.AddDays(-1);//deletes cookie
            //used for storing names of items ordered for email order details 
            string sentNames = null, name = null; ;
            //used for storing total price of items ordered for email order details 
            decimal total = 0;
            //gets all the items in the cart which are seperated by : and stores them in a string array 
            string[] itemsFromCart = itemsInCart["cartContents"].Split(':');
            //declares a list of guitars and adds all guitars in database to it 
            List<Guitar> guitarList = dao.ShowAllGuitars();
            // decalres a new order 
            Order newOrder = new Order();
            //declares a new list of orderItems 
            List<OrderItem> allItems = new List<OrderItem>();
            foreach (string item in itemsFromCart)
            {
                string[] titleQty = item.Split('_');
                foreach (Guitar m in guitarList)
                {
                    if (m.ID == int.Parse(titleQty[0]))
                    {
                        OrderItem newOI = new OrderItem();
                        newOI.Price = m.Price;
                        newOI.Quantity = int.Parse(titleQty[1]);
                        newOI.ItemOrdered = m;
                        allItems.Add(newOI);
                        sentNames += m.Name + " ";
                        total += m.Price * Convert.ToInt32(titleQty[1]);
                        break;
                    }
                }
            }
            newOrder.OrderItems = allItems;
            newOrder.CustomerEmail = Session["email"].ToString();
            
            dao.InsertOrder(newOrder.CustomerEmail);
            newOrder.OrderID = dao.GetCurrentOrderID();

            

            foreach (OrderItem item in newOrder.OrderItems)
            {
                dao.InsertOrderItem(newOrder.OrderID, item);
                
            }

            string recepiant = newOrder.CustomerEmail;
            

            WebMail.SmtpServer = "smtp.gmail.com";
            WebMail.SmtpPort = 587;
            WebMail.SmtpUseDefaultCredentials = true;
            WebMail.EnableSsl = true;
        
            WebMail.UserName = "musicshop1122@gmail.com";
            WebMail.Password = "testtest1122";

            WebMail.Send(to: recepiant, subject: "Order Details", body: "Hi There!," + "<br> Thank you for your order of " 
                + sentNames.ToString() + "<br> Your purchase will be delivered in the next 10 working days " 
                + "<br> The total Price of your order is €" + total + "<br><br> Regards <br> Music Shop Team", isBodyHtml: true);

            return RedirectToAction("LatestOrder", "Order", newOrder);
        }

        [HttpGet]
        public ActionResult LatestOrder()
        {
            Order latestOrder = new Order();
            int orderID = dao.latestOrderCustomer(Session["email"].ToString());
            latestOrder = dao.GetOrder(orderID);
            foreach (OrderItem itemOrdered in latestOrder.OrderItems)
            {
                itemOrdered.ItemOrdered = dao.ShowOneGuitar(itemOrdered.ItemOrdered.ID);
            }

            return View(latestOrder);
        }

        public ActionResult AdminOrders()
        {
            if (Session != null && Session["userType"] != null && Session["userType"].ToString() == "staff")
            {
                List<Order> list = dao.GetAllOrders();
                return View(list);
            }
            return View();
        }

        public ActionResult AdminOrderItems(int id)
        {
            Order order = new Order();
            int orderID = id;
            order = dao.GetOrder(orderID);
            foreach (OrderItem item in order.OrderItems)
            {
                item.ItemOrdered = dao.ShowOneGuitar(item.ItemOrdered.ID);
            }

            return View(order);
        }
    }


}

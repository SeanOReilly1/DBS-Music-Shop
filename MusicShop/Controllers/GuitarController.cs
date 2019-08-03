using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicShop.Models;
using System.IO;

namespace MusicShop.Controllers
{
    public class GuitarController : Controller
    {
        DAO dao = new DAO();

        


        // GET: Guitar
        public ActionResult Index()
        {
            List<Guitar> list = dao.ShowAllGuitars();
            return View(list);
        }

        public ActionResult Amps()
        {
            List<Guitar> list = dao.ShowAllGuitars();
            return View(list);
        }

        public ActionResult Search(string search)
        {
            List<Guitar> list = dao.ShowFilterList(search);
            return View(list);
        }

        [HttpGet]
        public ActionResult Add()
        {
           
            return View();
        }

        [HttpGet]
        public ActionResult Single(int id)
        {
            Guitar guitar = dao.ShowOneGuitar(id);
            return View(guitar);
        }

        [HttpPost]
        public ActionResult Add(Guitar guitar)
        {
            

            int counter = 0; 
            counter = dao.InsertGuitar(guitar);
            if (counter == 1)
            {
                ViewBag.Message = "Record inserted successfully";
                ModelState.Clear();
            }
            else
            {
                ViewBag.Message = "Error, " + dao.message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult AddImage()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Guitar guitar = dao.ShowOneGuitar(id);
            return View(guitar);
        }

        [HttpPost]
        public ActionResult Edit(Guitar guitar)
        {
            if (ModelState.IsValid)
            {
                dao.UpdateGuitar(guitar);
                return RedirectToAction("Index");
            }

            return View(guitar);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            dao.DeleteGuitar(ID);
            return RedirectToAction("Index");
        }
    }
}
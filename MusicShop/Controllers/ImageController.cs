using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MusicShop.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if( file.ContentLength > 0)
            {
                string imgName = Path.GetFileName(file.FileName);
                string imgTxt = Path.GetExtension(imgName);
                if(imgTxt == ".jpg" || imgTxt == ".png")
                {
                    string imgPath = Path.Combine(Server.MapPath("~/Content/Images"), imgName);
                    file.SaveAs(imgPath);
                }
            }
            return View();
        }
    }
}
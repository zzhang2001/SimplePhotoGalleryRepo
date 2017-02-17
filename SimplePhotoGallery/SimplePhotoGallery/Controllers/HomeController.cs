using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplePhotoGallery.Models;

namespace SimplePhotoGallery.Controllers
{
    public class HomeController : Controller
    {
        private PhotoDbContext context = new PhotoDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SlideShow()
        {
            List<Photo> photos = context.Photos.ToList();
            return View(photos);
        }

        public ActionResult AllPhotos()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
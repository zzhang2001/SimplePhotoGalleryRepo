using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplePhotoGallery.Models;

namespace SimplePhotoGallery.Controllers
{
    public class PhotoController : Controller
    {
        private PhotoDbContext context = new PhotoDbContext();

        [Authorize]
        public ActionResult Create()
        {
            Photo p = new Photo();
            return View(p);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Photo p, HttpPostedFileBase inputImage)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            p.UserName = User.Identity.Name;
            p.CreatedDate = DateTime.Now;
            p.ModifiedDate = DateTime.Now;

            // If there is an image, upload it.
            if (inputImage != null)
            {
                p.ImageMimeType = inputImage.ContentType;
                p.FileName = System.IO.Path.GetFileName(inputImage.FileName);
                p.FileData = new byte[inputImage.ContentLength];
                inputImage.InputStream.Read(p.FileData, 0, inputImage.ContentLength);
            }

            // Add the photo to the database.
            context.Photos.Add(p);
            context.SaveChanges();

            return RedirectToAction("AllPhotos", "Home");
        }

        public ActionResult Details(int id = 0)
        {
            Photo p = context.Photos.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(p);
            }
        }

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Photo p = context.Photos.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Photos.Remove(p);
                context.SaveChanges();
                return RedirectToAction("AllPhotos", "Home");
            }
        }

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Photo p = context.Photos.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(p);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Photo p, int id = 0)
        {
            Photo p1 = context.Photos.Find(id);
            if (p1 == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    p1.Title = p.Title;
                    p1.Description = p.Description;
                    p1.ModifiedDate = DateTime.Now;
                    context.SaveChanges();
                    return RedirectToAction("Details", new { id = id });
                }
                else
                {
                    return View(p1);
                }

            }
        }

        public ActionResult GetComments(int PhotoId = 0)
        {
            // Disable proxy creation to prevent circular reference serialization error.
            context.Configuration.ProxyCreationEnabled = false;
            var comments = from c in context.Comments where c.PhotoId == PhotoId select c;
            CommentsViewModel vm = new CommentsViewModel();
            vm.Comments = comments.ToList();
            vm.IsAuthenticated = Request.IsAuthenticated;
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteComment(int CommentId = 0)
        {
            context.Configuration.ProxyCreationEnabled = false;
            Comment comment = context.Comments.Find(CommentId);
            if (comment == null)
            {
                return HttpNotFound();
            }
            context.Comments.Remove(comment);
            context.SaveChanges();

            var comments = from c in context.Comments where c.PhotoId == comment.PhotoId select c;
            CommentsViewModel vm = new CommentsViewModel();
            vm.Comments = comments.ToList();
            vm.IsAuthenticated = Request.IsAuthenticated;
            return Json(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(string NewCommentText, int PhotoId)
        {
            context.Configuration.ProxyCreationEnabled = false;
            Comment comment = new Comment();
            comment.PhotoId = PhotoId;
            comment.UserName = HttpContext.User.Identity.Name;
            comment.Body = NewCommentText;
            comment.CreatedDate = DateTime.Now;
            context.Comments.Add(comment);
            context.SaveChanges();

            var comments = from c in context.Comments where c.PhotoId == comment.PhotoId select c;
            CommentsViewModel vm = new CommentsViewModel();
            vm.Comments = comments.ToList();
            vm.IsAuthenticated = Request.IsAuthenticated;
            return Json(vm);
        }

        [ChildActionOnly]
        public ActionResult _PhotoList(int number = 0)
        {
            List<Photo> photos;
            if (number == 0)
            {
                photos = context.Photos.ToList();
            }
            else
            {
                photos = context.Photos.Take(number).ToList();
            }
            return PartialView("_PhotoList", photos);
        }

        public FileContentResult GetImage(int PhotoId)
        {
            Photo photo = context.Photos.FirstOrDefault(p => p.PhotoId == PhotoId);
            if (photo != null)
            {
                return File(photo.FileData, photo.ImageMimeType);
            }
            else
            {
                return null;
            }
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
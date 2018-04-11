using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Models;
using System.IO;
using System.Data.Entity;

namespace DemoWebApplication.Controllers
{
    [RoutePrefix("HomeBanner")]
    public class HomeBannerController : Controller
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();

        [Authorize(Roles = "Admin")]
        // GET: HomeBanner
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveBanner(HomeBanner banner, HttpPostedFileBase file)
        {
            var status= (Object)null;

            if (!ModelState.IsValid)
            {
                status = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
                return Json(new { errCode = 401, status }, JsonRequestBehavior.AllowGet);

            }

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);

            //save the file
            try
            {
                banner.ImagePath = "/Uploads/Banner/" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + extension;
                file.SaveAs(Server.MapPath("~/Uploads/Banner/") + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + extension);
            }
            catch (IOException exc)
            {
                status = exc.Message;
                return Json(new { status }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                banner.CreatedOn = System.DateTime.Now;
                banner.UpdatedOn = System.DateTime.Now;
                banner.CreatedBy = User.Identity.Name;
                banner.UpdatedBy = User.Identity.Name;
                _context.HomeBanners.Add(banner);
                _context.SaveChanges();
                status = 1;
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetBanners(string term, int? page, int? pageSize)
        {
            List<HomeBanner> banners = null;
            int totalCount = 0;

            try
            {
                IQueryable<HomeBanner> result = _context.HomeBanners;

                if (!string.IsNullOrEmpty(term))
                {
                    result = result.Where(w => w.Title.ToLower().Contains(term.ToLower())).OrderBy(o => o.Sequence);
                }
                else
                {
                    result = result.OrderBy(o => o.Sequence);
                }

                banners = result
                            .Skip(((int)page - 1) * (int)pageSize)
                            .Take((int)pageSize)
                            .ToList();

                totalCount = result.Count();
            }
            catch (Exception ex)
            {
                banners = null;
            }

            return Json(new { totalCount, banners }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetBannerByID(int id)
        {
            HomeBanner banner = null;
            try
            {
                banner = _context.HomeBanners.Where(w => w.ID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                banner = null;
            }

            return Json(new { banner }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult DeleteBanner(int id)
        {
            var status = (Object)null;
            try
            {
                var banner = _context.HomeBanners.Where(w => w.ID == id).FirstOrDefault();
                _context.HomeBanners.Attach(banner);
                _context.HomeBanners.Remove(banner);
                _context.SaveChanges();

                if (System.IO.File.Exists(Server.MapPath("~" + banner.ImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath("~" + banner.ImagePath));
                }

                status = 1;
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UpdateBanner(HomeBanner banner, HttpPostedFileBase file)
        {
            var status = (Object)null;

            if (!ModelState.IsValid)
            {
                status = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
                return Json(new { errCode = 401, status }, JsonRequestBehavior.AllowGet);

            }

            HomeBanner existingBanner = _context.HomeBanners.Where(w => w.ID == banner.ID).FirstOrDefault();


            string uploadImagePath = "";


            //If new file uploaded then delete old file and save new file.
            if (file != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                uploadImagePath = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + extension;
                banner.ImagePath = "/Uploads/Banner/" + uploadImagePath;

                if (System.IO.File.Exists(Server.MapPath("~" + existingBanner.ImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath("~" + existingBanner.ImagePath));
                }

                //save the file
                try
                {
                    file.SaveAs(Server.MapPath("~/Uploads/Banner/") + uploadImagePath);
                }
                catch (IOException exc)
                {
                    status = exc.Message;
                    return Json(new { status }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                banner.ImagePath = existingBanner.ImagePath;
            }

            try
            {
                if (banner.Description.ToLower() == "null".ToLower())
                    banner.Description = null;
                if (banner.Url.ToLower() == "null".ToLower())
                    banner.Url = null;
                banner.CreatedOn = existingBanner.CreatedOn;
                banner.CreatedBy = existingBanner.CreatedBy;
                banner.UpdatedOn = System.DateTime.Now;
                banner.UpdatedBy = User.Identity.Name;

                _context.Entry(existingBanner).State = EntityState.Detached;
                _context.HomeBanners.Attach(banner);
                _context.Entry(banner).State = EntityState.Modified;
                _context.SaveChanges();
                status = 1;
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(new { status }, JsonRequestBehavior.AllowGet);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteSelectedHomeBanners(List<int> Ids)
        {
            var status = (Object)null;

            if (!ModelState.IsValid)
            {
                status = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
                return Json(new { errCode = 401, status }, JsonRequestBehavior.AllowGet);

            }

            try
            {
                // Select all the records to be deleted
                IEnumerable<HomeBanner> list = _context.HomeBanners.Where(x => Ids.Contains(x.ID)).ToList();

                //Delete Selected Data Images
                foreach (var single in list)
                {
                    if (System.IO.File.Exists(Server.MapPath("~" + single.ImagePath)))
                    {
                        System.IO.File.Delete(Server.MapPath("~" + single.ImagePath));
                    }
                }

                // Use Remove Range function to delete all records at once
                _context.HomeBanners.RemoveRange(list);
                // Save changes
                _context.SaveChanges();
                status = 1;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    status = ex.InnerException.InnerException.Message;
                else
                    status = ex.Message;
            }

            return Json(new { status }, JsonRequestBehavior.AllowGet);
        }

    }
}
using DemoWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Controllers
{
    [RoutePrefix("SubCategory")]
    public class SubCategoryController : Controller
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveSubCategory(SubCategory subCategory)
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
                subCategory.CreatedOn = System.DateTime.Now;
                subCategory.UpdatedOn = System.DateTime.Now;
                subCategory.CreatedBy = User.Identity.Name;
                subCategory.UpdatedBy = User.Identity.Name;
                _context.SubCategories.Add(subCategory);
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetSubCategories(string term, int? page, int? pageSize)
        {
            List<SubCategoryViewModel> subCategories = null;
            int totalCount = 0;

            try
            {
                IQueryable<SubCategoryViewModel> result = _context.SubCategories
                                                                .Select(s => new SubCategoryViewModel
                                                                {
                                                                    ID = s.ID,
                                                                    Title = s.Title,
                                                                    Sequence = s.Sequence,
                                                                    CategoryID = s.CategoryId,
                                                                    CategoryName = s.Category.Title,
                                                                });


                if (!string.IsNullOrEmpty(term))
                {
                    result = result.Where(w => w.Title.ToLower().Contains(term.ToLower()) 
                                            || w.CategoryName.ToLower().Contains(term.ToLower()))
                                    .OrderBy(o => o.CategoryID)
                                    .ThenBy(t => t.Sequence);
                }
                else
                {
                    result = result.OrderBy(o => o.CategoryID)
                                    .ThenBy(t => t.Sequence);
                }

                subCategories = result
                            .Skip(((int)page - 1) * (int)pageSize)
                            .Take((int)pageSize)
                            .ToList();

                totalCount = result.Count();
                
            }
            catch (Exception ex)
            {
                subCategories = null;
            }

            return Json(new { totalCount, subCategories }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetSubCategoryByID(int id)
        {
            SubCategoryViewModel subCategory = null;
            try
            {
                subCategory = _context.SubCategories.Where(w => w.ID == id)
                            .Select(s => new SubCategoryViewModel
                            {
                                ID = s.ID,
                                Title = s.Title,
                                Sequence = s.Sequence,
                                CategoryID = s.CategoryId,
                                CategoryName = s.Category.Title,
                            })
                            .FirstOrDefault();
            }
            catch (Exception ex)
            {
                subCategory = null;
            }

            return Json(new { subCategory }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult DeleteSubCategory(int id)
        {
            var status = (Object)null;
            try
            {
                var subCategory = _context.SubCategories.Where(w => w.ID == id).FirstOrDefault();
                _context.SubCategories.Attach(subCategory);
                _context.SubCategories.Remove(subCategory);
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
        public ActionResult UpdateSubCategory(SubCategory subCategory)
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
                SubCategory existingSubCategory = _context.SubCategories.Where(w => w.ID == subCategory.ID).FirstOrDefault();
                subCategory.CreatedOn = existingSubCategory.CreatedOn;
                subCategory.CreatedBy = existingSubCategory.CreatedBy;
                subCategory.UpdatedOn = System.DateTime.Now;
                subCategory.UpdatedBy = User.Identity.Name;

                _context.Entry(existingSubCategory).State = EntityState.Detached;
                _context.SubCategories.Attach(subCategory);
                _context.Entry(subCategory).State = EntityState.Modified;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetAllSubCategoriesByCategory(int? catId)
        {
            List<SubCategoryViewModel> subCategories = null;

            try
            {
                if (catId != null)
                {
                    subCategories = _context.SubCategories
                                    .Where(w => w.CategoryId == catId)
                                    .OrderBy(t => t.Sequence)
                                    .Select(s => new SubCategoryViewModel
                                    {
                                        ID = s.ID,
                                        Title = s.Title,
                                    })
                                    .ToList();
                }
            }
            catch
            {
                subCategories = null;
            }

            return Json(new { subCategories }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteSelectedSubCategories(List<int> Ids)
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
                IEnumerable<SubCategory> list = _context.SubCategories.Where(x => Ids.Contains(x.ID)).ToList();
                // Use Remove Range function to delete all records at once
                _context.SubCategories.RemoveRange(list);
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
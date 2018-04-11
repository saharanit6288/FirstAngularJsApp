using DemoWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Controllers
{
    [RoutePrefix("Category")]
    public class CategoryController : Controller
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveCategory(Category category)
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
                category.CreatedOn = System.DateTime.Now;
                category.UpdatedOn = System.DateTime.Now;
                category.CreatedBy = User.Identity.Name;
                category.UpdatedBy = User.Identity.Name;
                _context.Categories.Add(category);
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
        public ActionResult GetCategories(string term, int? page, int? pageSize)
        {
            List<CategoryViewModel> categories = null;
            int totalCount = 0;

            try
            {
                IQueryable<CategoryViewModel> result = _context.Categories
                                                                .Select(s => new CategoryViewModel
                                                                {
                                                                    ID = s.ID,
                                                                    Title = s.Title,
                                                                    Sequence = s.Sequence
                                                                });


                if (!string.IsNullOrEmpty(term))
                {
                    result = result.Where(w => w.Title.ToLower().Contains(term.ToLower())).OrderBy(o => o.Sequence);
                }
                else
                {
                    result = result.OrderBy(o => o.Sequence);
                }

                categories = result
                            .Skip(((int)page - 1) * (int)pageSize)
                            .Take((int)pageSize)
                            .ToList();

                totalCount = result.Count();
            }
            catch (Exception ex)
            {
                categories = null;
            }

            return Json(new { totalCount, categories }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetCategoryByID(int id)
        {
            CategoryViewModel category = null;
            try
            {
                category = _context.Categories.Where(w => w.ID == id)
                            .Select(s => new CategoryViewModel { ID = s.ID, Title = s.Title, Sequence = s.Sequence })
                            .FirstOrDefault();
            }
            catch (Exception ex)
            {
                category = null;
            }

            return Json(new { category }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult DeleteCategory(int id)
        {
            var status = (Object)null;
            try
            {
                var category = _context.Categories.Where(w => w.ID == id).FirstOrDefault();
                var subCategories = _context.SubCategories.Where(f => f.CategoryId == id).ToList();
                _context.SubCategories.RemoveRange(subCategories);
                _context.Categories.Attach(category);
                _context.Categories.Remove(category);
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
        public ActionResult UpdateCategory(Category category)
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
                Category existingCategory = _context.Categories.Where(w => w.ID == category.ID).FirstOrDefault();
                category.CreatedOn = existingCategory.CreatedOn;
                category.CreatedBy = existingCategory.CreatedBy;
                category.UpdatedOn = System.DateTime.Now;
                category.UpdatedBy = User.Identity.Name;

                _context.Entry(existingCategory).State = EntityState.Detached;
                _context.Categories.Attach(category);
                _context.Entry(category).State = EntityState.Modified;
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
        public ActionResult GetAllCategories()
        {
            List<CategoryViewModel> categories = null;

            try
            {
                categories = _context.Categories
                                    .OrderBy(o => o.Sequence)
                                    .Select(s => new CategoryViewModel
                                    {
                                        ID = s.ID,
                                        Title = s.Title,
                                    })
                                    .ToList();
                
            }
            catch (Exception ex)
            {
                categories = null;
            }

            return Json(new { categories }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteSelectedCategories(List<int> Ids)
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
                IEnumerable<Category> list = _context.Categories.Where(x => Ids.Contains(x.ID)).ToList();
                // Use Remove Range function to delete all records at once
                _context.Categories.RemoveRange(list);
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
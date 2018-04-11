using DemoWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Controllers
{
    public class ProductController : Controller
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveProduct(Product product, HttpPostedFileBase file)
        {
            var status = (Object)null;

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
                product.ImagePath = "/Uploads/Product/" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + extension;
                file.SaveAs(Server.MapPath("~/Uploads/Product/") + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + extension);
            }
            catch (IOException exc)
            {
                status = exc.Message;
                return Json(new { status }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                product.CreatedOn = System.DateTime.Now;
                product.UpdatedOn = System.DateTime.Now;
                product.CreatedBy = User.Identity.Name;
                product.UpdatedBy = User.Identity.Name;
                _context.Products.Add(product);
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
        public ActionResult GetProducts(string term, int? page, int? pageSize)
        {
            List<ProductViewModel> products = null;
            int totalCount = 0;

            try
            {
                IQueryable<ProductViewModel> result = _context.Products
                                                                .Select(s => new ProductViewModel
                                                                {
                                                                    ID = s.ID,
                                                                    CategoryId = s.CategoryId,
                                                                    SubCategoryId = s.SubCategoryId,
                                                                    Description = s.Description,
                                                                    ImagePath = s.ImagePath,
                                                                    IsOfferable = s.IsOfferable,
                                                                    OfferPrice = s.OfferPrice,
                                                                    OriginalPrice = s.OriginalPrice,
                                                                    Quantity = s.Quantity,
                                                                    Rating = s.Rating,
                                                                    Sequence = s.Sequence,
                                                                    Title = s.Title,
                                                                    CategoryName = s.SubCategory.Category.Title,
                                                                    SubCategoryName = s.SubCategory.Title,
                                                                    Discount = s.Discount
                                                                });


                if (!string.IsNullOrEmpty(term))
                {
                    result = result.Where(w => w.Title.ToLower().Contains(term.ToLower())
                                            || w.CategoryName.ToLower().Contains(term.ToLower())
                                            || w.SubCategoryName.ToLower().Contains(term.ToLower()))
                                    .OrderBy(o => o.CategoryId)
                                    .ThenBy(o => o.SubCategoryId)
                                    .ThenBy(t => t.Sequence);
                }
                else
                {
                    result = result.OrderBy(o => o.CategoryId)
                                    .ThenBy(o => o.SubCategoryId)
                                    .ThenBy(t => t.Sequence);
                }

                products = result
                            .Skip(((int)page - 1) * (int)pageSize)
                            .Take((int)pageSize)
                            .ToList();

                totalCount = result.Count();
                
            }
            catch (Exception ex)
            {
                products = null;
            }

            return Json(new { totalCount, products }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetProductByID(int id)
        {
            ProductViewModel product = null;
            try
            {
                product = _context.Products
                                    .Where(w => w.ID == id)
                                    .Select(s => new ProductViewModel
                                    {
                                        ID = s.ID,
                                        CategoryId = s.CategoryId,
                                        SubCategoryId = s.SubCategoryId,
                                        Description = s.Description,
                                        ImagePath = s.ImagePath,
                                        IsOfferable = s.IsOfferable,
                                        OfferPrice = s.OfferPrice,
                                        OriginalPrice = s.OriginalPrice,
                                        Quantity = s.Quantity,
                                        Rating = s.Rating,
                                        Sequence = s.Sequence,
                                        Title = s.Title,
                                        CategoryName = s.SubCategory.Category.Title,
                                        SubCategoryName = s.SubCategory.Title,
                                        Discount = s.Discount
                                    })
                                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                product = null;
            }

            return Json(new { product }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult DeleteProduct(int id)
        {
            var status = (Object)null;
            try
            {
                var product = _context.Products.Where(w => w.ID == id).FirstOrDefault();
                _context.Products.Attach(product);
                _context.Products.Remove(product);
                _context.SaveChanges();

                if (System.IO.File.Exists(Server.MapPath("~" + product.ImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath("~" + product.ImagePath));
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
        public ActionResult UpdateProduct(Product product, HttpPostedFileBase file)
        {
            var status = (Object)null;

            if (!ModelState.IsValid)
            {
                status = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
                return Json(new { errCode = 401, status }, JsonRequestBehavior.AllowGet);

            }

            Product existingProduct = _context.Products.Where(w => w.ID == product.ID).FirstOrDefault();


            string uploadImagePath = "";


            //If new file uploaded then delete old file and save new file.
            if (file != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                uploadImagePath = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + extension;
                product.ImagePath = "/Uploads/Product/" + uploadImagePath;

                if (System.IO.File.Exists(Server.MapPath("~" + existingProduct.ImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath("~" + existingProduct.ImagePath));
                }

                //save the file
                try
                {
                    file.SaveAs(Server.MapPath("~/Uploads/Product/") + uploadImagePath);
                }
                catch (IOException exc)
                {
                    status = exc.Message;
                    return Json(new { status }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                product.ImagePath = existingProduct.ImagePath;
            }

            try
            {
                if (product.Description.ToLower() == "null".ToLower())
                    product.Description = null;
                product.CreatedOn = existingProduct.CreatedOn;
                product.CreatedBy = existingProduct.CreatedBy;
                product.UpdatedOn = System.DateTime.Now;
                product.UpdatedBy = User.Identity.Name;

                _context.Entry(existingProduct).State = EntityState.Detached;
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                status = 1;
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(new { status }, JsonRequestBehavior.AllowGet);

        }
		
		[AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetSingleDetails(int id)
        {
            ProductViewModel product = null;
            try
            {
                product = _context.Products
                                    .Where(w => w.ID == id)
                                    .Select(s => new ProductViewModel
                                    {
                                        ID = s.ID,
                                        CategoryId = s.CategoryId,
                                        SubCategoryId = s.SubCategoryId,
                                        Description = s.Description,
                                        ImagePath = s.ImagePath,
                                        IsOfferable = s.IsOfferable,
                                        OfferPrice = s.OfferPrice,
                                        OriginalPrice = s.OriginalPrice,
                                        Quantity = s.Quantity,
                                        Rating = s.Rating,
                                        Sequence = s.Sequence,
                                        Title = s.Title,
                                        CategoryName = s.SubCategory.Category.Title,
                                        SubCategoryName = s.SubCategory.Title,
                                        Discount = s.Discount
                                    })
                                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                product = null;
            }

            return Json(new { product }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        [Route("Product/All/{type}/{id}")]
        public ActionResult All(ProductTypeEnum type, int id)
        {
            return View();
        }

        [AllowAnonymous]
        [Route("Product/GetAllDetails/{type}/{id}/{sort}/{page}/{pageSize}")]
        public ActionResult GetAllDetails(ProductTypeEnum type, int id, SortOrderEnum? sort, int? page, PageSizeEnum? pageSize)
        {
            List<ProductViewModel> products = null;
            int totalCount = 0;
            int newPageSize = 9;

            try
            {
                IQueryable<ProductViewModel> result = _context.Products
                                                        .Select(s => new ProductViewModel
                                                        {
                                                            ID = s.ID,
                                                            CategoryId = s.CategoryId,
                                                            SubCategoryId = s.SubCategoryId,
                                                            Description = s.Description,
                                                            ImagePath = s.ImagePath,
                                                            IsOfferable = s.IsOfferable,
                                                            OfferPrice = s.OfferPrice,
                                                            OriginalPrice = s.OriginalPrice,
                                                            Quantity = s.Quantity,
                                                            Rating = s.Rating,
                                                            Sequence = s.Sequence,
                                                            Title = s.Title,
                                                            CategoryName = s.SubCategory.Category.Title,
                                                            SubCategoryName = s.SubCategory.Title,
                                                            Discount = s.Discount
                                                        });


                switch (type)
                {
                    case ProductTypeEnum.Category:
                        result = result.Where(w => w.CategoryId == id);
                        break;
                    case ProductTypeEnum.SubCategory:
                        result = result.Where(w => w.SubCategoryId == id);
                        break;
                }

                switch (sort)
                {
                    case SortOrderEnum.Default:
                        result = result.OrderBy(o => o.ID);
                        break;
                    case SortOrderEnum.HighToLowPrice:
                        result = result.OrderByDescending(o => o.OfferPrice);
                        break;
                    case SortOrderEnum.LowToHighPrice:
                        result = result.OrderBy(o => o.OfferPrice);
                        break;
                    case SortOrderEnum.Rating:
                        result = result.OrderByDescending(o => o.Rating);
                        break;
                    case SortOrderEnum.Discount:
                        result = result.OrderByDescending(o => o.Discount);
                        break;
                }

                switch (pageSize)
                {
                    case PageSizeEnum.ItemPerPage9:
                        newPageSize = newPageSize * 1;
                        break;
                    case PageSizeEnum.ItemPerPage18:
                        newPageSize = newPageSize * 2;
                        break;
                    case PageSizeEnum.ItemPerPage32:
                        newPageSize = newPageSize * 4;
                        break;
                    case PageSizeEnum.All:
                        newPageSize = result.ToList().Count();
                        break;
                }

                products = result
                            .Skip(((int)page - 1) * newPageSize)
                            .Take(newPageSize)
                            .ToList();

                totalCount = result.Count();

            }
            catch (Exception ex)
            {
                products = null;
            }

            return Json(new { totalCount, newPageSize, products }, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteSelectedProducts(List<int> Ids)
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
                IEnumerable<Product> list = _context.Products.Where(x => Ids.Contains(x.ID)).ToList();

                //Delete Selected Data Images
                foreach (var single in list)
                {
                    if (System.IO.File.Exists(Server.MapPath("~" + single.ImagePath)))
                    {
                        System.IO.File.Delete(Server.MapPath("~" + single.ImagePath));
                    }
                }

                // Use Remove Range function to delete all records at once
                _context.Products.RemoveRange(list);
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
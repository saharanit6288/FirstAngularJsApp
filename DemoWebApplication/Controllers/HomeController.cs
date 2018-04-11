using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Models;
using System.Data.Entity;

namespace DemoWebApplication.Controllers
{
    public class HomeController : Controller
    {
        DemoWebAppDBContext _context = new DemoWebAppDBContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult GetCategorySubCategoryList()
        {
            List<CategorywiseSubCategoryViewModel> categorywiseSubCategories = new List<CategorywiseSubCategoryViewModel>();

            categorywiseSubCategories = _context.Categories
                                                .OrderBy(t => t.Sequence)
                                                .Select(s => new CategorywiseSubCategoryViewModel
                                                {
                                                    ID = s.ID,
                                                    Title = s.Title,
                                                    SubCategories = _context.SubCategories
                                                                            .Where(w => w.CategoryId == s.ID)
                                                                            .OrderBy(o => o.Sequence)
                                                                            .Select(m => new SubCategoryInfoViewModel
                                                                            {
                                                                                ID = m.ID,
                                                                                Title = m.Title
                                                                            })
                                                                            .ToList()
                                                })
                                                .ToList();


            return Json(new { categorywiseSubCategories }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("Home/GetBannerByType/{bannerType}")]
        public ActionResult GetBannerByType(string bannerType)
        {
            List<HomeBanner> banners = _context.HomeBanners
                                                .Where(w => w.BannerType.ToLower() == bannerType.ToLower())
                                                .OrderBy(o=>o.Sequence)
                                                .Select(s => s)
                                                .ToList();

            return Json(new { banners }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("Home/GetNewProducts/{productCount}")]
        public ActionResult GetNewProducts(int productCount)
        {
            List<ProductViewModel> products = _context.Products
                                            .OrderByDescending(o => o.ID)
                                            .Take(productCount)
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
                                                Discount = s.Discount
                                            })
                                            .ToList();

            return Json(new { products }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("Home/GetWeeklyTopProducts/{productCount}")]
        public ActionResult GetWeeklyTopProducts(int productCount)
        {
            DateTime givenDate = DateTime.Today;
            DateTime startOfWeek = givenDate.AddDays(-1 * (int)givenDate.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            List<ProductViewModel> products = _context.Products
                                            .Where(w => DbFunctions.TruncateTime(w.UpdatedOn) >= startOfWeek
                                                    && DbFunctions.TruncateTime(w.UpdatedOn) < endOfWeek)
                                            .OrderByDescending(o => o.UpdatedOn)
                                            .Take(productCount)
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
                                                Discount = s.Discount
                                            })
                                            .ToList();

            return Json(new { products }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("Home/GetTopDiscountedProducts/{productCount}")]
        public ActionResult GetTopDiscountedProducts(int productCount)
        {
            List<ProductViewModel> products = _context.Products
                                            .Where(w => w.IsOfferable)
                                            .OrderByDescending(o => o.Discount)
                                            .Take(productCount)
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
                                                Discount = s.Discount
                                            })
                                            .ToList();

            return Json(new { products }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetSearchResult(string term)
        {
            List<ProductFilterViewModel> results = null;

            try
            {
                IEnumerable<ProductFilterViewModel> products = _context.Products
                                                                .Where(w => w.Title.ToLower().Contains(term.ToLower())
                                                                            || w.Description.ToLower().Contains(term.ToLower())
                                                                        )
                                                                .Select(s => new ProductFilterViewModel
                                                                {
                                                                    ID = s.ID,
                                                                    Title = s.Title,
                                                                    PageUrl = "/Product/Details/" + s.ID
                                                                })
                                                                .ToList();


                IEnumerable<ProductFilterViewModel> categories = _context.Categories
                                                                .Where(w => w.Title.ToLower().Contains(term.ToLower()))
                                                                .Select(s => new ProductFilterViewModel
                                                                {
                                                                    ID = s.ID,
                                                                    Title = s.Title,
                                                                    PageUrl = "/Product/All/Category/" + s.ID
                                                                })
                                                                .ToList();

                IEnumerable<ProductFilterViewModel> subCategories = _context.SubCategories
                                                                .Where(w => w.Title.ToLower().Contains(term.ToLower()))
                                                                .Select(s => new ProductFilterViewModel
                                                                {
                                                                    ID = s.ID,
                                                                    Title = s.Title,
                                                                    PageUrl = "/Product/All/SubCategory/" + s.ID
                                                                })
                                                                .ToList();

                results = products.Union(categories).Union(subCategories).ToList();
                
            }
            catch (Exception ex)
            {
                results = null;
            }

            return Json(new { results }, JsonRequestBehavior.AllowGet);
        }
    }
}
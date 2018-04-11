using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class CategorywiseSubCategoryViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<SubCategoryInfoViewModel> SubCategories { get; set; }
    }

    public class SubCategoryInfoViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
}
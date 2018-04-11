using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal OfferPrice { get; set; }
        public bool IsOfferable { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int Sequence { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
    }

    public enum ProductTypeEnum
    { 
        Category=1,
        SubCategory=2
    }

    public enum SortOrderEnum
    {
        Default = 1,
        HighToLowPrice = 2,
        LowToHighPrice = 3,
        Rating = 4,
        Discount = 5
    }

    public enum PageSizeEnum
    {
        ItemPerPage9 = 1,
        ItemPerPage18 = 2,
        ItemPerPage32 = 3,
        All = 4
    }

    public class ProductFilterViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string PageUrl { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class Product : BaseModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal OfferPrice { get; set; }
        public bool IsOfferable { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}
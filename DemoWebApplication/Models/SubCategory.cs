using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class SubCategory : BaseModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string Title { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual IList<Product> Products { get; set; }

    }
}
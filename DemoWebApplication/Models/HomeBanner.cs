using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class HomeBanner : BaseModel
    {
        public int ID { get; set; }

        [Required]
        public string BannerType { get; set; }

        [Required]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string Url { get; set; }
    }
}
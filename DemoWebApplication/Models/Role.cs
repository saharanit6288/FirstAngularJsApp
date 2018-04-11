using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class Role
    {
        public int ID { get; set; }

        [Required]
        [StringLength(150)]
        [Index(IsUnique = true)]
        [Display(Name = "Name: ")]
        public string Name { get; set; }
    }
}
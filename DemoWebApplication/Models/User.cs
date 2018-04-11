using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Index(IsUnique = true)]
        [Display(Name = "Email Address: ")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public string UserName { get; set; }

        [Display(Name = "First Name: ")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name: ")]
        public string LastName { get; set; }

        [Display(Name = "Contact No: ")]
        public string ContactNo { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public string IPAddress { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
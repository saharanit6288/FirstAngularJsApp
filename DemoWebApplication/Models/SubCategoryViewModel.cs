using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoWebApplication.Models
{
    public class SubCategoryViewModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public string CategoryName { get; set; }
    }
}
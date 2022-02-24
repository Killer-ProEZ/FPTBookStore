using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FPTBookStore.Models
{
    public class Orderdetail
    {
        [Key]
        public int OrderdetailsID { get; set; }
        [Required]
        public string BookID { get; set; }
        [Required]
        [Range(0, 100000000, ErrorMessage = "Please enter correct value")]
        public int Price { get; set; }
        [Required]
        [Range(0, 100000, ErrorMessage = "Please enter correct value")]
        public int Quality { get; set; }
        [Required]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}
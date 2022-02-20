using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPTBookStore.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        [Range(0, 100000000, ErrorMessage = "Please enter correct value")]
        public int TotalPrice { get; set; }
        [Required]
        public int UserName { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string Address { get; set; }
        public virtual Account Account{ get; set; }
        public ICollection<Orderdetail> Orderdetails{ get; set; }
    }
}
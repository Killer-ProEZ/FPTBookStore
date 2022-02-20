using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPTBookStore.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string Img { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        [Range(0,100000000, ErrorMessage= "Please enter correct value")]
        public int Price { get; set; }
        [Required]
        [Range(0, 100000000, ErrorMessage = "Please enter correct value")]
        public string Description { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{dd.MM.yyyy}")]
        public DateTime Date_add { get; set; }
        [Required]
        public int AuthorID { get; set; }
        [Required]
        public int CategoryID { get; set; }

        public virtual Author Author { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<Orderdetail> Orderdetails { get; set; }
    }
}
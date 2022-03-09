using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPTBookStore.Models
{
    public class Account
    {
        [Key]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(1000)]
        public string Password { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(1000)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string RePassword {get;set;}
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Tel { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Please enter correct value")]
        public int State { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
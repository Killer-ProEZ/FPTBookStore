using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTBookStore.Models
{
    public class Cart
    {
        public Book Book { get; set; }
        public int ProductID { get; set; }
        public int Quality { get; set; }
    }
}
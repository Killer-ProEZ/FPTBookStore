using FPTBookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FPTBookStore.DB
{
    public class MyApplicationDBContext:DbContext
    {
        public MyApplicationDBContext():base("MyConnection")
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Order>  Orders{ get; set; }
        public DbSet<Orderdetail> Orderdetails { get; set; }

        public DbSet<Author> Authors { get; set; }
    }
}
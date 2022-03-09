namespace FPTBookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        Fullname = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 1000),
                        RePassword = c.String(nullable: false, maxLength: 1000),
                        Email = c.String(nullable: false),
                        Tel = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        TotalPrice = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 128),
                        OrderDate = c.DateTime(nullable: false),
                        Address = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Accounts", t => t.UserName, cascadeDelete: true)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.Orderdetails",
                c => new
                    {
                        OrderdetailsID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderdetailsID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        BookName = c.String(nullable: false),
                        Img = c.String(),
                        Stock = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        Date_add = c.DateTime(nullable: false),
                        AuthorID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orderdetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orderdetails", "BookID", "dbo.Books");
            DropForeignKey("dbo.Books", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Books", "AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Orders", "UserName", "dbo.Accounts");
            DropIndex("dbo.Books", new[] { "CategoryID" });
            DropIndex("dbo.Books", new[] { "AuthorID" });
            DropIndex("dbo.Orderdetails", new[] { "OrderID" });
            DropIndex("dbo.Orderdetails", new[] { "BookID" });
            DropIndex("dbo.Orders", new[] { "UserName" });
            DropTable("dbo.Categories");
            DropTable("dbo.Authors");
            DropTable("dbo.Books");
            DropTable("dbo.Orderdetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Accounts");
        }
    }
}

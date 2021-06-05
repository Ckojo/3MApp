namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_installenments_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InstallenmentAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Paid = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstallenmentDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePaid = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PaymentDates", "Amount_Id", c => c.Int());
            AddColumn("dbo.PaymentDates", "Date_Id", c => c.Int());
            CreateIndex("dbo.PaymentDates", "Amount_Id");
            CreateIndex("dbo.PaymentDates", "Date_Id");
            AddForeignKey("dbo.PaymentDates", "Amount_Id", "dbo.InstallenmentAmounts", "Id");
            AddForeignKey("dbo.PaymentDates", "Date_Id", "dbo.InstallenmentDates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentDates", "Date_Id", "dbo.InstallenmentDates");
            DropForeignKey("dbo.PaymentDates", "Amount_Id", "dbo.InstallenmentAmounts");
            DropIndex("dbo.PaymentDates", new[] { "Date_Id" });
            DropIndex("dbo.PaymentDates", new[] { "Amount_Id" });
            DropColumn("dbo.PaymentDates", "Date_Id");
            DropColumn("dbo.PaymentDates", "Amount_Id");
            DropTable("dbo.InstallenmentDates");
            DropTable("dbo.InstallenmentAmounts");
        }
    }
}

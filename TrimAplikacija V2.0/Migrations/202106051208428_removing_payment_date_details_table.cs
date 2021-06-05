namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removing_payment_date_details_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentDateDetails", "PaymentDate_Id", "dbo.PaymentDates");
            DropIndex("dbo.PaymentDateDetails", new[] { "PaymentDate_Id" });
            DropTable("dbo.PaymentDateDetails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentDateDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstallenmentPaymentAmount = c.Double(nullable: false),
                        InstallenmentPurchaseDate = c.DateTime(nullable: false),
                        PaymentDate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PaymentDateDetails", "PaymentDate_Id");
            AddForeignKey("dbo.PaymentDateDetails", "PaymentDate_Id", "dbo.PaymentDates", "Id");
        }
    }
}

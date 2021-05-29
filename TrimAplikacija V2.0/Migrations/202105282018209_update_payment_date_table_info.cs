namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_payment_date_table_info : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentInstallenmentAmounts", "PaymentDate_Id", "dbo.PaymentDates");
            DropForeignKey("dbo.PaymentInstallenmentDates", "PaymentDate_Id", "dbo.PaymentDates");
            DropIndex("dbo.PaymentInstallenmentAmounts", new[] { "PaymentDate_Id" });
            DropIndex("dbo.PaymentInstallenmentDates", new[] { "PaymentDate_Id" });
            CreateTable(
                "dbo.PaymentDateDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstallenmentPaymentAmount = c.Double(nullable: false),
                        InstallenmentPurchaseDate = c.DateTime(nullable: false),
                        PaymentDate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentDates", t => t.PaymentDate_Id)
                .Index(t => t.PaymentDate_Id);
            
            DropTable("dbo.PaymentInstallenmentAmounts");
            DropTable("dbo.PaymentInstallenmentDates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentInstallenmentDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstallenmentPaymentDate = c.DateTime(nullable: false),
                        PaymentDate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentInstallenmentAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstallenmentAmount = c.Double(nullable: false),
                        PaymentDate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.PaymentDateDetails", "PaymentDate_Id", "dbo.PaymentDates");
            DropIndex("dbo.PaymentDateDetails", new[] { "PaymentDate_Id" });
            DropTable("dbo.PaymentDateDetails");
            CreateIndex("dbo.PaymentInstallenmentDates", "PaymentDate_Id");
            CreateIndex("dbo.PaymentInstallenmentAmounts", "PaymentDate_Id");
            AddForeignKey("dbo.PaymentInstallenmentDates", "PaymentDate_Id", "dbo.PaymentDates", "Id");
            AddForeignKey("dbo.PaymentInstallenmentAmounts", "PaymentDate_Id", "dbo.PaymentDates", "Id");
        }
    }
}

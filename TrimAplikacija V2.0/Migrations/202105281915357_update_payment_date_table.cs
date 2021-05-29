namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_payment_date_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentInstallenmentAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstallenmentAmount = c.Double(nullable: false),
                        PaymentDate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentDates", t => t.PaymentDate_Id)
                .Index(t => t.PaymentDate_Id);
            
            CreateTable(
                "dbo.PaymentInstallenmentDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstallenmentPaymentDate = c.DateTime(nullable: false),
                        PaymentDate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentDates", t => t.PaymentDate_Id)
                .Index(t => t.PaymentDate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentInstallenmentDates", "PaymentDate_Id", "dbo.PaymentDates");
            DropForeignKey("dbo.PaymentInstallenmentAmounts", "PaymentDate_Id", "dbo.PaymentDates");
            DropIndex("dbo.PaymentInstallenmentDates", new[] { "PaymentDate_Id" });
            DropIndex("dbo.PaymentInstallenmentAmounts", new[] { "PaymentDate_Id" });
            DropTable("dbo.PaymentInstallenmentDates");
            DropTable("dbo.PaymentInstallenmentAmounts");
        }
    }
}

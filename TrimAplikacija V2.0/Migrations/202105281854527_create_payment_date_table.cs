namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_payment_date_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Payment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .Index(t => t.Payment_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentDates", "Payment_Id", "dbo.Payments");
            DropIndex("dbo.PaymentDates", new[] { "Payment_Id" });
            DropTable("dbo.PaymentDates");
        }
    }
}

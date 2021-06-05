namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removing_columns_from_paymentdate_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentDates", "Amount_Id", "dbo.InstallenmentAmounts");
            DropForeignKey("dbo.PaymentDates", "Date_Id", "dbo.InstallenmentDates");
            DropIndex("dbo.PaymentDates", new[] { "Amount_Id" });
            DropIndex("dbo.PaymentDates", new[] { "Date_Id" });
            DropColumn("dbo.PaymentDates", "Amount_Id");
            DropColumn("dbo.PaymentDates", "Date_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentDates", "Date_Id", c => c.Int());
            AddColumn("dbo.PaymentDates", "Amount_Id", c => c.Int());
            CreateIndex("dbo.PaymentDates", "Date_Id");
            CreateIndex("dbo.PaymentDates", "Amount_Id");
            AddForeignKey("dbo.PaymentDates", "Date_Id", "dbo.InstallenmentDates", "Id");
            AddForeignKey("dbo.PaymentDates", "Amount_Id", "dbo.InstallenmentAmounts", "Id");
        }
    }
}

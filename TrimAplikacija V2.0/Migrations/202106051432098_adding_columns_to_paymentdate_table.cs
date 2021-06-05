namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_columns_to_paymentdate_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentDates", "Amount_Id", c => c.Int());
            AddColumn("dbo.PaymentDates", "InstallenmentDates_Id", c => c.Int());
            CreateIndex("dbo.PaymentDates", "Amount_Id");
            CreateIndex("dbo.PaymentDates", "InstallenmentDates_Id");
            AddForeignKey("dbo.PaymentDates", "Amount_Id", "dbo.InstallenmentAmounts", "Id");
            AddForeignKey("dbo.PaymentDates", "InstallenmentDates_Id", "dbo.InstallenmentDates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentDates", "InstallenmentDates_Id", "dbo.InstallenmentDates");
            DropForeignKey("dbo.PaymentDates", "Amount_Id", "dbo.InstallenmentAmounts");
            DropIndex("dbo.PaymentDates", new[] { "InstallenmentDates_Id" });
            DropIndex("dbo.PaymentDates", new[] { "Amount_Id" });
            DropColumn("dbo.PaymentDates", "InstallenmentDates_Id");
            DropColumn("dbo.PaymentDates", "Amount_Id");
        }
    }
}

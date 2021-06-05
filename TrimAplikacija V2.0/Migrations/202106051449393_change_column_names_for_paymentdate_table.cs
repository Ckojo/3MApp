namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_column_names_for_paymentdate_table : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PaymentDates", name: "Amount_Id", newName: "InstallenmentAmount_Id");
            RenameColumn(table: "dbo.PaymentDates", name: "InstallenmentDates_Id", newName: "InstallenmentDate_Id");
            RenameIndex(table: "dbo.PaymentDates", name: "IX_Amount_Id", newName: "IX_InstallenmentAmount_Id");
            RenameIndex(table: "dbo.PaymentDates", name: "IX_InstallenmentDates_Id", newName: "IX_InstallenmentDate_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PaymentDates", name: "IX_InstallenmentDate_Id", newName: "IX_InstallenmentDates_Id");
            RenameIndex(table: "dbo.PaymentDates", name: "IX_InstallenmentAmount_Id", newName: "IX_Amount_Id");
            RenameColumn(table: "dbo.PaymentDates", name: "InstallenmentDate_Id", newName: "InstallenmentDates_Id");
            RenameColumn(table: "dbo.PaymentDates", name: "InstallenmentAmount_Id", newName: "Amount_Id");
        }
    }
}

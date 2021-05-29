namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creating_payment_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        Installements = c.Int(nullable: false),
                        PurchaseAmount = c.Double(nullable: false),
                        InstallementAmount = c.Double(nullable: false),
                        TotalDebt = c.Double(nullable: false),
                        Paid = c.Double(nullable: false),
                        DebtLeft = c.Double(nullable: false),
                        Employee_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId)
                .Index(t => t.Employee_EmployeeId);
            
            DropColumn("dbo.Employees", "PurchaseDate");
            DropColumn("dbo.Employees", "Installements");
            DropColumn("dbo.Employees", "PurchaseAmount");
            DropColumn("dbo.Employees", "InstallementAmount");
            DropColumn("dbo.Employees", "TotalDebt");
            DropColumn("dbo.Employees", "Paid");
            DropColumn("dbo.Employees", "DebtLeft");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "DebtLeft", c => c.Double(nullable: false));
            AddColumn("dbo.Employees", "Paid", c => c.Double(nullable: false));
            AddColumn("dbo.Employees", "TotalDebt", c => c.Double(nullable: false));
            AddColumn("dbo.Employees", "InstallementAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Employees", "PurchaseAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Employees", "Installements", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "PurchaseDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Payments", "Employee_EmployeeId", "dbo.Employees");
            DropIndex("dbo.Payments", new[] { "Employee_EmployeeId" });
            DropTable("dbo.Payments");
        }
    }
}

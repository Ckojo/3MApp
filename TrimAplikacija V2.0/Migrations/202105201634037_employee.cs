namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(),
                        TIN = c.String(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IdNumber = c.String(),
                        UniqueNumber = c.String(),
                        PurchaseDate = c.DateTime(nullable: false),
                        Installements = c.Int(nullable: false),
                        PurchaseAmount = c.Double(nullable: false),
                        InstallementAmount = c.Double(nullable: false),
                        TotalDebt = c.Double(nullable: false),
                        Paid = c.Double(nullable: false),
                        DebtLeft = c.Double(nullable: false),
                        CompanyId_CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Companies", t => t.CompanyId_CompanyId)
                .Index(t => t.CompanyId_CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "CompanyId_CompanyId", "dbo.Companies");
            DropIndex("dbo.Employees", new[] { "CompanyId_CompanyId" });
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
        }
    }
}

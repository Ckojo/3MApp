namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class set_company_fk_to_required : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.Employees", new[] { "Company_CompanyId" });
            AlterColumn("dbo.Employees", "Company_CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "Company_CompanyId");
            AddForeignKey("dbo.Employees", "Company_CompanyId", "dbo.Companies", "CompanyId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.Employees", new[] { "Company_CompanyId" });
            AlterColumn("dbo.Employees", "Company_CompanyId", c => c.Int());
            CreateIndex("dbo.Employees", "Company_CompanyId");
            AddForeignKey("dbo.Employees", "Company_CompanyId", "dbo.Companies", "CompanyId");
        }
    }
}

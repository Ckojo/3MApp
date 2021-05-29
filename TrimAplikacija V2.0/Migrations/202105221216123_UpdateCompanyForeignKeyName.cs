namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCompanyForeignKeyName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Employees", name: "CompanyId_CompanyId", newName: "Company_CompanyId");
            RenameIndex(table: "dbo.Employees", name: "IX_CompanyId_CompanyId", newName: "IX_Company_CompanyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Employees", name: "IX_Company_CompanyId", newName: "IX_CompanyId_CompanyId");
            RenameColumn(table: "dbo.Employees", name: "Company_CompanyId", newName: "CompanyId_CompanyId");
        }
    }
}

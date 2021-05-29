namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_company_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Email", c => c.String(maxLength: 255));
            AddColumn("dbo.Companies", "TotalDebt", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "TotalDebt");
            DropColumn("dbo.Companies", "Email");
        }
    }
}

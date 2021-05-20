namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_companies_table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Companies", "City", c => c.String(maxLength: 255));
            AlterColumn("dbo.Companies", "TIN", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "TIN", c => c.String());
            AlterColumn("dbo.Companies", "City", c => c.String());
            AlterColumn("dbo.Companies", "Name", c => c.String());
        }
    }
}

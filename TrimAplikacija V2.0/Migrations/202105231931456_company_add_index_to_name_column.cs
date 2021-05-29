namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class company_add_index_to_name_column : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Companies", "Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", new[] { "Name" });
        }
    }
}

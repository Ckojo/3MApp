namespace TrimAplikacija_V2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_employee_table : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "FirstName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Employees", "LastName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Employees", "IdNumber", c => c.String(maxLength: 255));
            AlterColumn("dbo.Employees", "UniqueNumber", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "UniqueNumber", c => c.String());
            AlterColumn("dbo.Employees", "IdNumber", c => c.String());
            AlterColumn("dbo.Employees", "LastName", c => c.String());
            AlterColumn("dbo.Employees", "FirstName", c => c.String());
        }
    }
}

namespace LexiconLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Epost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Epost", c => c.String());
        }
    }
}

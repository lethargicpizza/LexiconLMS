namespace LexiconLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FörstaMigrationHotFix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aktivitets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Namn = c.String(),
                        StartTid = c.DateTime(nullable: false),
                        SlutTid = c.Time(nullable: false, precision: 7),
                        AktivitetsTypId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AktivitetsTyps", t => t.AktivitetsTypId)
                .Index(t => t.AktivitetsTypId);
            
            CreateTable(
                "dbo.AktivitetsTyps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Typ = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Namn = c.String(),
                        Beskrivning = c.String(),
                        StartDatum = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Moduls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Namn = c.String(),
                        Beskrivning = c.String(),
                        StartDatum = c.DateTime(nullable: false),
                        SlutDatum = c.DateTime(nullable: false),
                        KursId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kurs", t => t.KursId)
                .Index(t => t.KursId);
            
            CreateIndex("dbo.AspNetUsers", "KursId");
            AddForeignKey("dbo.AspNetUsers", "KursId", "dbo.Kurs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moduls", "KursId", "dbo.Kurs");
            DropForeignKey("dbo.AspNetUsers", "KursId", "dbo.Kurs");
            DropForeignKey("dbo.Aktivitets", "AktivitetsTypId", "dbo.AktivitetsTyps");
            DropIndex("dbo.Moduls", new[] { "KursId" });
            DropIndex("dbo.AspNetUsers", new[] { "KursId" });
            DropIndex("dbo.Aktivitets", new[] { "AktivitetsTypId" });
            DropTable("dbo.Moduls");
            DropTable("dbo.Kurs");
            DropTable("dbo.AktivitetsTyps");
            DropTable("dbo.Aktivitets");
        }
    }
}

namespace LexiconLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FörstaMig : DbMigration
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
                        Modul_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AktivitetsTyps", t => t.AktivitetsTypId)
                .ForeignKey("dbo.Moduls", t => t.Modul_Id)
                .Index(t => t.AktivitetsTypId)
                .Index(t => t.Modul_Id);
            
            CreateTable(
                "dbo.AktivitetsTyps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Typ = c.String(),
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
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FörNamn = c.String(),
                        EfterNamn = c.String(),
                        Email = c.String(maxLength: 256),
                        KursId = c.Int(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kurs", t => t.KursId)
                .Index(t => t.KursId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Moduls", "KursId", "dbo.Kurs");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "KursId", "dbo.Kurs");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Aktivitets", "Modul_Id", "dbo.Moduls");
            DropForeignKey("dbo.Aktivitets", "AktivitetsTypId", "dbo.AktivitetsTyps");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "KursId" });
            DropIndex("dbo.Moduls", new[] { "KursId" });
            DropIndex("dbo.Aktivitets", new[] { "Modul_Id" });
            DropIndex("dbo.Aktivitets", new[] { "AktivitetsTypId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Kurs");
            DropTable("dbo.Moduls");
            DropTable("dbo.AktivitetsTyps");
            DropTable("dbo.Aktivitets");
        }
    }
}

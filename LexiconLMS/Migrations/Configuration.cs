namespace LexiconLMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.SqlServer;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LexiconLMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LexiconLMS.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = new ApplicationUser();
            string lösenord = "123qwe";
            user.UserName = "elev@lms.se";
            user.Email = "elev@lms.se";
            user.FörNamn = "Leif";
            user.EfterNamn = "Den Store";

            if (userManager.FindByName(user.UserName) == null)
            {
                IdentityResult resultat = userManager.Create(user, lösenord);
                if (resultat.Succeeded)
                {
                    throw new Exception(string.Join("\n", resultat.Errors));
                }
            }

            string[] förnamn = { "Adrian", "Bertil", "Conny", "Per", "Ramus", "Olle", "Thomas", "Johan", "Dmitris" };
            string[] efternamn = { "Ahlberg", "Anderberg", "Ahlin", "Adamsson", "Cederberg", "Bylund", "Classon", "Falk", "Fahlgren" };

            int numFörnamn = förnamn.Count();
            int numEfternamn = efternamn.Count();

            int numAnvändare = 40;
            int kursId = 1;

            Random random = new Random();
            for (int i = 0; i < numAnvändare; i++)
            {
                var användare = new ApplicationUser();
                användare.FörNamn = förnamn[random.Next(numFörnamn)];
                användare.EfterNamn = efternamn[random.Next(numEfternamn)];
                användare.Email = $"{user.FörNamn}.{user.EfterNamn}@lms.se";
                användare.UserName = user.Email;
                användare.KursId = kursId;

                userManager.Create(användare, lösenord);
            }

            var kurser = new Kurs[]
            {
                new Kurs { Namn = ".NET 2015", Beskrivning = "Grundläggande C# inlärning", StartDatum = new DateTime(2015,11,9) },
                new Kurs { Namn = "Java 2015", Beskrivning = "Grundläggande Java", StartDatum = new DateTime(2015,11,9) },
                new Kurs { Namn = ".NET 2016", Beskrivning = "Grundläggande C# inlärning", StartDatum = new DateTime(2016,3,2) },
                new Kurs { Namn = "Java 2016", Beskrivning = "Grundläggande Java", StartDatum = new DateTime(2016,4,10) },
                new Kurs { Namn = "Realtids Projekt", Beskrivning = "Coola realtidsprojekt stuff", StartDatum = new DateTime(2015,9,5) }
            };
            context.Kurser.AddOrUpdate(kurser);

            var moduler = new Modul[]
            {
                new Modul { Namn = "C#", Beskrivning = "Lär dig C#", StartDatum = new DateTime(2015,9,10), SlutDatum = new DateTime(2015,10,9) },
                new Modul { Namn = "MVC", Beskrivning = "Episka MVC inlärning", StartDatum = new DateTime(2015,10,10), SlutDatum = new DateTime(2015,10,20) },
                new Modul { Namn = "Frontend", Beskrivning = "Lär dig Frontend", StartDatum = new DateTime(2015,10,21), SlutDatum = new DateTime(2015,11,2) },
                new Modul { Namn = "Testning", Beskrivning = "Lär dig Testning med Johan", StartDatum = new DateTime(2015,11,3), SlutDatum = new DateTime(2015,11,10) },
                new Modul { Namn = "Projekt", Beskrivning = "Lär dig Projekt med Adrian", StartDatum = new DateTime(2015,11,11), SlutDatum = new DateTime(2015,11,30) },
            };
            context.Moduler.AddOrUpdate(moduler);

            var aktivitetsTyper = new AktivitetsTyp[]
            {
                new AktivitetsTyp { Typ = "E-learning" },
                new AktivitetsTyp { Typ = "Föreläsning" },
                new AktivitetsTyp { Typ = "Övning" },
             };
            context.AktivitetsTyper.AddOrUpdate(aktivitetsTyper);

            var aktiviteter = new Aktivitet[]
            {
                    new Aktivitet { Namn = "C# med Scott Alan", StartTid = new DateTime(2015,9,10, 8,30,0), SlutTid = new TimeSpan(12,0,0)},
                    new Aktivitet { Namn = "C#", StartTid = new DateTime(2015,9,10, 13,0,0), SlutTid = new TimeSpan(17,0,0)},
                    new Aktivitet { Namn = "Garage", StartTid = new DateTime(2015,10,10, 8,30,0), SlutTid = new TimeSpan(12,0,0)},
                    new Aktivitet { Namn = "Garage med Adrian", StartTid = new DateTime(2015,10,10, 13,0,0), SlutTid = new TimeSpan(17,0,0)},
            };
            context.Aktiviteter.AddOrUpdate(aktiviteter);
        }
    }
}
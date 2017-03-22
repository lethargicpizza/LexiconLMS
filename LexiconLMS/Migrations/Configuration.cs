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
            var kurser = new Kurs[]
            {
                new Kurs { Namn = ".NET 2015", Beskrivning = "Grundl�ggande C# inl�rning", StartDatum = new DateTime(2015,11,9) },
                new Kurs { Namn = "Java 2015", Beskrivning = "Grundl�ggande Java", StartDatum = new DateTime(2015,11,9) },
                new Kurs { Namn = ".NET 2016", Beskrivning = "Grundl�ggande C# inl�rning", StartDatum = new DateTime(2016,3,2) },
                new Kurs { Namn = "Java 2016", Beskrivning = "Grundl�ggande Java", StartDatum = new DateTime(2016,4,10) },
                new Kurs { Namn = "Realtids Projekt", Beskrivning = "Coola realtidsprojekt stuff", StartDatum = new DateTime(2015,9,5) }
            };
            context.Kurser.AddOrUpdate(kurser);
            context.SaveChanges();

            var moduler = new Modul[]
            {
                new Modul { Namn = "C#", Beskrivning = "L�r dig C#", StartDatum = new DateTime(2015,9,10), SlutDatum = new DateTime(2015,10,9) },
                new Modul { Namn = "MVC", Beskrivning = "Episka MVC inl�rning", StartDatum = new DateTime(2015,10,10), SlutDatum = new DateTime(2015,10,20) },
                new Modul { Namn = "Frontend", Beskrivning = "L�r dig Frontend", StartDatum = new DateTime(2015,10,21), SlutDatum = new DateTime(2015,11,2) },
                new Modul { Namn = "Testning", Beskrivning = "L�r dig Testning med Johan", StartDatum = new DateTime(2015,11,3), SlutDatum = new DateTime(2015,11,10) },
                new Modul { Namn = "Projekt", Beskrivning = "L�r dig Projekt med Adrian", StartDatum = new DateTime(2015,11,11), SlutDatum = new DateTime(2015,11,30) },
            };
            context.Moduler.AddOrUpdate(moduler);
            context.SaveChanges();

            var aktivitetsTyper = new AktivitetsTyp[]
            {
                new AktivitetsTyp { Typ = "E-learning" },
                new AktivitetsTyp { Typ = "F�rel�sning" },
                new AktivitetsTyp { Typ = "�vning" },
             };
            context.AktivitetsTyper.AddOrUpdate(aktivitetsTyper);
            context.SaveChanges();

            var aktiviteter = new Aktivitet[]
            {
                    new Aktivitet { Namn = "C# med Scott Alan", StartTid = new DateTime(2015,9,10, 8,30,0), SlutTid = new TimeSpan(12,0,0)},
                    new Aktivitet { Namn = "C#", StartTid = new DateTime(2015,9,10, 13,0,0), SlutTid = new TimeSpan(17,0,0)},
                    new Aktivitet { Namn = "Garage", StartTid = new DateTime(2015,10,10, 8,30,0), SlutTid = new TimeSpan(12,0,0)},
                    new Aktivitet { Namn = "Garage med Adrian", StartTid = new DateTime(2015,10,10, 13,0,0), SlutTid = new TimeSpan(17,0,0)},
            };
            context.Aktiviteter.AddOrUpdate(aktiviteter);
            context.SaveChanges();

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            string l�senord = "123qwe";

            ApplicationUser anv�ndare = new ApplicationUser();
            anv�ndare.UserName = "elev@lms.se";
            anv�ndare.Email = "elev@lms.se";
            anv�ndare.F�rNamn = "Leif";
            anv�ndare.EfterNamn = "Den Store";

            if (userManager.FindByName(anv�ndare.UserName) == null)
            {
                IdentityResult resultat = userManager.Create(anv�ndare, l�senord);
                if (!resultat.Succeeded)
                {
                    throw new Exception(string.Join("\n", resultat.Errors));
                }
            }

            string[] f�rnamn = { "Adrian", "Bertil", "Conny", "Per", "Ramus", "Olle", "Thomas", "Johan", "Dmitris" };
            string[] efternamn = { "Ahlberg", "Anderberg", "Ahlin", "Adamsson", "Cederberg", "Bylund", "Classon", "Falk", "Fahlgren" };

            int numF�rnamn = f�rnamn.Count() - 1;
            int numEfternamn = efternamn.Count() - 1;

            int numAnv�ndare = 40;
            int kursId = 1;

            Random random = new Random();
            for (int i = 0; i < numAnv�ndare; i++)
            {
                anv�ndare = new ApplicationUser();
                anv�ndare.F�rNamn = f�rnamn[random.Next(1, numF�rnamn)];
                anv�ndare.EfterNamn = efternamn[random.Next(1, numEfternamn)];
                anv�ndare.Email = $"{anv�ndare.F�rNamn}.{anv�ndare.EfterNamn}@lms.se";
                anv�ndare.UserName = anv�ndare.Email;
                anv�ndare.KursId = kursId;

                if (userManager.FindByName(anv�ndare.UserName) == null)
                {
                    IdentityResult resultat = userManager.Create(anv�ndare, l�senord);
                    if (!resultat.Succeeded)
                    {
                        throw new Exception(string.Join("\n", resultat.Errors));
                    }
                }
            }
        }
    }
}
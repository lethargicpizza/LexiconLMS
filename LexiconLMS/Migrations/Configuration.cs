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
                //1-3
                new Kurs { Namn = ".NET 2017", Beskrivning = "Grundl�ggande C# inl�rning", StartDatum = new DateTime(2016,12,9) },
                new Kurs { Namn = "Java 2017", Beskrivning = "Grundl�ggande Java", StartDatum = new DateTime(2016,12,9) },
                new Kurs { Namn = "Realtids Projekt", Beskrivning = "Grundl�ggande C# inl�rning", StartDatum = new DateTime(2015,3,2) },
            };
            context.Kurser.AddOrUpdate(kurser);
            context.SaveChanges();

            Modul[] moduler = new Modul[]
            {
                //1-5
                new Modul { Namn = "C#", Beskrivning = "L�r dig C#", StartDatum = new DateTime(2017,1,1), SlutDatum = new DateTime(2017,2,1), KursId = 1 },
                new Modul { Namn = "Frontend", Beskrivning = "L�r dig Frontend", StartDatum = new DateTime(2017,2,1), SlutDatum = new DateTime(2017,3,1), KursId = 1 },
                new Modul { Namn = "MVC", Beskrivning = "Episka MVC inl�rning", StartDatum = new DateTime(2017,3,1), SlutDatum = new DateTime(2017,4,1), KursId = 1 },
                new Modul { Namn = "Testning", Beskrivning = "L�r dig Testning med Johan", StartDatum = new DateTime(2017,4,1), SlutDatum = new DateTime(2017,5,1), KursId = 1 },
                new Modul { Namn = "Projekt", Beskrivning = "L�r dig Projekt med Adrian", StartDatum = new DateTime(2017,5,1), SlutDatum = new DateTime(2017,6,1), KursId = 1 },
                //6-8
                new Modul { Namn = "Java", Beskrivning = "L�r dig Java", StartDatum = new DateTime(2017,1,1), SlutDatum = new DateTime(2017,2,10), KursId = 2 },
                new Modul { Namn = "Frontend", Beskrivning = "L�r dig frontend med Java!", StartDatum = new DateTime(2017,3,1), SlutDatum = new DateTime(2017,4,30), KursId = 2},
                new Modul { Namn = "JUnit", Beskrivning = "L�r dig testa med Java!", StartDatum = new DateTime(2017,5,1), SlutDatum = new DateTime(2017,5,10), KursId = 2 },
                //9-11
                new Modul { Namn = "Planering", Beskrivning = "L�r dig planera med John!", StartDatum = new DateTime(2015,3,2), SlutDatum = new DateTime(2015,3,10), KursId = 3 },
                new Modul { Namn = "Projekt i C#", Beskrivning = "L�r dig realtidsprojekt med C#", StartDatum = new DateTime(2015,4,1), SlutDatum = new DateTime(2015,4,20), KursId = 3 },
                new Modul { Namn = "Debugging", Beskrivning = "Realtids debugging", StartDatum = new DateTime(2015,5,1), SlutDatum = new DateTime(2015,5,10), KursId = 3 },
            };
            context.Moduler.AddOrUpdate(moduler);
            context.SaveChanges();

            var aktivitetsTyper = new AktivitetsTyp[]
            {
                //1-3
                new AktivitetsTyp { Typ = "E-learning" },
                new AktivitetsTyp { Typ = "F�rel�sning" },
                new AktivitetsTyp { Typ = "�vning" },
             };
            context.AktivitetsTyper.AddOrUpdate(aktivitetsTyper);
            context.SaveChanges();

            string[] cAktivitetsNamn = { "C#", "Kaffeparty", "Garage", "Git", "Javascript", "HTML", "CSS", "Angular" };
            int numAktivitetsNamn = cAktivitetsNamn.Count() - 1;

            List<Aktivitet> aktiviteter = new List<Aktivitet>();
            Random random = new Random();
            //Seed past month, this month, and next month
            foreach (var modul in moduler)
            {
                int startIntervall = modul.StartDatum.DayOfYear;
                int slutIntervall = modul.SlutDatum.DayOfYear;
                DateTime datum = modul.StartDatum;
                for (int i = startIntervall; i < slutIntervall; i++)
                {
                    string namn = cAktivitetsNamn[random.Next(1, numAktivitetsNamn)];
                    int aktivitetsTyp = random.Next(1,4);
                    aktiviteter.Add(new Aktivitet { Namn = namn, StartTid = new DateTime(datum.Year, datum.Month, datum.Day, 8, 30, 0), SlutTid = new TimeSpan(12, 0, 0), ModulId = modul.Id, AktivitetsTypId = random.Next(1, 4) });
                    namn = cAktivitetsNamn[random.Next(1, numAktivitetsNamn)];
                    aktiviteter.Add(new Aktivitet { Namn = namn, StartTid = new DateTime(datum.Year, datum.Month, datum.Day, 13, 0, 0), SlutTid = new TimeSpan(17, 0, 0), ModulId = modul.Id, AktivitetsTypId = random.Next(1, 4) });
                    datum = datum.AddDays(1);
                }
            }
            context.Aktiviteter.AddOrUpdate(aktiviteter.ToArray());
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
            anv�ndare.KursId = 1;

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

            for (int i = 0; i < numAnv�ndare; i++)
            {
                anv�ndare = new ApplicationUser();
                anv�ndare.F�rNamn = f�rnamn[random.Next(1, numF�rnamn)];
                anv�ndare.EfterNamn = efternamn[random.Next(1, numEfternamn)];
                anv�ndare.Email = $"{anv�ndare.F�rNamn.ToLower()}.{anv�ndare.EfterNamn.ToLower()}@lms.se";
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
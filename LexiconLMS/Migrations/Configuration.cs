namespace LexiconLMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LexiconLMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LexiconLMS.Models.ApplicationDbContext context)
        {
            CreateRoles(context);
            CreateUsers(context);

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

        private static void CreateUsers(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = new ApplicationUser();
            user.UserName = "lärare@lms.se";
            user.Email = user.UserName;
            user.FörNamn = "Per";
            user.EfterNamn = "Nordenbrink";
            var lösenord = "123qwe";

            IdentityResult result;
            if (userManager.FindByName(user.UserName) != null)
            {
                userManager.Update(user);
                var hashadLösen = userManager.PasswordHasher.HashPassword(lösenord);
                userStore.SetPasswordHashAsync(user, hashadLösen);
            }
            else
            {
                result = userManager.Create(user, lösenord);
                if (!userManager.Create(user, lösenord).Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            user = new ApplicationUser();
            user.UserName = "elev@lms.se";
            user.Email = user.UserName;
            user.FörNamn = "O'Leif";
            user.EfterNamn = "Den Store";

            if (userManager.FindByName(user.UserName) != null)
            {
                userManager.Update(user);
                var hashedPw = userManager.PasswordHasher.HashPassword(lösenord);
                userStore.SetPasswordHashAsync(user, hashedPw);
            }

            else
            {
                result = userManager.Create(user, lösenord);
                if (!userManager.Create(user, lösenord).Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }

            var lärare = userManager.FindByName("larare@lms.se");
            userManager.AddToRole(lärare.Id, "Lärare");

            var elev = userManager.FindByName("elev@lms.se");
        }

        private static void CreateRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("Lärare"))
            {
                var role = new IdentityRole();
                role.Name = "Lärare";
                roleManager.Create(role);
            }
        }
    }
}

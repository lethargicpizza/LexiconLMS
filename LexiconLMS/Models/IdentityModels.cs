using System.Data.Entity;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LexiconLMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Namn", FullNamn));


            return userIdentity;
        }

        [Display(Name ="Förnamn")]
        public string FörNamn { get; set; }

        [Display(Name = "Efternamn")]
        public string EfterNamn { get; set; }

        [Display(Name = "Namn")]
        public string FullNamn { get { return FörNamn + " " + EfterNamn; } }

        [Display(Name ="E-postadress")]
        public override string Email  { get; set; }

        public int? KursId { get; set; }

        public virtual Kurs Kurs { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Kurs> Kurser { get; set; }
        public DbSet<Modul> Moduler { get; set; }
        public DbSet<Aktivitet> Aktiviteter { get; set; }
        public DbSet<AktivitetsTyp> AktivitetsTyper { get; set; }

       
    }
}
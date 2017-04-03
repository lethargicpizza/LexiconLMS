using LexiconLMS.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LexiconLMS.Models
{
    public class AccountEditViewModel
    {
        public string Id { get; set; }


        [Password]
        [StringLength(30, ErrorMessage = "Ett lösenord behöver vara mellan 6 till 30 tecken långt!" , MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Required]
        [Display( Name = "E-post")]
        public string Epost { get; set; }

        [Required]
        public string Förnamn { get; set; }

        [Required]
        public string Efternamn { get; set; }

        [Display(Name = "Är lärare")]
        public bool ÄrLärare { get; set; }

        [Display(Name = "Kurs")]
        public int? KursId { get; set; }

        public System.Web.Mvc.SelectList Kurser { get; set; }
    }

    public class AccountIndexViewModel
    {
        public string Id { get; set; }
        [Display(Name ="E-post")]
        public string Epost { get; set; }

        [Display(Name = "Namn")]
        public string FullNamn { get; set; }

        [Display(Name = "Roll")]
        public bool ÄrLärare { get; set; }

        [DisplayFormat(NullDisplayText = "Ej deltagande")]
        [Display(Name = "Kurs")]
        public string Kursnamn { get; set; }
    }

    public class AccountDetailViewModel
    {
        public string Id { get; set; }

        public string Epost { get; set; }

        [Display(Name = "Namn")]
        public string FullNamn { get; set; }

        [Display(Name = "Roll")]
        public string Roll { get; set; }

        [Display(Name = "Kurs")]
        public string Kursnamn { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-post")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "A name may have max {0} characters long.")]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        public int? KursId { get; set; }

        [Display(Name = "Är lärare")]
        public bool ÄrLärare { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Ni har matat in två olika lösenord!")]
        public string ConfirmPassword { get; set; }
    }


    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [Display(Name = "Kom ihåg mig?")]
        public bool RememberMe { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-postadress")]
        public string Email { get; set; }
    }
}

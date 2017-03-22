using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class Klasslista
    {
        //[Display(Name = "Förnamn")]
        //public string FörNamn { get; set; }

        //[Display(Name = "Efternamn")]
        //public string EfterNamn { get; set; }

        [Display(Name = "Namn")]
        public string FullNamn { get; set; }

        [Display(Name ="E-postadress")]
        public string Email { get; set; }
    }
}
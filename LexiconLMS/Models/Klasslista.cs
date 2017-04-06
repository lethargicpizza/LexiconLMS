using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class Klasslista
    {
        //[Display(Name = "Förnamn", Order =1)]
        public string FörNamn { get; set; }

        //[Display(Name = "Efternamn", Order=0)]
        public string EfterNamn { get; set; }

        //[Display(Name = "Namn", Order = -9)]
        //public string FullNamn { get; set; }

        [Display(Name ="E-post")]
        public string Email { get; set; }
    }
}
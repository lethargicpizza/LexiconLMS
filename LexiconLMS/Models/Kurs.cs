using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LexiconLMS.Models;

namespace LexiconLMS.Models
{
    public class Kurs
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public DateTime StartDatum { get; set; }

        public virtual ICollection<ApplicationUser> KursMedlemmar { get; set; }

        public virtual ICollection<Modul> Moduler { get; set; } 
    }
}
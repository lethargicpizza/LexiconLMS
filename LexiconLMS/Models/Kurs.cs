using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LexiconLMS.Models;
using System.ComponentModel.DataAnnotations;

namespace LexiconLMS.Models
{
    public class Kurs
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }

        [Display(Name = "Start")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDatum { get; set; }

        // Navigation properties
        public virtual ICollection<ApplicationUser> KursMedlemmar { get; set; }
        public virtual ICollection<Modul> Moduler { get; set; } 
        
        //public virtual Modul KursModuler { get; set; }
    }
}
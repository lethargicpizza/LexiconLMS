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

        [Display (Name = "Kursnamn")]
        [Required(ErrorMessage ="Ett namn krävs")]
        public string Namn { get; set; }

        public string Beskrivning { get; set; }

        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage ="Ett datum krävs")]
        public DateTime StartDatum { get; set; }

        // Navigation properties
        public virtual ICollection<ApplicationUser> KursMedlemmar { get; set; }
        public virtual ICollection<Modul> Moduler { get; set; } 
        
        //public virtual Modul KursModuler { get; set; }
    }
}
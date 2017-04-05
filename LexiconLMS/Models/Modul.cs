using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class Modul
    {
        public int Id { get; set; }
        [Display(Name = "Modul")]
        public string Namn { get; set; }
        public string Beskrivning { get; set; }

        [Display (Name ="Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDatum { get; set; }

        [Display(Name = "Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime SlutDatum { get; set; }

        [Display(Name = "Kursnamn")]
        public int? KursId { get; set; }
        public int? DokumentId { get; set; }

        public virtual Kurs Kurs { get; set; }
        public virtual Dokument Moduldokument { get; set; }
        public virtual ICollection<Aktivitet> Aktiviteter { get; set; }
    }
}
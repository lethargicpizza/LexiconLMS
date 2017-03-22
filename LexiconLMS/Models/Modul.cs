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
        public string Namn { get; set; }
        public string Beskrivning { get; set; }

        [Display (Name ="Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDatum { get; set; }

        [Display(Name = "Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SlutDatum { get; set; }

        public int? KursId { get; set; }
        public virtual Kurs Kurs { get; set; }
    }
}
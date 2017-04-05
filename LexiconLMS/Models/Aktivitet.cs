﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexiconLMS.Models
{
    [Authorize]
    public class Aktivitet
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Ett namn krävs")]
        public string Namn { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name="Start")]
        [Required(ErrorMessage = "Ett tidpunkt krävs")]
        public DateTime StartTid { get; set; }

        [Display(Name = "Slut")]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Ett tidpunkt krävs")]
        public TimeSpan SlutTid { get; set; }
        //public int AntalTimmar { get; set; } Kan ej sätta SlutTid på 12:00 med denna variant!

        public int? AktivitetsTypId { get; set; }
        public int? DokumentsId { get; set; }
        public int? ModulId { get; set; }

        [Display(Name ="Aktivitetstyp")]
        public virtual AktivitetsTyp AktivitetsTyp { get; set; }
        public virtual Dokument Aktivitetdokument { get; set; }
        public virtual Modul Modul { get; set; }
    }
}
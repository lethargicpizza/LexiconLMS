﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LexiconLMS.Models
{
    [Authorize]
    public class Aktivitet
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public DateTime StartTid { get; set; }
        public TimeSpan SlutTid { get; set; }
        //public int AntalTimmar { get; set; } Kan ej sätta SlutTid på 12:00 med denna variant!

        public int? AktivitetsTypId { get; set; }
        public virtual AktivitetsTyp AktivitetsTyp { get; set; }

        public virtual Modul Modul { get; set; }
    }
}
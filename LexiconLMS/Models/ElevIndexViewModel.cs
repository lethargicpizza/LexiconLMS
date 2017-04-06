using LexiconLMS.Src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class ElevIndexViewModel
    {
        public string Användarnamn { get; set; }
        public Kurs PågåendeKurs { get; set; }
        public Modul PågåendeModul { get; set; }
        public IEnumerable<Dag> KommandeDagar { get; set; }
    }
}
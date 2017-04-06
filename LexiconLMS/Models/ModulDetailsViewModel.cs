using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
 
    public class ModulDetailsViewModel
    {
        public ModulDetailsViewModel()
        {
            datum = new List<string>();
            startTid = new List<string>();
            slutTid = new List<string>();
        }

        public Modul modul { get; set; }
        public Kurs kurs { get; set; }
        public List<Aktivitet> aktiviteter { get; set; }
        public List<string> datum { get; set; }
        public List<string> startTid { get; set; }
        public List<string> slutTid { get; set; }
    }
}
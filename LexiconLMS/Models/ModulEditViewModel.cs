using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class ModulEditViewModel
    {
        public Modul modul { get; set; }
        public Kurs kurs { get; set; }
        public List<Aktivitet> aktiviteter { get; set; }
    }
}
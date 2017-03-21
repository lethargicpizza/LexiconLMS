using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class Modul
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }

        public int? KursId { get; set; }
        public virtual Kurs Kurs { get; set; }
    }
}
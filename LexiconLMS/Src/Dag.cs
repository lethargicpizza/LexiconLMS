using LexiconLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMS.Src
{
    public class Dag
    {
        public DateTime Datum { get; set; }
        public List<Aktivitet> Aktiviteter { get; set; }

        public Dag()
        {
            Datum = new DateTime();
            Aktiviteter = new List<Aktivitet>();
        }
    }
}
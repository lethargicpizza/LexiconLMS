using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class LäggTillElevViewModel
    {
        public int? KursId { get; set; }
        public string Kursnamn { get; set; }
        public string AnvändarId { get; set; }
        public IEnumerable<ApplicationUser> Användare { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class Dokument
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Namn  { get; set; }

        public string Beskrivning { get; set; }

        public DateTime? Tidsstämpel { get; set; }

        [Required]
        [Display(Name ="Publicerat")]
        public DateTime Publiceringsdatum { get; set; }

        [Required]
        public byte[] Fil { get; set; }

        public int? AnvändarId{ get; set; }
        public int? DokumentTypId { get; set; }

        public virtual ApplicationUser Användare { get; set; }

        [Display(Name = "Dokument typ")]
        public virtual DokumentTyp DokumentTyp { get; set; }
    }
}
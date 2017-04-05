using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LexiconLMS.Models
{
    public class DokumentTyp
    {
            [Required]
            public int Id { get; set; }
            [Required]
            public string Typ { get; set; }
    }
}
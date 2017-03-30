using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LexiconLMS.DataAnnotations
{
    public class PasswordAttribute : ValidationAttribute
    {
        Regex ingenSpace;
        Regex minstEnSiffra;
        Regex minstEnBokstav;

        public PasswordAttribute()
        {
            ingenSpace = new Regex(@"[\s]");
            minstEnSiffra = new Regex(@"[\d]");
            minstEnBokstav = new Regex(@"[a-zA-Z]");
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool harMisslyckats = false;
            string resultatMeddelande = "";

            string password = (string)value;
            if(!String.IsNullOrWhiteSpace(password))
            {
                if (ingenSpace.IsMatch(password))
                {
                    resultatMeddelande = "Inga mellanslag i lösenordet. ";
                    harMisslyckats = true;
                }

                if (!minstEnSiffra.IsMatch(password))
                {
                    resultatMeddelande += "Ett lösenord behöver minst en siffra. ";
                    harMisslyckats = true;
                }

                if (!minstEnBokstav.IsMatch(password))
                {
                    resultatMeddelande += "Ett lösenord behöver minst en bokstav.";
                    harMisslyckats = true;
                }

                if (harMisslyckats)
                {
                    return new ValidationResult(resultatMeddelande);
                }
            }

            return ValidationResult.Success;
        }
    }
}
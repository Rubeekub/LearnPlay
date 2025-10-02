using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Models
{
    public class Utilisateurs
    {
        public int IdUti { get; set; }
        public string? NomUti { get; set; }
        public string? PrenomUti { get; set; }
        public string MailUti { get; set; } = default!;
        public byte[] MdpUti { get; set; } = Array.Empty<byte>(); // 64 octets
        public DateOnly DateInscription { get; set; }             // SQL DATE
    }
}

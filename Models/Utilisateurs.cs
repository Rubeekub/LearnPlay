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
        [Key]
        public int IdUti { get; set; }
        public string NomUti { get; set; }
        public string PrenomUti { get; set; }
        public string MailUti { get; set; }
        public byte[] MdpUti { get; set; }        // hash (déjà calculé côté service)
        public DateTime DateInscription { get; set; }
        public int? IdProfActif { get; set; }
    }
}

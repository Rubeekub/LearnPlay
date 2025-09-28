using System.ComponentModel.DataAnnotations;

namespace LearnPlay.Models
{
    public class ProfAppRecomp
    {
        [Key]
        public int IdProfil { get; set; }

        [Key]
        public int IdRecompense { get; set; }
        [Key]
        public int IdApplication { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Duree { get; set; }

        [Required]
        public bool Resultat { get; set; } = false;

        [Required]
        public int Points { get; set; } = 0;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateObtention { get; set; }

        public Profils Profil { get; set; }
        public Recompenses Recompense { get; set; } 
        public Applications Application { get; set; } 
    }
}

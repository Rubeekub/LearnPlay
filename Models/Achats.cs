using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnPlay.Models
{
    public class Achats
    {
        [Key]
        public int IdAchat { get; set; }

        [Required]
        [StringLength(50)]
        public string NomAchat { get; set; }

        [Required]
        public int ValeurAchat { get; set; }

        [Required]
        [StringLength(100)]
        public string DescripAchat { get; set; }

        [Required]
        public DateTime? DateAchat { get; set; }

        [Required]
        public int IdProfAchat { get; set; }

        [Required]
        public Profils Profils { get; set; }
    }
}

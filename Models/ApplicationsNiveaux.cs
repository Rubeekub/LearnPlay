using System.ComponentModel.DataAnnotations;

namespace LearnPlay.Models
{
    public class ApplicationsNiveaux
    {
        [Key]
        public int IdAppNiv { get; set; }

        [Key]
        public int IdNivApp { get; set; }

        public Applications Applications { get; set; }
        public Niveaux? Niveaux { get; set; }
    }
}

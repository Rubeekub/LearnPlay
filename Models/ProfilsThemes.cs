using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnPlay.Models
{
    public class ProfilsThemes
    {
        [Key]
        public int IdThemeProf { get; set; }

        [Key]
        public int IdProfTheme { get; set; }

        public bool IsActif { get; set; }

        public Profils? Profils { get; set; }
        public Themes Themes { get; set; } = new Themes();
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnPlay.Models
{
    public class ThemesAchats
    {
        [Key]
        public int IdThemeAchat { get; set; }

        [Key]
        public int IdAchatTheme { get; set; }

        public Themes? Themes { get; set; }
        public Achats? Achats { get; set; }
    }
}

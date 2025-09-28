using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnPlay.Models
{
    public class ImagesAchats
    {
        [Key]
        public int IdThemeAchat { get; set; }

        [Key]
        public int IdAchatTheme { get; set; }

        public Themes? Themes { get; set; }
        public Achats? Achats { get; set; }
    }
}

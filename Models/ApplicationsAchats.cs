using System.ComponentModel.DataAnnotations;

namespace LearnPlay.Models
{
    public class ApplicationsAchats
    {
        [Key]
        public int IdAppAchat { get; set; }

        [Key]
        public int IdAchatApp { get; set; }

        public Applications? Applications { get; set; }
        public Achats? Achats { get; set; }
    }
}

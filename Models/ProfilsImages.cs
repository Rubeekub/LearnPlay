using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnPlay.Models
{
    public class ProfilsImages
    {
        [Key]
        public int IdImgProf { get; set; }

        [Key]
        public int IdProfImg { get; set; }

        public bool IsActif { get; set; }

        public Images? Images { get; set; }

        public Profils Profils { get; set; } =new Profils();
    }
}

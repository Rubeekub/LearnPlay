using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnPlay.Models
{
    public class Profils
    {
        [Key] public int idProf { get; set; }

        [Required]
        public int numProf { get; set; }

        public string pseudoProf { get; set; }

        public int pointsProf { get; set; }

        public int nivProf { get; set; }

        public DateOnly dateNaissanceProf { get; set; }

        public int idUtiProf {  get; set; }

        public int idRoleProf {  get; set; }

  

        public void MettreAJourPoints(int nouveauxPoints)
        {
            this.pointsProf = nouveauxPoints;
        }

        public void choixNiveau(int nouveauNiveau)
        {
            this.nivProf = nouveauNiveau;
        }
    
    public Profils() 
        {
            this.idProf = 0;
            this.numProf = 0;
            this.pseudoProf = string.Empty;
            this.pointsProf = 0;
            this.nivProf = 0;
            this.dateNaissanceProf = DateOnly.MinValue;
        }

        /// <summary>
        /// constructeur du profil
        /// </summary>
        /// <param name="unIdProf">identifiant du profil</param>
        /// <param name="unNumProf">numero du profil</param>
        /// <param name="unPseudoProf">pseudo du profil</param>
        /// <param name="unPointsProf">point du profil</param>
        /// <param name="unNivProf">niveau du profil</param>
        /// <param name="uneDateNaissanceProf">dat de naissance du profil</param>
        public Profils(int unIdProf, int unNumProf, string unPseudoProf, int unPointsProf, int unNivProf, DateOnly uneDateNaissanceProf, int UtilisateurProf, int RoleProf)
        {
            this.idProf = unIdProf;
            this.numProf = unNumProf;
            this.pseudoProf = unPseudoProf;
            this.pointsProf = unPointsProf;
            this.nivProf = unNivProf;
            this.dateNaissanceProf = uneDateNaissanceProf;
            this.idUtiProf = UtilisateurProf;
            this.idRoleProf = RoleProf;
         }
    }
}

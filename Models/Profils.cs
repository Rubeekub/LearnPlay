using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exoModelsProjet.Models
{
    public class Profils
    {
        /// <summary>
        /// identifiant du profil
        /// </summary>
        [Key] public int idProf { get; set; }

        /// <summary>
        /// Nom du profil
        /// </summary>
        public int numProf { get; set; }

        /// <summary>
        /// pseudo du profil
        /// </summary>
        public string pseudoProf { get; set; }

        /// <summary>
        /// point du profil
        /// </summary>
        public int pointsProf { get; set; }

        /// <summary>
        /// niveau du profil
        /// </summary>
        public int nivProf { get; set; }

        /// <summary>
        /// date de naissance du profil
        /// </summary>
        public DateOnly dateNaissanceProf { get; set; }

        /// <summary>
        /// utilisateur lié au profil
        /// </summary>
        public Utilisateurs utilisateurProf {  get; set; }

        /// <summary>
        /// liste des  roles disponible
        /// </summary>
        public List<Roles> roleProf {  get; set; }

        /// <summary>
        /// Verification des champs obligatoire du profil
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void champsProfilRequis() 
        {
            if (this.pseudoProf.Length ==0)
            {
                throw new Exception("Le pseudo ne peut pas être vide ou null.");
            }
        }

        /// <summary>
        /// Mise a jour des points du profil
        /// </summary>
        /// <param name="nouveauxPoints"></param>
        public void MettreAJourPoints(int nouveauxPoints)
        {
            this.pointsProf = nouveauxPoints;
        }

        /// <summary>
        /// mise a jour du niveau du profil
        /// </summary>
        /// <param name="nouveauNiveau"></param>
        public void choixNiveau(int nouveauNiveau)
        {
            this.nivProf = nouveauNiveau;
        }
    
        /// <summary>
        /// contructeur par défaut
        /// </summary>
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
        public Profils(int unIdProf, int unNumProf, string unPseudoProf, int unPointsProf, int unNivProf, DateOnly uneDateNaissanceProf, Utilisateurs unUtilisateurProf, List<Roles> unRoleProf)
        {
            this.idProf = unIdProf;
            this.numProf = unNumProf;
            this.pseudoProf = unPseudoProf;
            this.pointsProf = unPointsProf;
            this.nivProf = unNivProf;
            this.dateNaissanceProf = uneDateNaissanceProf;
            this.utilisateurProf = unUtilisateurProf;
            this.roleProf = unRoleProf;

            champsProfilRequis();
         }


    }
}

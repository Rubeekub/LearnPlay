using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exoModelsProjet.Models
{
    internal class Niveaux
    {
        /// <summary>
        /// identifiant de la classe Niveaux
        /// </summary>
        [Key] public int idNiv { get; set; }

        /// <summary>
        /// nom de la classe Niveaux
        /// </summary>
        public string nomNiv { get; set; }

        /// <summary>
        /// description de la classe Niveaux
        /// </summary>
        public string descripNiv { get; set; }

        /// <summary>
        /// méthode pour modifier le nom
        /// </summary>
        /// <param name="unNom"></param>
        public void modifierNom(string unNom)
        {
            this.nomNiv = unNom;
        }

        /// <summary>
        /// méthode pour modifier la description
        /// </summary>
        /// <param name="uneDescrip"></param>
        public void modifierDescrip(string uneDescrip)
        {
            this.descripNiv = uneDescrip;
        }

        /// <summary>
        /// controle des champs requis de la classe classe Niveau
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void controleChamp()
        {
            if (this.nomNiv.Length == 0 || this.descripNiv.Length == 0)
            {
                throw new Exception("les champs nomNiv et descripNiv ne peuvent pas être vides ou null");
            }
        }
        /// <summary>
        /// constructeur par défaut de la classe Niveaux
        /// </summary>
        public Niveaux()
{
            this.idNiv = 0;
            this.nomNiv = string.Empty;
            this.descripNiv = string.Empty;
        }

        /// <summary>
        /// constructeur de la classe Niveaux
        /// </summary>
        /// <param name="unIdNiv">identifiant de la classe</param>
        /// <param name="unNomNiv">nom de la classe</param>
        /// <param name="uneDescripNiv">description de la classe</param>
        public Niveaux(int unIdNiv, string unNomNiv, string uneDescripNiv)
    {
            this.idNiv = unIdNiv;
            this.nomNiv = unNomNiv;
            this.descripNiv = uneDescripNiv;
            controleChamp();
        }

        [Required]
        [StringLength(20)]
        public string NomNiv { get; set; } = "Débutant";

        [Required]
        [StringLength(100)]
        public string DescripNiv { get; set; } = string.Empty;
    }
}

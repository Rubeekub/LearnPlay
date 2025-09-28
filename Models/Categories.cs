using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Models
{
    internal class Categories
    {
        /// <summary>
        /// identifiant de la catégorie
        /// </summary>
        [Key] public int idCat{ get; set; }

        /// <summary>
        /// nom de la catégorie
        /// </summary>
        public string nomCat { get; set; }

        /// <summary>
        /// description de la catégorie
        /// </summary>
        public string descripCat { get; set; }

        /// <summary>
        /// méthode pour modifier le nom
        /// </summary>
        /// <param name="unNom"></param>
        public void modifierNom(string unNom)
        {
            this.nomCat = unNom;
        }

        /// <summary>
        /// méthode pour modifier la description
        /// </summary>
        /// <param name="uneDescrip"></param>
        public void modifierDescrip(string uneDescrip)
        {
            this.descripCat = uneDescrip;
        }

        /// <summary>
        /// controle des champs requis de la classe catégorie
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void controleChamp()
        {
            if(this.nomCat.Length ==0 || this.descripCat.Length == 0)
            {
                throw new Exception("les champs nomCat et descripCat ne peuvent pas être vides ou null");
            }
        }
        /// <summary>
        /// constructeur par défaut de la classe Catégories
        /// </summary>
        public Categories()
        {
            this.idCat = 0;
            this.nomCat = string.Empty;
            this.descripCat= string.Empty;
        }

        /// <summary>
        /// constructeur de la classe Categories
        /// </summary>
        /// <param name="unIdCat">identifiant de la classe</param>
        /// <param name="unNomCat">nom de la classe</param>
        /// <param name="uneDescripCat">description de la classe</param>
        public Categories(int unIdCat, string unNomCat, string uneDescripCat)
    {
            this.idCat = unIdCat;
            this.nomCat = unNomCat;
            this.descripCat = uneDescripCat;
            controleChamp();
        }

        [Required]
        [StringLength(20)]
        public string NomCat { get; set; }

        [Required]
        [StringLength(150)]
        public string DescripCat { get; set; }
    }
}

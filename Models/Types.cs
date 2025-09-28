using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exoModelsProjet.Models
{
    internal class Types
    {
        /// <summary>
        /// identifiant de la classe type
        /// </summary>
        [Key] public int idType { get; set; }

        /// <summary>
        /// nom de la classe type
        /// </summary>
        public string nomType { get; set; }

        /// <summary>
        /// description de la classe type
        /// </summary>
        public string descripType { get; set; }

        /// <summary>
        /// méthode pour modifier le nom
        /// </summary>
        /// <param name="unNom"></param>
        public void modifierNom(string unNom)
        {
            this.nomType = unNom;
        }

        /// <summary>
        /// méthode pour modifier la description
        /// </summary>
        /// <param name="uneDescrip"></param>
        public void modifierDescrip(string uneDescrip)
        {
            this.descripType = uneDescrip;
        }

        /// <summary>
        /// controle des champs requis de la classe classe type
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void controleChamp()
        {
            if (this.nomType.Length == 0 || this.descripType.Length == 0)
            {
                throw new Exception("les champs nomType et descripType ne peuvent pas être vides ou null");
            }
        }
        /// <summary>
        /// constructeur par défaut de la classe Types
        /// </summary>
        public Types()
{
            this.idType = 0;
            this.nomType = string.Empty;
            this.descripType = string.Empty;
        }

        /// <summary>
        /// constructeur de la classe Types
        /// </summary>
        /// <param name="unIdType">identifiant de la classe</param>
        /// <param name="unNomType">nom de la classe</param>
        /// <param name="uneDescripType">description de la classe</param>
        public Types(int unIdType, string unNomType, string uneDescripType)
    {
            this.idType = unIdType;
            this.nomType = unNomType;
            this.descripType = uneDescripType;
            controleChamp();
        }

        [Required]
        [StringLength(20)]
        public string NomType { get; set; } = "Cours";

        [Required]
        [StringLength(150)]
        public string DescripType { get; set; }="Une fonctionnalité de ton cerveau sera créée ou améliorée";
    }
}

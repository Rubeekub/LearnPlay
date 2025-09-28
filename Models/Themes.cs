using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exoModelsProjet.Models
{
    internal class Themes
    {
        /// <summary>
        /// identifiant du thème
        /// </summary>
        [Key] public int idTheme { get; set; }

        /// <summary>
        /// Nom du thème
        /// </summary>
        public string nomTheme { get; set; }

        /// <summary>
        /// couleur du thème
        /// </summary>
        public string couleurTheme { get; set; }

        /// <summary>
        /// police du thème
        /// </summary>
        public string policeTheme { get; set; }

        /// <summary>
        /// mettre a jour la couleur
        /// </summary>
        /// <param name="unePoliceTheme"></param>
        public void mettreAJourCouleur(string uneCouleurTheme)
        {
            this.couleurTheme = uneCouleurTheme;
        }

        /// <summary>
        /// mettre a jour la police
        /// </summary>
        /// <param name="unePoliceTheme"></param>
        public void mettreAJourPolice(string unePoliceTheme)
        {
            this.policeTheme = unePoliceTheme;
        }

        /// <summary>
        /// verification des champs requis
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void champNonNul()
        {
            if (this.nomTheme.Length == 0 || this.couleurTheme.Length == 0 || this.policeTheme.Length == 0)
            {
                throw new Exception("Les champs nom, couleur et police ne peuvent pas être vides ou null.");
            }
        }

        /// <summary>
        /// constructeur par défaut
        /// </summary>
        public Themes()
        {
            this.idTheme = 0;
            this.nomTheme = string.Empty;
            this.couleurTheme = string.Empty;
            this.policeTheme = string.Empty;
        }

        /// <summary>
        /// constructeur de la classe thème
        /// </summary>
        /// <param name="unIdTheme"></param>
        /// <param name="unNomTheme"></param>
        /// <param name="uneCouleurTheme"></param>
        /// <param name="unePoliceTheme"></param>
        public Themes(int unIdTheme, string unNomTheme, string uneCouleurTheme, string unePoliceTheme)
            { 
                this.idTheme = unIdTheme;
                this.nomTheme = unNomTheme;
                this.couleurTheme = uneCouleurTheme;
                this.policeTheme = unePoliceTheme;

            champNonNul();
            }
    }
}

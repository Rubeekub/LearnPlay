using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Models
{
    internal class Applications
    {
        /// <summary>
        /// identifiant de l'application
        /// </summary>
        [Key] public int idApp { get; set; }

        /// <summary>
        /// titre de l'application
        /// </summary>
        public string titreApp { get; set; }

        /// <summary>
        /// description de l'application
        /// </summary>
        public string descripApp  { get; set; }

        /// <summary>
        /// valeur de l'application
        /// </summary>
        public int valeurApp { get; set; }

        /// <summary>
        /// matière de l'application (français, math, geographie, histoire, ...)
        /// </summary>
        public string matiereApp  { get; set; }

        /// <summary>
        /// chemin de l'application
        /// </summary>
        public string cheminApp {  get; set; }

        /// <summary>
        /// modifier le titre de l'app
        /// </summary>
        /// <param name="unTitreApp"></param>
        public void modifierTitre(string unTitreApp)
        {
            this.titreApp = unTitreApp ;
        }

        /// <summary>
        /// modifier la description
        /// </summary>
        /// <param name="uneDecrip"></param>
        public void modifierDecrip(string uneDecrip)
        {
            this.descripApp = uneDecrip ;
        }

        /// <summary>
        /// modifier la valeur
        /// </summary>
        /// <param name="uneValeur"></param>
        public void modifierValeur(int uneValeur)
        {
            this.valeurApp = uneValeur ;
        }

        /// <summary>
        /// modifier la matiere
        /// </summary>
        /// <param name="uneMatiere"></param>
        public void modifierMatiere(string uneMatiere)
        {
            this .matiereApp = uneMatiere ;
        }

        [Required]
        [StringLength(250)]
        public string CheminApp { get; set; }

        /// <summary>
        /// modifier le chemin 
        /// </summary>
        /// <param name="unChemin"></param>
        public void modifierChemin(string unChemin)
        {
            this .cheminApp = unChemin ;
        }

        /// <summary>
        /// controle des champs requis
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void controleChamp()
        {
            if (this.titreApp.Length == 0 || this.cheminApp.Length == 0)
            {
                throw new Exception("Les champs  titreApp et cheminApp ne peuvent pas etre vides ");
            }
        }

        /// <summary>
        /// constructeur par défaut
        /// </summary>
        public Applications()
        {
            this.idApp = 0;
            this.titreApp = string.Empty;
            this.descripApp = string.Empty;
            this.valeurApp = 0;
            this.matiereApp = string.Empty;
            this.cheminApp = string.Empty;
        }

        /// <summary>
        /// constructeur de l'application
        /// </summary>
        /// <param name="unIdApp">l'identifiant </param>
        /// <param name="unTitreApp">le titre</param>
        /// <param name="uneDescripApp">la description</param>
        /// <param name="uneValeurApp">la valeur</param>
        /// <param name="uneMatiereApp">la matière</param>
        /// <param name="unCheminApp">le chemin</param>
        public Applications(int unIdApp, string unTitreApp, string uneDescripApp, int uneValeurApp, string uneMatiereApp, string unCheminApp)
        {
            this.idApp = unIdApp;
            this.titreApp = unTitreApp;
            this.descripApp = uneDescripApp;
            this.valeurApp = uneValeurApp;
            this.matiereApp = uneMatiereApp;
            this.cheminApp = unCheminApp;

            controleChamp();
        }
    }
}

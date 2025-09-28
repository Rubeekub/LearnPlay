using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Models
{
    internal class Boutique
    {
        /// <summary>
        /// Identifiant de l'achat
        /// </summary>
        [Key] public int IdAchat { get; set; } 
        
        /// <summary>
        /// nom del'achat
        /// </summary>
        public string nomAchat { get; set; }

        /// <summary>
        /// Valeur de l'achat
        /// </summary>
        public int valeurAchat { get; set; }

        /// <summary>
        /// Description de l'achat
        /// </summary>
        public string descripAchat {  get; set; }

        /// <summary>
        /// methode pour modifier le nom de l achat
        /// </summary>
        /// <param name="unNomAchat"></param>
        public void modifierNomAchat(string unNomAchat)
        {
            this.nomAchat = unNomAchat;
        }

        /// méthode pour modifier la valeur de l 'achat
        /// </summary>
        /// <param name="uneValeurAchat"></param>
        public void modifierValeurAchat(int uneValeurAchat)
        {
            this.valeurAchat = uneValeurAchat;
        }

        /// <summary>
        /// methode pour modifier la description de l'achat
        /// </summary>
        /// <param name="uneDescripAchat"></param>
        public void modifierDescripAchat(string uneDescripAchat)
        {
            this.descripAchat = uneDescripAchat;
        }

        /// <summary>
        /// Controle des champs requis
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void controlChamp()
        {
            if (this.nomAchat.Length == 0 || this.valeurAchat == 0 ||this.descripAchat.Length == 0) 
            {
                throw new Exception("Les champs nomAchat, valeurAchat et descripAchat ne peuvent pas etre vides ou null."); 
            }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Boutique() 
        {
            this.IdAchat = 0;
            this.nomAchat = string.Empty;
            this.valeurAchat = 0;
            this.descripAchat = string.Empty;
        }


        /// <summary>
        /// Constructeur de la classe Boutique
        /// </summary>
        /// <param name="unIdAchat"></param>
        /// <param name="unNomAchat"></param>
        /// <param name="uneValeurAchat"></param>
        /// <param name="uneDescripAchat"></param>
        public Boutique(int unIdAchat, string unNomAchat, int uneValeurAchat, string uneDescripAchat)
        {
            IdAchat = unIdAchat;
            this.nomAchat = unNomAchat;
            this.valeurAchat = uneValeurAchat;
            this.descripAchat = uneDescripAchat;

            controlChamp();
        }
    }
}

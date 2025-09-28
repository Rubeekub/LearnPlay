using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Models
{
    internal class Images
    {
        /// <summary>
        /// identifiant de l'image
        /// </summary>
        [Key] public int idImg { get; set; }

        /// <summary>
        /// nom de l'image
        /// </summary>
        public string nomImg { get; set; }

        /// <summary>
        /// chemin d'accès de l'image
        /// </summary>
        public string cheminImg { get; set; }

        /// <summary>
        /// classement de l immage selon sa nature avatar, recompense, fond d ecran...icones
        /// </summary>
        public string typeImg {  get; set; }

        /// <summary>
        /// Méthode pour mettre à jour le chemin d'accès de l'image
        /// </summary>
        /// <param name="nouveauChemin">Nouveau chemin d'accès</param>
        public void MettreAJourChemin(string nouveauChemin)
        {
            this.cheminImg = nouveauChemin;
        }

        /// <summary>
        /// Méthode pour mettre à jour le type de l'image
        /// </summary>
        /// <param name="nouveauType">Nouveau type d'image</param>
        public void MettreAJourType(string nouveauType)
        {
            this.typeImg = nouveauType;
        }

        /// <summary>
        /// verification des champs requis
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void champNonNul()
        {
            if (this.nomImg.Length==0 || this.cheminImg.Length==0 || this.typeImg.Length==0) 
            {
                throw new Exception("Les champs nom, chemin et type ne peuvent pas être vides ou null.");
            }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Images()
        {
            this.idImg = 0;
            this.nomImg = string.Empty;
            this.cheminImg = string.Empty;
            this.typeImg = string.Empty;
        }

        /// <summary>
        /// Constructeur de l'image
        /// </summary>
        /// <param name="unIdImg">Identifiant de l'image</param>
        /// <param name="unNomImg">Nom de l'image</param>
        /// <param name="unCheminImg">Chemin d'accès de l'image</param>
        /// <param name="unTypeImg">Type de l'image</param>
        public Images(int unIdImg, string unNomImg, string unCheminImg, string unTypeImg)
        {
            this.idImg = unIdImg;
            this.nomImg = unNomImg;
            this.cheminImg = unCheminImg;

            champNonNul();
        }
    }
}



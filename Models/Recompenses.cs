using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exoModelsProjet.Models
{
    internal class Recompenses
    {
        /// <summary>
        /// Identifiant de la recompense
        /// </summary>
        [Key] public int idRecomp {  get; set; }

        /// <summary>
        /// nom de la récompense
        /// </summary>
        public string nomRecomp { get; set; }

        /// <summary>
        /// Mettre a jour le nom de la récompense
        /// </summary>
        /// <param name="unNomRecomp"></param>
        public void mettreAJourRécomp(string unNomRecomp)
        {
            this.nomRecomp = unNomRecomp;
        }

        /// <summary>
        /// verification des champs requis
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void champNonNul()
        {
            if (this.nomRecomp.Length == 0 )
            {
                throw new Exception("Le champs nomRecomp peu^t pas être vide ou null.");
            }
        }
        /// <summary>
        /// constructeur par défaut
        /// </summary>
        public Recompenses() 
        {
            this.idRecomp = 0;
            this.nomRecomp = string.Empty;
        }

        /// <summary>
        /// Constructeur de la classe Récompense
        /// </summary>
        /// <param name="unIdRecomp">l'identifiant de la recompense</param>
        /// <param name="unNomRecomp">Le nom de la récompense</param>
        public Recompenses(int unIdRecomp, string unNomRecomp)
        {
            this.idRecomp = unIdRecomp;
            this.nomRecomp = unNomRecomp;

            champNonNul();
        }
    }
}

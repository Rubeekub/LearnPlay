using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exoModelsProjet.Models
{
    internal class Roles
    {
        /// <summary>
        /// identifiant du role de l utilisateur
        /// </summary>
        [Key] public int idRole{  get; set; }

        /// <summary>
        /// nom du role de l'utilisateur
        /// </summary>
        public string nomRole { get; set; }

       /// <summary>
       /// contructeur du role de l'utilisateur
       /// </summary>
       /// <param name="aIdRole"></param>
       /// <param name="aNomRole"></param>
        public Roles(int aIdRole, string aNomRole) 
        {
            this.idRole = aIdRole;
            this.nomRole = aNomRole;
        }

        /// <summary>
        /// delimite le nombre de profil en fonction du role
        /// </summary>
        /// <returns>renvoie la limite de profil du role</returns>
        /// <exception cref="Exception"></exception>
        public int LimiteRole()                     // peut on faire un retour d exception dans une methode qui renvoie des int
        {
            if (this.idRole == 1)
            {
                return 1;
            } else
            {
                if (this.idRole == 5)
                {
                    return 5;
                }
                else
                {
                    if (this.idRole == 999)
{
                        return 999;
                    } else
    {
                        throw new Exception("Rôle non reconn");

                    }
                }
            }
        }
    }
}

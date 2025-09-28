using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using exoModelsProjet.Models;
using Microsoft.Data.SqlClient;

namespace exoModelsProjet.repository
{
    internal class RepRoles
    {
        Connexion maConnexion = new Connexion();  

    

    public List<Roles> GetRoles()
        {
            List<Roles> listRoles = new List<Roles>();
            SqlCommand RequestGetRoles = activeConnexion.CreateCommande();

            RequestGetRoles.CommandText = "Select * from Roles";
            while (listRoles.Read())
            {
                Roles.unRole = new Roles();
                unRole.idFunction = Convert.ToInt32$"{roles[2]}";
                unRole.Person_id = Convert.ToInt32$"{roles[3]}";
                listRoles = Add(unRole);
            }
            return listRoles;
        }
}

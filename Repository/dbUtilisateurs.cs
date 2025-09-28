using System.Data;
using LearnPlay.Models;
using Microsoft.Data.SqlClient;

namespace LearnPlay.Repository
{
    internal class dbUtilisateurs
    {
        private SqlConnection? activeConnexion;

        public dbUtilisateurs()
        {
            this.dbConnecter();
        }

        private void dbConnecter()
        {
            Connexion maConnexion = new Connexion();
            this.activeConnexion = maConnexion.GetConnexion();
        }

        public bool addUtilisateur( Utilisateurs newUtilisateur)
        {
            if (this.activeConnexion == null || this.activeConnexion.State == ConnectionState.Closed)
            {
                this.dbConnecter();
            }
            SqlCommand requestAddUtilisateur = activeConnexion.CreateCommand();
            
            requestAddUtilisateur.CommandText = "INSERT INTO utilisateurs (nomUti, prenomUti, mailUti, mdpUti, dateInscription) VALUES (@nom, @prenom,@mail, @mdp, @dateInscription)";

            requestAddUtilisateur.Parameters.AddWithValue("@nom", newUtilisateur.NomUti);
            requestAddUtilisateur.Parameters.AddWithValue("@prenom",newUtilisateur.PrenomUti);
            requestAddUtilisateur.Parameters.AddWithValue("@mail", newUtilisateur.MailUti);
            requestAddUtilisateur.Parameters.AddWithValue("@mdp",newUtilisateur.MdpUti);
            requestAddUtilisateur.Parameters.AddWithValue("@dateInscription",newUtilisateur.MailUti);

            int resultat = requestAddUtilisateur.ExecuteNonQuery();

            //Fermeture de la connexion
            this.activeConnexion.Close();
            Console.WriteLine("il  y a  " + resultat + "ligne(s) modifiée(s)");

            return resultat > 0;
        }

        public Utilisateurs GetUtilisateurById(int id)
        {
            if (this.activeConnexion == null || this.activeConnexion.State == ConnectionState.Closed)
            {
                this.dbConnecter();
            }
            SqlCommand requestGetUtilisateurById = activeConnexion.CreateCommand();

            requestGetUtilisateurById.CommandText = "Select idUti,nomUti, prenomUti, mailUti, mdpUti, dateInscription from utlisateurs where idUti=@id";
            requestGetUtilisateurById.Parameters.AddWithValue("@id", id);


            SqlDataReader reader = requestGetUtilisateurById.ExecuteReader();
            Utilisateurs unUtilisateur = null;

            if (reader.Read())
            {
                unUtilisateur = new Utilisateurs
                {
                    IdUti = (int)reader["idUti"],
                    NomUti = reader["nomUti"].ToString(),
                    PrenomUti = reader["prenomUti"].ToString(),
                    MailUti = reader["mailUti"].ToString(),
                    MdpUti = reader["mdpUti"].ToString(),
                    DateInscription = DateTime.Parse(reader["dateInscription"].ToString())
                };
            }

            //Fermeture de la connexion
            this.activeConnexion.Close();
            Console.Write(unUtilisateur);

            return unUtilisateur;
        }

        public bool UpdateUtilisateur(Utilisateurs newUtilisateur)
        {
            if (this.activeConnexion == null || this.activeConnexion.State == ConnectionState.Closed)
            {
                this.dbConnecter();
            }

            SqlCommand requestUpdateUtilisateur = activeConnexion.CreateCommand();
            requestUpdateUtilisateur.CommandText = "UPDATE utilisateurs SET nomUti =@nom, prenomUti=@prenom, mailUti=@mail, mdpUti=@mdp, dateInscription=@date where person_id = @id";

            requestUpdateUtilisateur.Parameters.AddWithValue("@nom", newUtilisateur.NomUti);
            requestUpdateUtilisateur.Parameters.AddWithValue("@prenom",newUtilisateur.PrenomUti);
            requestUpdateUtilisateur.Parameters.AddWithValue("@mail", newUtilisateur.MailUti);
            requestUpdateUtilisateur.Parameters.AddWithValue("@mdp",newUtilisateur.MdpUti);
            requestUpdateUtilisateur.Parameters.AddWithValue("@dateInscription",newUtilisateur.MailUti);

            int result = requestUpdateUtilisateur.ExecuteNonQuery();

            //Fermeture de la connexion
            this.activeConnexion.Close();
            Console.WriteLine("il  y a  " + result + "ligne(s) modifiée(s)");

            return result > 0;
        }

        public bool DeleteUtilisateur(int id)
        {
            if (this.activeConnexion == null || this.activeConnexion.State == ConnectionState.Closed)
            {
                this.dbConnecter();
            }

            SqlCommand requestDeleteUtilisateur = activeConnexion.CreateCommand();

            requestDeleteUtilisateur.CommandText = "DELETE from utilisateurs  where idUti=@id";
            requestDeleteUtilisateur.Parameters.AddWithValue("@id", id);

            
            int resultat = requestDeleteUtilisateur.ExecuteNonQuery();            

            //Fermeture de la connexion
            this.activeConnexion.Close();
            Console.WriteLine("Suppression effectuée");

            return resultat > 0;
        }
    }
}


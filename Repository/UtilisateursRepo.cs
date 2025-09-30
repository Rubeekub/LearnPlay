using LearnPlay.Interfaces;
using LearnPlay.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LearnPlay.Repository
{
    public class UtilisateursRepo : IUtilisateursRepository
    {
        private SqlConnection activeConnexion;

        public UtilisateursRepo()
        {
            dbConnecter();
        }

        private void dbConnecter()
        {
            Connexion con = new Connexion();
            activeConnexion = con.GetConnection();
            if (activeConnexion.State != ConnectionState.Open)
            {
                activeConnexion.Open();
            }
        }

        private void VerifConnexion()
        {
            if (activeConnexion == null)
            {
                dbConnecter();
                return;
            }

            if (activeConnexion.State != ConnectionState.Open)
            {
                dbConnecter();
            }
        }

        // ================== Helpers ==================

        private static Utilisateurs MapReader(SqlDataReader rd)
        {
            Utilisateurs u = new Utilisateurs();
            int iId = rd.GetOrdinal("idUti");
            int iNom = rd.GetOrdinal("nomUti");
            int iPrenom = rd.GetOrdinal("prenomUti");
            int iMail = rd.GetOrdinal("mailUti");
            int iMdp = rd.GetOrdinal("mdpUti");
            int iDate = rd.GetOrdinal("dateInscription");

            u.IdUti = rd.GetInt32(iId);
            if (rd.IsDBNull(iNom)) u.NomUti = string.Empty; else u.NomUti = rd.GetString(iNom);
            if (rd.IsDBNull(iPrenom)) u.PrenomUti = string.Empty; else u.PrenomUti = rd.GetString(iPrenom);
            u.MailUti = rd.GetString(iMail);
            u.MdpUti = (byte[])rd.GetValue(iMdp);   // VARBINARY(64)
            DateTime d = rd.GetDateTime(iDate);
            u.DateInscription = d;

            // Le modèle a IdProfActif, mais la table ne l'a pas dans LearnPlayDb.sql
            // u.IdProfActif = null;

            return u;
        }

        // ================== Lectures ==================

        public Utilisateurs? GetById(int idUti)
        {
            VerifConnexion();

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription
FROM utilisateurs
WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;

            using SqlDataReader rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                return null;
            }

            Utilisateurs u = MapReader(rd);
            return u;
        }

        public Utilisateurs? GetByEmail(string mailUti)
        {
            VerifConnexion();

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription
FROM utilisateurs
WHERE mailUti = @m;";
            cmd.Parameters.Add("@m", SqlDbType.VarChar, 255).Value = mailUti;

            using SqlDataReader rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                return null;
            }

            Utilisateurs u = MapReader(rd);
            return u;
        }

        public List<Utilisateurs> GetAll()
        {
            VerifConnexion();

            List<Utilisateurs> list = new List<Utilisateurs>();

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription
FROM utilisateurs
ORDER BY idUti;";

            using SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Utilisateurs u = MapReader(rd);
                list.Add(u);
            }

            return list;
        }

        //  Création 

        public Utilisateurs Create(Utilisateurs u)
        {
            VerifConnexion();

            // Unicité mail (la base a déjà UNIQUE, on évite l'exception)
            using (SqlCommand c0 = activeConnexion.CreateCommand())
            {
                c0.CommandText = "SELECT COUNT(*) FROM utilisateurs WHERE mailUti = @m;";
                c0.Parameters.Add("@m", SqlDbType.VarChar, 255).Value = u.MailUti ?? string.Empty;
                object countObj = c0.ExecuteScalar();
                int exists = Convert.ToInt32(countObj);
                if (exists > 0)
                {
                    throw new InvalidOperationException("Email déjà utilisé.");
                }
            }

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
INSERT INTO utilisateurs (nomUti, prenomUti, mailUti, mdpUti, dateInscription)
VALUES (@n, @p, @m, @h, @d);
SELECT CAST(SCOPE_IDENTITY() AS INT);";     // récupérer l’ID auto-généré
            //ExecuteScalar() te renvoie l’ID.

            if (string.IsNullOrWhiteSpace(u.NomUti))
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 50).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 50).Value = u.NomUti;
            }

            if (string.IsNullOrWhiteSpace(u.PrenomUti))
            {
                cmd.Parameters.Add("@p", SqlDbType.VarChar, 50).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@p", SqlDbType.VarChar, 50).Value = u.PrenomUti;
            }

            cmd.Parameters.Add("@m", SqlDbType.VarChar, 255).Value = u.MailUti ?? string.Empty;

            if (u.MdpUti == null)
            {
                throw new ArgumentException("Le hash de mot de passe est requis (MdpUti).");
            }
            cmd.Parameters.Add("@h", SqlDbType.VarBinary, 64).Value = u.MdpUti;

            DateTime date = u.DateInscription;
            if (date == default)
            {
                date = DateTime.UtcNow.Date;
            }
            cmd.Parameters.Add("@d", SqlDbType.Date).Value = date;

            object newIdObj = cmd.ExecuteScalar();
            int newId = Convert.ToInt32(newIdObj);

            Utilisateurs created = GetById(newId);
            return created;
        }

        //  Mises à jour 

        public bool UpdateIdentite(int idUti, string? nom, string? prenom)
        {
            VerifConnexion();

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
UPDATE utilisateurs
SET
  nomUti = COALESCE(@n, nomUti),
  prenomUti = COALESCE(@p, prenomUti)
WHERE idUti = @id;";

            if (string.IsNullOrWhiteSpace(nom))
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 50).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@n", SqlDbType.VarChar, 50).Value = nom.Trim();
            }

            if (string.IsNullOrWhiteSpace(prenom))
            {
                cmd.Parameters.Add("@p", SqlDbType.VarChar, 50).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@p", SqlDbType.VarChar, 50).Value = prenom.Trim();
            }

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;

            int rows = cmd.ExecuteNonQuery();
            if (rows == 1)
            {
                return true;
            }

            return false;
        }

        public bool UpdateEmail(int idUti, string newEmail)
        {
            VerifConnexion();

            // Vérif unicité
            using (SqlCommand c0 = activeConnexion.CreateCommand())
            {
                c0.CommandText = "SELECT COUNT(*) FROM utilisateurs WHERE mailUti = @m AND idUti <> @id;";
                c0.Parameters.Add("@m", SqlDbType.VarChar, 255).Value = newEmail ?? string.Empty;
                c0.Parameters.Add("@id", SqlDbType.Int).Value = idUti;
                object countObj = c0.ExecuteScalar();
                int exists = Convert.ToInt32(countObj);
                if (exists > 0)
                {
                    return false;
                }
            }

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE utilisateurs SET mailUti = @m WHERE idUti = @id;";
            cmd.Parameters.Add("@m", SqlDbType.VarChar, 255).Value = newEmail ?? string.Empty;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;

            int rows = cmd.ExecuteNonQuery();
            if (rows == 1)
            {
                return true;
            }

            return false;
        }

        public bool UpdatePasswordHash(int idUti, byte[] newHash)
        {
            VerifConnexion();

            if (newHash == null)
            {
                return false;
            }

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE utilisateurs SET mdpUti = @h WHERE idUti = @id;";
            cmd.Parameters.Add("@h", SqlDbType.VarBinary, 64).Value = newHash;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;

            int rows = cmd.ExecuteNonQuery();
            if (rows == 1)
            {
                return true;
            }

            return false;
        }

        //  Suppression 

        public bool Delete(int idUti)
        {
            VerifConnexion();

            using SqlCommand cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "DELETE FROM utilisateurs WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;

            int rows = cmd.ExecuteNonQuery();
            if (rows == 1)
            {
                return true;
            }

            return false;
        }
    }
}

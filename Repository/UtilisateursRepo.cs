// UtilisateursRepo.cs
using LearnPlay;
using LearnPlay.Interfaces;
using LearnPlay.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LearnPlay.Repository
{
    public class UtilisateursRepo : IUtilisateursRepository, IDisposable
    {
        private SqlConnection activeConnexion;

        // --- Construction / Connexion ---
        public UtilisateursRepo()
        {
            dbConnecter();
        }

        private void dbConnecter()
        {
            var con = new Connexion();
            activeConnexion = con.GetConnection();
            if (activeConnexion.State != ConnectionState.Open)
            {
                activeConnexion.Open();
            }
        }

        private void VerifConnexion()
        {
            if (activeConnexion == null || activeConnexion.State != ConnectionState.Open)
            {
                dbConnecter();
            }
        }

        // --- Helpers ---
        private static DateTime ToDbDate(DateOnly d) => d.ToDateTime(TimeOnly.MinValue);

        private static Utilisateurs MapReader(SqlDataReader rd)
        {
            int cId = rd.GetOrdinal("idUti");
            int cNom = rd.GetOrdinal("nomUti");
            int cPre = rd.GetOrdinal("prenomUti");
            int cMail = rd.GetOrdinal("mailUti");
            int cMdp = rd.GetOrdinal("mdpUti");
            int cDate = rd.GetOrdinal("dateInscription");

            var u = new Utilisateurs();
            u.IdUti = rd.GetInt32(cId);
            u.NomUti = rd.IsDBNull(cNom) ? null : rd.GetString(cNom);
            u.PrenomUti = rd.IsDBNull(cPre) ? null : rd.GetString(cPre);
            u.MailUti = rd.GetString(cMail);
            u.MdpUti = rd.GetFieldValue<byte[]>(cMdp);               // VARBINARY(64)
            u.DateInscription = DateOnly.FromDateTime(rd.GetDateTime(cDate)); // SQL DATE -> DateOnly
            return u;
        }

        // ---------- Lire ----------

        public Utilisateurs? Lire(int idUti)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription
FROM utilisateurs
WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;
            cmd.Prepare();

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            Utilisateurs? u = null;
            bool aUneLigne = rd.Read();
            if (aUneLigne)
            {
                u = MapReader(rd);
            }
            return u;
        }

        public Utilisateurs? LireParEmail(string mailUti)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription
FROM utilisateurs
WHERE mailUti = @mail;";
            cmd.Parameters.Add("@mail", SqlDbType.VarChar, 255).Value = mailUti;
            cmd.Prepare();

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            Utilisateurs? u = null;
            bool aUneLigne = rd.Read();
            if (aUneLigne)
            {
                u = MapReader(rd);
            }
            return u;
        }

        public List<Utilisateurs> Lister()
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription
FROM utilisateurs
ORDER BY idUti;";
            cmd.Prepare();

            var list = new List<Utilisateurs>();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Utilisateurs u = MapReader(rd);
                list.Add(u);
            }
            return list;
        }

        // ---------- Enregistrer ----------

        // Création : le hash est déjà calculé par la couche métier
        public Utilisateurs Enregistrer(Utilisateurs u)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
INSERT INTO utilisateurs (nomUti, prenomUti, mailUti, mdpUti, dateInscription)
VALUES (@nom, @prenom, @mail, @mdp, @date);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            cmd.Parameters.Add("@nom", SqlDbType.VarChar, 50).Value = (object?)u.NomUti ?? DBNull.Value;
            cmd.Parameters.Add("@prenom", SqlDbType.VarChar, 50).Value = (object?)u.PrenomUti ?? DBNull.Value;
            cmd.Parameters.Add("@mail", SqlDbType.VarChar, 255).Value = u.MailUti;
            cmd.Parameters.Add("@mdp", SqlDbType.VarBinary, 64).Value = u.MdpUti;

            DateTime dt = ToDbDate(u.DateInscription);  // DateOnly -> DATE (minuit)
            cmd.Parameters.Add("@date", SqlDbType.Date).Value = dt;

            cmd.Prepare();

            object o = cmd.ExecuteScalar();
            int newId = Convert.ToInt32(o);
            u.IdUti = newId;

            return u;
        }

        // ---------- Modifier ----------

        public bool ModifierIdentite(int idUti, string? nom, string? prenom)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
UPDATE utilisateurs
SET nomUti = COALESCE(@nom, nomUti),
    prenomUti = COALESCE(@prenom, prenomUti)
WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;
            cmd.Parameters.Add("@nom", SqlDbType.VarChar, 50).Value = (object?)nom ?? DBNull.Value;
            cmd.Parameters.Add("@prenom", SqlDbType.VarChar, 50).Value = (object?)prenom ?? DBNull.Value;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        public bool ModifierEmail(int idUti, string newEmail)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"UPDATE utilisateurs SET mailUti = @mail WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;
            cmd.Parameters.Add("@mail", SqlDbType.VarChar, 255).Value = newEmail;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        public bool ModifierMotDePasse(int idUti, byte[] newHash)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"UPDATE utilisateurs SET mdpUti = @mdp WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;
            cmd.Parameters.Add("@mdp", SqlDbType.VarBinary, 64).Value = newHash;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        // ---------- Supprimer ----------

        public bool Supprimer(int idUti)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"DELETE FROM utilisateurs WHERE idUti = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idUti;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        // --- Dispose ---
        // manière la plus sûre de libérer la connexion proprement
        /*
        using var repo = new UtilisateursRepo();
        // ... utilisation
        // Dispose() sera appelé automatiquement à la fin du using

        // En ASP.NET Core via DI :
        // Program.cs
        // builder.Services.AddScoped<IUtilisateursRepository, UtilisateursRepo>();
        */
        public void Dispose()
        {
            if (activeConnexion != null)
            {
                activeConnexion.Dispose();
                activeConnexion = null!;
            }
        }
    }
}

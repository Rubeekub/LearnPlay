using exoModelsProjet.Models;
using LearnPlay.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.repository
{
    internal class UtilisateursRepo
    {
        private SqlConnection? activeConnexion;

        public void dbConnecter()
        {
            Connexion _cs = new Connexion();
            this.activeConnexion = _cs.OpenConnection();
            Console.WriteLine("Connexion établie");
        }

        public Utilisateurs GetById(int idUti)
        {
            dbConnecter();

            using SqlCommand cmd = activeConnexion.CreateCommand();

            cmd.CommandText = @"
            SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription, idProfActif
            FROM utilisateurs WHERE idUti=@id;";
            cmd.Parameters.AddWithValue("@id", idUti);

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            var u = new Utilisateurs();
            u.IdUti = rd.GetInt32(0);
            u.NomUti = rd.IsDBNull(1) ? null : rd.GetString(1);
            u.PrenomUti = rd.IsDBNull(2) ? null : rd.GetString(2);
            u.MailUti = rd.GetString(3);
            u.MdpUti = (byte[])rd["mdpUti"];
            u.DateInscription = rd.GetDateTime(5);
            u.IdProfActif = rd.IsDBNull(6) ? (int?)null : rd.GetInt32(6);
            return u;
        }

        public List<Utilisateurs> GetAll()
        {
            var list = new List<Utilisateurs>();
            using var cn = new SqlConnection(_cs);
            cn.Open();
            var cmd = new SqlCommand(@"
            SELECT idUti, nomUti, prenomUti, mailUti, mdpUti, dateInscription, idProfActif
            FROM utilisateurs;", cn);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var u = new Utilisateurs();
                u.IdUti = rd.GetInt32(0);
                u.NomUti = rd.IsDBNull(1) ? null : rd.GetString(1);
                u.PrenomUti = rd.IsDBNull(2) ? null : rd.GetString(2);
                u.MailUti = rd.GetString(3);
                u.MdpUti = (byte[])rd["mdpUti"];
                u.DateInscription = rd.GetDateTime(5);
                u.IdProfActif = rd.IsDBNull(6) ? (int?)null : rd.GetInt32(6);
                list.Add(u);
            }
            return list;
        }

        public int Create(string nom, string prenom, string mail, byte[] mdpHash, DateTime dateInscription)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();
            var cmd = new SqlCommand(@"
INSERT INTO utilisateurs(nomUti, prenomUti, mailUti, mdpUti, dateInscription)
VALUES(@n,@p,@m,@h,@d);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);
            cmd.Parameters.AddWithValue("@n", (object?)nom ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p", (object?)prenom ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@m", mail);
            cmd.Parameters.Add("@h", System.Data.SqlDbType.VarBinary, 64).Value = mdpHash;
            cmd.Parameters.AddWithValue("@d", dateInscription);
            var id = (int)cmd.ExecuteScalar();
            return id;
        }

        public bool UpdateEmail(int idUti, string newMail)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();
            var cmd = new SqlCommand("UPDATE utilisateurs SET mailUti=@m WHERE idUti=@id;", cn);
            cmd.Parameters.AddWithValue("@m", newMail);
            cmd.Parameters.AddWithValue("@id", idUti);
            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        public bool UpdatePassword(int idUti, byte[] oldHash, byte[] newHash)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();

            // 1) lire hash courant
            var cmdRead = new SqlCommand("SELECT mdpUti FROM utilisateurs WHERE idUti=@id;", cn);
            cmdRead.Parameters.AddWithValue("@id", idUti);
            var current = (byte[])cmdRead.ExecuteScalar();
            if (current == null) return false;

            // 2) comparer
            var sameLength = current.Length == oldHash.Length;
            if (!sameLength) return false;
            var i = 0;
            while (i < current.Length)
            {
                if (current[i] != oldHash[i]) return false;
                i = i + 1;
            }

            // 3) update
            var cmd = new SqlCommand("UPDATE utilisateurs SET mdpUti=@h WHERE idUti=@id;", cn);
            cmd.Parameters.Add("@h", System.Data.SqlDbType.VarBinary, 64).Value = newHash;
            cmd.Parameters.AddWithValue("@id", idUti);
            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        public int? GetActiveProfileId(int idUti)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();
            var cmd = new SqlCommand("SELECT idProfActif FROM utilisateurs WHERE idUti=@id;", cn);
            cmd.Parameters.AddWithValue("@id", idUti);
            var val = cmd.ExecuteScalar();
            if (val == null || val == DBNull.Value) return null;
            return (int)val;
        }

        public bool SetActiveProfile(int idUti, int idProf)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();

            // vérifier appartenance profil -> utilisateur
            var cmdCheck = new SqlCommand("SELECT COUNT(*) FROM profils WHERE idProf=@p AND idUtiProf=@u;", cn);
            cmdCheck.Parameters.AddWithValue("@p", idProf);
            cmdCheck.Parameters.AddWithValue("@u", idUti);
            var count = (int)cmdCheck.ExecuteScalar();
            if (count == 0) return false;

            // maj
            var cmd = new SqlCommand("UPDATE utilisateurs SET idProfActif=@p WHERE idUti=@u;", cn);
            cmd.Parameters.AddWithValue("@p", idProf);
            cmd.Parameters.AddWithValue("@u", idUti);
            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        public bool RequestDeletion(int idUti)
        {
            // Placeholder : à brancher sur une SP si tu implémentes la “mise en attente 15j”
            // Exemple : EXEC DemanderSuppression @idUti
            return true;
        }
    }
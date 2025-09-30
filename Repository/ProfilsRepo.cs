// ProfilsRepo.cs
using LearnPlay;
using LearnPlay.Interfaces;
using LearnPlay.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LearnPlay.Repository
{
    public class ProfilsRepo : IProfilsRepository
    {
        private SqlConnection activeConnexion;

        // --- Construction / Connexion ---
        public ProfilsRepo()
        {
            dbConnecter();
        }

        private void dbConnecter()
        {
            var con = new Connexion();
            activeConnexion = con.GetConnection();
            if (activeConnexion.State != ConnectionState.Open)
                activeConnexion.Open();
        }

        private void VerifConnexion()
        {
            if (activeConnexion == null || activeConnexion.State != ConnectionState.Open)
                dbConnecter();
        }

        // --- Helpers ---
        private static object DateOnlyToDb(DateOnly d)
            => d == DateOnly.MinValue ? DBNull.Value : DateOnlyToDateTime(d);

        private static DateTime DateOnlyToDateTime(DateOnly d)
            => d.ToDateTime(TimeOnly.MinValue);

        private static DateOnly DbToDateOnlyOrMin(object dbValue)
            => dbValue == DBNull.Value ? DateOnly.MinValue : DateOnly.FromDateTime((DateTime)dbValue);

        //  Afficher un profil par id
        public Profils? AfficheProfil(int idProf)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
SELECT idProf, numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf
FROM profils
WHERE idProf = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            // On lit par noms de colonnes pour éviter les erreurs d’index
            var profil = new Profils
            {
                idProf = rd.GetInt32(rd.GetOrdinal("idProf")),
                numProf = rd.GetInt32(rd.GetOrdinal("numProf")),
                pseudoProf = rd.GetString(rd.GetOrdinal("pseudoProf")),
                pointsProf = rd.GetInt32(rd.GetOrdinal("pointsProf")),
                nivProf = rd.GetInt32(rd.GetOrdinal("nivProf")),
                dateNaissanceProf = rd.IsDBNull(rd.GetOrdinal("dateNaissanceProf"))
                    ? DateOnly.MinValue
                    : DateOnly.FromDateTime(rd.GetDateTime(rd.GetOrdinal("dateNaissanceProf"))),
                idUtiProf = rd.GetInt32(rd.GetOrdinal("idUtiProf")),
                idRoleProf = rd.GetInt32(rd.GetOrdinal("idRoleProf"))
            };

            return profil;
        }

        //  Lister les profils d’un utilisateur
        public List<Profils> GetByUsersId(int idUti)
        {
            VerifConnexion();
            var list = new List<Profils>();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
SELECT idProf, numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf
FROM profils
WHERE idUtiProf = @u
ORDER BY numProf;";
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = idUti;

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var p = new Profils
                {
                    idProf = rd.GetInt32(rd.GetOrdinal("idProf")),
                    numProf = rd.GetInt32(rd.GetOrdinal("numProf")),
                    pseudoProf = rd.GetString(rd.GetOrdinal("pseudoProf")),
                    pointsProf = rd.GetInt32(rd.GetOrdinal("pointsProf")),
                    nivProf = rd.GetInt32(rd.GetOrdinal("nivProf")),
                    dateNaissanceProf = rd.IsDBNull(rd.GetOrdinal("dateNaissanceProf"))
                        ? DateOnly.MinValue
                        : DateOnly.FromDateTime(rd.GetDateTime(rd.GetOrdinal("dateNaissanceProf"))),
                    idUtiProf = rd.GetInt32(rd.GetOrdinal("idUtiProf")),
                    idRoleProf = rd.GetInt32(rd.GetOrdinal("idRoleProf"))
                };
                list.Add(p);
            }

            return list;
        }

        //  Créer un profil
        //  vérifie l’unicité du pseudo
        //  renvoie l’objet créé avec id
        public Profils Create(Profils p)
        {
            VerifConnexion();

            // Unicité du pseudo (à affiner si tu veux par utilisateur)
            using (var cmdSelect = activeConnexion.CreateCommand())
            {
                cmdSelect.CommandText = "SELECT COUNT(*) FROM profils WHERE pseudoProf = @ps;";
                cmdSelect.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = p.pseudoProf ?? string.Empty;
                var exists = (int)cmdSelect.ExecuteScalar();
                if (exists > 0) throw new InvalidOperationException("Pseudo déjà utilisé.");
            }

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
INSERT INTO profils (numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf)
VALUES (@n, @ps, @pt, @niv, @dn, @u, @r);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            cmd.Parameters.Add("@n", SqlDbType.Int).Value = p.numProf;
            cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = p.pseudoProf ?? string.Empty;
            cmd.Parameters.Add("@pt", SqlDbType.Int).Value = p.pointsProf;
            cmd.Parameters.Add("@niv", SqlDbType.Int).Value = p.nivProf;
            cmd.Parameters.Add("@dn", SqlDbType.Date).Value = DateOnlyToDb(p.dateNaissanceProf);
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = p.idUtiProf;
            cmd.Parameters.Add("@r", SqlDbType.Int).Value = p.idRoleProf;

            var newId = (int)cmd.ExecuteScalar();

            // Relire l’objet inséré (sécurité + valeurs DB)
            return AfficheProfil(newId)!;
        }


        //  Mettre à jour le pseudo (avec contrôle d’unicité)

        public bool UpdatePseudo(int idProf, string newPseudo)
        {
            VerifConnexion();

            using (var cmdSelect = activeConnexion.CreateCommand())
            {
                cmdSelect.CommandText = "SELECT COUNT(*) FROM profils WHERE pseudoProf = @ps AND idProf <> @id;";
                cmdSelect.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = newPseudo ?? string.Empty;
                cmdSelect.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
                var exists = (int)cmdSelect.ExecuteScalar();
                if (exists > 0) return false;
            }

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE profils SET pseudoProf = @ps WHERE idProf = @id;";
            cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = newPseudo ?? string.Empty;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;

            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        //  Incrémenter/décrémenter des points (éviter négatif)

        public bool UpdatePoints(int idProf, int delta)
        {
            VerifConnexion();

            int current;
            using (var cmdRead = activeConnexion.CreateCommand())
            {
                cmdRead.CommandText = "SELECT pointsProf FROM profils WHERE idProf = @id;";
                cmdRead.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
                var curObj = cmdRead.ExecuteScalar();
                if (curObj == null || curObj == DBNull.Value) return false;
                current = (int)curObj;
            }

            var next = current + delta;
            if (next < 0) next = 0; // choix fonctionnel : clamp à 0

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE profils SET pointsProf = @p WHERE idProf = @id;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = next;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;

            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        //  Mise à jour du « cœur » du profil : pseudo, avatar, thème, date de naissance, niveau
        public bool UpdateCore(int idProf, string? pseudo, int? niveau, DateOnly? dateNaissance)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = @"
UPDATE profils
SET
  pseudoProf = COALESCE(@ps, pseudoProf),
  nivProf = COALESCE(@niv, nivProf),
  dateNaissanceProf = COALESCE(@dn, dateNaissanceProf)
WHERE idProf = @id;";
            cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = (object?)pseudo ?? DBNull.Value;
            cmd.Parameters.Add("@niv", SqlDbType.Int).Value = (object?)niveau ?? DBNull.Value;
            cmd.Parameters.Add("@dn", SqlDbType.Date).Value = dateNaissance.HasValue ? DateOnlyToDateTime(dateNaissance.Value) : DBNull.Value;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;

            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }
        // ... dans LearnPlay.Repository.ProfilsRepo

        public bool UpdateCore(int idProf, string? pseudo = null, int? niveau = null, DateOnly? dateNaissance = null, int? themeId = null, int? avatarId = null)
        {
            VerifConnexion();

            using var tx = activeConnexion.BeginTransaction();
            try
            {
                // MAJ des champs du profil (optionnels)
                using (var cmd = activeConnexion.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
UPDATE profils
SET
  pseudoProf = COALESCE(@ps, pseudoProf),
  nivProf = COALESCE(@niv, nivProf),
  dateNaissanceProf = COALESCE(@dn, dateNaissanceProf)
WHERE idProf = @id;";
                    cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = (object?)pseudo ?? DBNull.Value;
                    cmd.Parameters.Add("@niv", SqlDbType.Int).Value = (object?)niveau ?? DBNull.Value;
                    cmd.Parameters.Add("@dn", SqlDbType.Date).Value = dateNaissance.HasValue
                        ? dateNaissance.Value.ToDateTime(TimeOnly.MinValue)
                        : (object)DBNull.Value;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;

                    cmd.ExecuteNonQuery();
                }

                // Thème actif (table profilsThemes)
                if (themeId.HasValue)
                {
                    using var cmd0 = activeConnexion.CreateCommand();
                    cmd0.Transaction = tx;
                    // Mettre tous les thèmes de ce profil à inactif
                    cmd0.CommandText = "UPDATE profilsThemes SET isActif = 0 WHERE idProfTheme = @idProf;";
                    cmd0.Parameters.Add("@idProf", SqlDbType.Int).Value = idProf;
                    cmd0.ExecuteNonQuery();

                    using var cmd1 = activeConnexion.CreateCommand();
                    cmd1.Transaction = tx;
                    // Activer (ou insérer+activer) le thème demandé
                    cmd1.CommandText = @"
IF EXISTS (SELECT 1 FROM profilsThemes WHERE idThemeProf=@t AND idProfTheme=@p)
    UPDATE profilsThemes SET isActif=1 WHERE idThemeProf=@t AND idProfTheme=@p;
ELSE
    INSERT INTO profilsThemes(idThemeProf, idProfTheme, isActif) VALUES(@t, @p, 1);";
                    cmd1.Parameters.Add("@t", SqlDbType.Int).Value = themeId.Value;
                    cmd1.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
                    cmd1.ExecuteNonQuery();
                }

                // Avatar actif (table profilsImages)
                if (avatarId.HasValue)
                {
                    using var cmd0 = activeConnexion.CreateCommand();
                    cmd0.Transaction = tx;
                    cmd0.CommandText = "UPDATE profilsImages SET isActif = 0 WHERE idProfImg = @idProf;";
                    cmd0.Parameters.Add("@idProf", SqlDbType.Int).Value = idProf;
                    cmd0.ExecuteNonQuery();

                    using var cmd1 = activeConnexion.CreateCommand();
                    cmd1.Transaction = tx;
                    cmd1.CommandText = @"
IF EXISTS (SELECT 1 FROM profilsImages WHERE idImgProf=@i AND idProfImg=@p)
    UPDATE profilsImages SET isActif=1 WHERE idImgProf=@i AND idProfImg=@p;
ELSE
    INSERT INTO profilsImages(idImgProf, idProfImg, isActif) VALUES(@i, @p, 1);";
                    cmd1.Parameters.Add("@i", SqlDbType.Int).Value = avatarId.Value;
                    cmd1.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
                    cmd1.ExecuteNonQuery();
                }

                tx.Commit();
                return true;
            }
            catch
            {
                try { tx.Rollback(); } catch { }
                return false;
            }
        }

        public bool SetProfilActifPourUtilisateur(int idUti, int idProf)
        {
            VerifConnexion();
            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE utilisateurs SET IdProfActif = @p WHERE idUti = @u;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = idUti;
            var rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        public int? GetThemeActifId(int idProf)
        {
            VerifConnexion();
            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "SELECT idThemeProf FROM profilsThemes WHERE idProfTheme=@p AND isActif=1;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
            var o = cmd.ExecuteScalar();
            return (o == null || o == DBNull.Value) ? (int?)null : Convert.ToInt32(o);
        }

        public int? GetAvatarActifId(int idProf)
        {
            VerifConnexion();
            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "SELECT idImgProf FROM profilsImages WHERE idProfImg=@p AND isActif=1;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
            var o = cmd.ExecuteScalar();
            return (o == null || o == DBNull.Value) ? (int?)null : Convert.ToInt32(o);
        }
    }
}

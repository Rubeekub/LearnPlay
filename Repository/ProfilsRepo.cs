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
    public class ProfilsRepo : IProfilsRepository, IDisposable
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

        private static Profils MapProfil(SqlDataReader rd)
        {
            int cId = rd.GetOrdinal("idProf");
            int cNum = rd.GetOrdinal("numProf");
            int cPs = rd.GetOrdinal("pseudoProf");
            int cPts = rd.GetOrdinal("pointsProf");
            int cNiv = rd.GetOrdinal("nivProf");
            int cDn = rd.GetOrdinal("dateNaissanceProf");
            int cU = rd.GetOrdinal("idUtiProf");
            int cR = rd.GetOrdinal("idRoleProf");

            var p = new Profils();
            p.idProf = rd.GetInt32(cId);
            p.numProf = rd.GetInt32(cNum);
            p.pseudoProf = rd.GetString(cPs);
            p.pointsProf = rd.GetInt32(cPts);
            p.nivProf = rd.GetInt32(cNiv);
            if (rd.IsDBNull(cDn))
            {
                p.dateNaissanceProf = DateOnly.MinValue;
            }
            else
            {
                p.dateNaissanceProf = DateOnly.FromDateTime(rd.GetDateTime(cDn));
            }
            p.idUtiProf = rd.GetInt32(cU);
            p.idRoleProf = rd.GetInt32(cR);

            return p;
        }

        // ---------- Lire ----------

        // Afficher un profil (numéro, pseudo, points, niveau, date de naissance)
        public Profils? AfficheProfil(int idProf)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
SELECT idProf, numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf
FROM profils
WHERE idProf = @id;";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
            cmd.Prepare();

            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            Profils? profil = null;
            bool aUneLigne = rd.Read();
            if (aUneLigne)
            {
                profil = MapProfil(rd);
            }
            return profil;
        }

        // Lister les profils d’un utilisateur
        public List<Profils> ListerUtilisateur(int idUti)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
SELECT idProf, numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf
FROM profils
WHERE idUtiProf = @u
ORDER BY numProf;";
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = idUti;
            cmd.Prepare();

            var list = new List<Profils>();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Profils p = MapProfil(rd);
                list.Add(p);
            }
            return list;
        }

        // ---------- CREATE ----------
        public Profils CreerProfil(Profils p)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText =
@"
INSERT INTO profils (numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf)
VALUES (@n, @ps, @pt, @niv, @dn, @u, @r);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            cmd.Parameters.Add("@n", SqlDbType.Int).Value = p.numProf;
            cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = p.pseudoProf ?? string.Empty;
            cmd.Parameters.Add("@pt", SqlDbType.Int).Value = p.pointsProf;
            cmd.Parameters.Add("@niv", SqlDbType.Int).Value = p.nivProf;
            if (p.dateNaissanceProf == DateOnly.MinValue)
            {
                cmd.Parameters.Add("@dn", SqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@dn", SqlDbType.Date).Value = ToDbDate(p.dateNaissanceProf);
            }
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = p.idUtiProf;
            cmd.Parameters.Add("@r", SqlDbType.Int).Value = p.idRoleProf;

            cmd.Prepare();

            object o = cmd.ExecuteScalar();
            int newId = Convert.ToInt32(o);
            p.idProf = newId;

            return p;
        }

        // ---------- UPDATE ----------

        // Mettre à jour le pseudo (avec contrôle d’unicité)
        public bool ModifierPseudo(int idProf, string newPseudo)
        {
            VerifConnexion();

            int dejaPris;
            using (var cmdCheck = activeConnexion.CreateCommand())
            {
                cmdCheck.CommandText = "SELECT COUNT(*) FROM profils WHERE pseudoProf = @ps AND idProf <> @id;";
                cmdCheck.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = newPseudo ?? string.Empty;
                cmdCheck.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
                object countObj = cmdCheck.ExecuteScalar();
                dejaPris = Convert.ToInt32(countObj);
            }

            if (dejaPris > 0)
            {
                bool resultatKo = false;
                return resultatKo;
            }

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE profils SET pseudoProf = @ps WHERE idProf = @id;";
            cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = newPseudo ?? string.Empty;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        // Incrémenter/décrémenter les points (clamp à 0)
        public bool ModifierPoints(int idProf, int delta)
        {
            VerifConnexion();

            int current;
            using (var cmdRead = activeConnexion.CreateCommand())
            {
                cmdRead.CommandText = "SELECT pointsProf FROM profils WHERE idProf = @id;";
                cmdRead.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
                cmdRead.Prepare();

                object curObj = cmdRead.ExecuteScalar();
                if (curObj == null || curObj == DBNull.Value)
                {
                    bool resultatKo = false;
                    return resultatKo;
                }
                current = Convert.ToInt32(curObj);
            }

            int next = current + delta;
            if (next < 0)
            {
                next = 0;
            }

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE profils SET pointsProf = @p WHERE idProf = @id;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = next;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        // Mise à jour “cœur” : pseudo/niveau/date de naissance (+ thème & avatar actifs)
        public bool MettreAJourCoeurProfil(
            int idProf,
            string? pseudo = null,
            int? niveau = null,
            DateOnly? dateNaissance = null,
            int? themeId = null,
            int? avatarId = null)
        {
            VerifConnexion();

            using var tx = activeConnexion.BeginTransaction();
            try
            {
                using (var cmd = activeConnexion.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText =
@"
UPDATE profils
SET
  pseudoProf = COALESCE(@ps, pseudoProf),
  nivProf = COALESCE(@niv, nivProf),
  dateNaissanceProf = COALESCE(@dn, dateNaissanceProf)
WHERE idProf = @id;";
                    cmd.Parameters.Add("@ps", SqlDbType.VarChar, 50).Value = (object?)pseudo ?? DBNull.Value;
                    cmd.Parameters.Add("@niv", SqlDbType.Int).Value = (object?)niveau ?? DBNull.Value;
                    if (dateNaissance.HasValue)
                    {
                        cmd.Parameters.Add("@dn", SqlDbType.Date).Value = ToDbDate(dateNaissance.Value);
                    }
                    else
                    {
                        cmd.Parameters.Add("@dn", SqlDbType.Date).Value = DBNull.Value;
                    }
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = idProf;
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }

                if (themeId.HasValue)
                {
                    using (var cmd0 = activeConnexion.CreateCommand())
                    {
                        cmd0.Transaction = tx;
                        cmd0.CommandText = "UPDATE profilsThemes SET isActif = 0 WHERE idProfTheme = @p;";
                        cmd0.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
                        cmd0.Prepare();
                        cmd0.ExecuteNonQuery();
                    }

                    using (var cmd1 = activeConnexion.CreateCommand())
                    {
                        cmd1.Transaction = tx;
                        cmd1.CommandText =
@"
IF EXISTS (SELECT 1 FROM profilsThemes WHERE idThemeProf=@t AND idProfTheme=@p)
    UPDATE profilsThemes SET isActif=1 WHERE idThemeProf=@t AND idProfTheme=@p;
else
    INSERT INTO profilsThemes(idThemeProf, idProfTheme, isActif) VALUES(@t, @p, 1);";
                        cmd1.Parameters.Add("@t", SqlDbType.Int).Value = themeId.Value;
                        cmd1.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
                        cmd1.Prepare();
                        cmd1.ExecuteNonQuery();
                    }
                }

                if (avatarId.HasValue)
                {
                    using (var cmd0 = activeConnexion.CreateCommand())
                    {
                        cmd0.Transaction = tx;
                        cmd0.CommandText = "UPDATE profilsImages SET isActif = 0 WHERE idProfImg = @p;";
                        cmd0.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
                        cmd0.Prepare();
                        cmd0.ExecuteNonQuery();
                    }

                    using (var cmd1 = activeConnexion.CreateCommand())
                    {
                        cmd1.Transaction = tx;
                        cmd1.CommandText =
@"
IF EXISTS (SELECT 1 FROM profilsImages WHERE idImgProf=@i AND idProfImg=@p)
    UPDATE profilsImages SET isActif=1 WHERE idImgProf=@i AND idProfImg=@p;
ELSE
    INSERT INTO profilsImages(idImgProf, idProfImg, isActif) VALUES(@i, @p, 1);";
                        cmd1.Parameters.Add("@i", SqlDbType.Int).Value = avatarId.Value;
                        cmd1.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
                        cmd1.Prepare();
                        cmd1.ExecuteNonQuery();
                    }
                }

                tx.Commit();
                bool resultatOk = true;
                return resultatOk;
            }
            catch
            {
                try { tx.Rollback(); } catch { }
                bool resultatKo = false;
                return resultatKo;
            }
        }

        // Changer de profil actif pour l’utilisateur (utilisateurs.IdProfActif)
        // ⚠️ Nécessite la colonne IdProfActif dans la table 'utilisateurs'.
        public bool ChangerProfilActif(int idUti, int idProf)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "UPDATE utilisateurs SET IdProfActif = @p WHERE idUti = @u;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
            cmd.Parameters.Add("@u", SqlDbType.Int).Value = idUti;
            cmd.Prepare();

            int rows = cmd.ExecuteNonQuery();
            bool resultat = rows > 0;
            return resultat;
        }

        // Obtenir l’ID du thème actif (ou null si aucun)
        public int? LireThemeActif(int idProf)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "SELECT idThemeProf FROM profilsThemes WHERE idProfTheme=@p AND isActif=1;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
            cmd.Prepare();

            object o = cmd.ExecuteScalar();
            int? id = null;
            if (o != null && o != DBNull.Value)
            {
                id = Convert.ToInt32(o);
            }
            return id;
        }

        // Obtenir l’ID de l’avatar actif (ou null si aucun)
        public int? LireAvatarActif(int idProf)
        {
            VerifConnexion();

            using var cmd = activeConnexion.CreateCommand();
            cmd.CommandText = "SELECT idImgProf FROM profilsImages WHERE idProfImg=@p AND isActif=1;";
            cmd.Parameters.Add("@p", SqlDbType.Int).Value = idProf;
            cmd.Prepare();

            object o = cmd.ExecuteScalar();
            int? id = null;
            if (o != null && o != DBNull.Value)
            {
                id = Convert.ToInt32(o);
            }
            return id;
        }

        // --- Dispose ---
        // manière la plus sûre de libérer la connexion proprement
        /*
        using var repo = new ProfilsRepo();
        // ... utilisation
        // Dispose() sera appelé automatiquement à la fin du using

        // En ASP.NET Core via DI :
        // Program.cs
        // builder.Services.AddScoped<IProfilsRepository, ProfilsRepo>();
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

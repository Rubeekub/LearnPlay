using exoModelsProjet.Models;
using LearnPlay.Data;
using LearnPlay.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace LearnPlay.Repository
{
    public class ProfilsRepo : IProfilsRepository
    {
        private SqlConnection? activeConnexion;

        public void dbConnecter()
        {
            Connexion _cs = new Connexion();
            this.activeConnexion = _cs.OpenConnection();
            Console.WriteLine("Connexion établie");
        }

        public Profils GetById(int idProf)
    {
        using var cn = new SqlConnection(_cs);
        cn.Open();
        var cmd = new SqlCommand(@"
SELECT idProf, numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf
FROM profils WHERE idProf=@id;", cn);
        cmd.Parameters.AddWithValue("@id", idProf);

        using var rd = cmd.ExecuteReader();
        if (!rd.Read()) return null;

        var p = new Profils();
        p.IdProf = rd.GetInt32(0);
        p.NumProf = rd.GetInt32(1);
        p.PseudoProf = rd.GetString(2);
        p.PointsProf = rd.GetInt32(3);
        p.NivProf = rd.GetInt32(4);
        p.DateNaissanceProf = rd.IsDBNull(5) ? (DateTime?)null : rd.GetDateTime(5);
        p.IdUtiProf = rd.GetInt32(6);
        p.IdRoleProf = rd.GetInt32(7);
        return p;
    }

    public List<Profils> GetByUserId(int idUti)
    {
        var list = new List<Profils>();
        using var cn = new SqlConnection(_cs);
        cn.Open();
        var cmd = new SqlCommand(@"
SELECT idProf, numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf
FROM profils WHERE idUtiProf=@u ORDER BY numProf;", cn);
        cmd.Parameters.AddWithValue("@u", idUti);

        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            var p = new Profils();
            p.IdProf = rd.GetInt32(0);
            p.NumProf = rd.GetInt32(1);
            p.PseudoProf = rd.GetString(2);
            p.PointsProf = rd.GetInt32(3);
            p.NivProf = rd.GetInt32(4);
            p.DateNaissanceProf = rd.IsDBNull(5) ? (DateTime?)null : rd.GetDateTime(5);
            p.IdUtiProf = rd.GetInt32(6);
            p.IdRoleProf = rd.GetInt32(7);
            list.Add(p);
        }
        return list;
    }

    public int Create(Profils p)
    {
        // Vérif pseudo unique (en plus de la contrainte DB)
        using var cn = new SqlConnection(_cs);
        cn.Open();

        var cmdCheck = new SqlCommand("SELECT COUNT(*) FROM profils WHERE pseudoProf=@ps;", cn);
        cmdCheck.Parameters.AddWithValue("@ps", p.PseudoProf);
        var exists = (int)cmdCheck.ExecuteScalar();
        if (exists > 0) throw new InvalidOperationException("Pseudo déjà utilisé.");

        var cmd = new SqlCommand(@"
INSERT INTO profils(numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf)
VALUES(@n,@ps,@pt,@niv,@dn,@u,@r);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);

        cmd.Parameters.AddWithValue("@n", p.NumProf);
        cmd.Parameters.AddWithValue("@ps", p.PseudoProf);
        cmd.Parameters.AddWithValue("@pt", p.PointsProf);
        cmd.Parameters.AddWithValue("@niv", p.NivProf);
        if (p.DateNaissanceProf.HasValue) cmd.Parameters.AddWithValue("@dn", p.DateNaissanceProf.Value);
        else cmd.Parameters.AddWithValue("@dn", DBNull.Value);
        cmd.Parameters.AddWithValue("@u", p.IdUtiProf);
        cmd.Parameters.AddWithValue("@r", p.IdRoleProf);

        var id = (int)cmd.ExecuteScalar();
        return id;
    }

    public bool UpdatePseudo(int idProf, string newPseudo)
    {
        using var cn = new SqlConnection(_cs);
        cn.Open();

        var cmdCheck = new SqlCommand("SELECT COUNT(*) FROM profils WHERE pseudoProf=@ps AND idProf<>@id;", cn);
        cmdCheck.Parameters.AddWithValue("@ps", newPseudo);
        cmdCheck.Parameters.AddWithValue("@id", idProf);
        var exists = (int)cmdCheck.ExecuteScalar();
        if (exists > 0) return false;

        var cmd = new SqlCommand("UPDATE profils SET pseudoProf=@ps WHERE idProf=@id;", cn);
        cmd.Parameters.AddWithValue("@ps", newPseudo);
        cmd.Parameters.AddWithValue("@id", idProf);
        var rows = cmd.ExecuteNonQuery();
        return rows == 1;
    }

    public bool UpdatePoints(int idProf, int delta)
    {
        using var cn = new SqlConnection(_cs);
        cn.Open();

        // Lire points actuels
        var cmdRead = new SqlCommand("SELECT pointsProf FROM profils WHERE idProf=@id;", cn);
        cmdRead.Parameters.AddWithValue("@id", idProf);
        var curObj = cmdRead.ExecuteScalar();
        if (curObj == null) return false;
        var current = (int)curObj;

        var next = current + delta;
        if (next < 0) return false; // évite l’échec du CHECK en DB (points >= 0)

        var cmd = new SqlCommand("UPDATE profils SET pointsProf=@p WHERE idProf=@id;", cn);
        cmd.Parameters.AddWithValue("@p", next);
        cmd.Parameters.AddWithValue("@id", idProf);
        var rows = cmd.ExecuteNonQuery();
        return rows == 1;
    }
}

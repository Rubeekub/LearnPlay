using LearnPlay.Interfaces;
using LearnPlay.Models;
using System;
using System.Collections.Generic;

namespace LearnPlay.Metiers
{
    public class UtilisateursMt : IUtilisateursMt
    {
        private readonly IUtilisateursRepository _repo;

        public UtilisateursMt(IUtilisateursRepository repo)
        {
            _repo = repo;
        }

        public Utilisateurs Lire(int idUti)
        {
            Utilisateurs u = _repo.GetById(idUti);
            return u;
        }

        public Utilisateurs LireParEmail(string email)
        {
            Utilisateurs u = _repo.GetByEmail(email);
            return u;
        }

        public List<Utilisateurs> Lister()
        {
            List<Utilisateurs> list = _repo.GetAll();
            return list;
        }

        public Utilisateurs Enregistrer(string nom, string prenom, string email, byte[] hashMdp)
        {
            Utilisateurs u = new Utilisateurs();
            if (string.IsNullOrWhiteSpace(nom))
            {
                u.NomUti = string.Empty;
            }
            else
            {
                u.NomUti = nom.Trim();
            }

            if (string.IsNullOrWhiteSpace(prenom))
            {
                u.PrenomUti = string.Empty;
            }
            else
            {
                u.PrenomUti = prenom.Trim();
            }

            u.MailUti = email;
            u.MdpUti = hashMdp;

            DateTime aujourdHui = DateTime.UtcNow.Date;
            u.DateInscription = aujourdHui;

            Utilisateurs created = _repo.Create(u);
            return created;
        }

        public bool ModifierIdentite(int idUti, string nom, string prenom)
        {
            string nomNettoye = nom;
            if (!string.IsNullOrWhiteSpace(nom))
            {
                nomNettoye = nom.Trim();
            }

            string prenomNettoye = prenom;
            if (!string.IsNullOrWhiteSpace(prenom))
            {
                prenomNettoye = prenom.Trim();
            }

            bool ok = _repo.UpdateIdentite(idUti, nomNettoye, prenomNettoye);
            return ok;
        }

        public bool ModifierEmail(int idUti, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string emailNettoye = email.Trim();
            bool ok = _repo.UpdateEmail(idUti, emailNettoye);
            return ok;
        }

        public bool ModifierMotDePasse(int idUti, byte[] hashMdp)
        {
            if (hashMdp == null)
            {
                return false;
            }

            bool ok = _repo.UpdatePasswordHash(idUti, hashMdp);
            return ok;
        }

        public bool Supprimer(int idUti)
        {
            bool ok = _repo.Delete(idUti);
            return ok;
        }
    }
}

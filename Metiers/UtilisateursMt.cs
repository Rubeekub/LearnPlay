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
            Utilisateurs u = _repo.Lire(idUti);
            return u;
        }

        public Utilisateurs LireParEmail(string email)
        {
            Utilisateurs u = _repo.LireParEmail(email);
            return u;
        }

        public List<Utilisateurs> Lister()
        {
            List<Utilisateurs> list = _repo.Lister();
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
            u.DateInscription = DateOnly.FromDateTime(DateTime.UtcNow);

            Utilisateurs created = _repo.Enregistrer(u);
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

            bool resultat = _repo.ModifierIdentite(idUti, nomNettoye, prenomNettoye);
            return resultat;
        }

        public bool ModifierEmail(int idUti, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                bool resultatKo = false;
                return resultatKo;
            }

            string emailNettoye = email.Trim();
            bool resultat = _repo.ModifierEmail(idUti, emailNettoye);
            return resultat;
        }

        public bool ModifierMotDePasse(int idUti, byte[] hashMdp)
        {
            if (hashMdp == null)
            {
                bool resultatKo = false;
                return resultatKo;
            }

            bool resultat = _repo.ModifierMotDePasse(idUti, hashMdp);
            return resultat;
        }

        public bool Supprimer(int idUti)
        {
            bool resultat = _repo.Supprimer(idUti);
            return resultat;
        }
    }
}

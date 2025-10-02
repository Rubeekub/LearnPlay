// Metiers/ProfilsMt.cs
using LearnPlay.Interfaces;
using LearnPlay.Models;
using System;
using System.Collections.Generic;

namespace LearnPlay.Metiers
{
    public class ProfilsMt : IProfilsMt
    {
        private readonly IProfilsRepository _repo;

        public ProfilsMt(IProfilsRepository repo)
        {
            _repo = repo;
        }

        public Profils? AfficheProfil(int idProf)
        {
            Profils? profil = _repo.AfficheProfil(idProf);
            return profil;
        }

        public List<Profils> ListerUtilisateur(int idUtilisateur)
        {
            List<Profils> list = _repo.ListerUtilisateur(idUtilisateur);
            return list;
        }

        public Profils CreerProfil(Profils nouveau)
        {
            Profils created = _repo.CreerProfil(nouveau);
            return created;
        }

        public bool ModifierPseudo(int idProf, string nouveauPseudo)
        {
            bool resultat = _repo.ModifierPseudo(idProf, nouveauPseudo);
            return resultat;
        }

        public bool AjouterPoints(int idProf, int points)
        {
            bool resultat;
            if (points <= 0)
            {
                resultat = false;
                return resultat;
            }

            resultat = _repo.ModifierPoints(idProf, points);
            return resultat;
        }

        public bool RetirerPoints(int idProf, int points)
        {
            bool resultat;
            if (points <= 0)
            {
                resultat = false;
                return resultat;
            }

            resultat = _repo.ModifierPoints(idProf, -points);
            return resultat;
        }

        public bool MettreAJourCoeurProfil(
            int idProf,
            string? pseudo = null,
            int? niveau = null,
            DateOnly? dateNaissance = null,
            int? themeId = null,
            int? avatarId = null)
        {
            bool resultat = _repo.MettreAJourCoeurProfil(idProf, pseudo, niveau, dateNaissance, themeId, avatarId);
            return resultat;
        }

        public bool ChangerProfilActif(int idUtilisateur, int idProf)
        {
            bool resultat = _repo.ChangerProfilActif(idUtilisateur, idProf);
            return resultat;
        }

        public int? LireThemeActif(int idProf)
        {
            int? id = _repo.LireThemeActif(idProf);
            return id;
        }

        public int? LireAvatarActif(int idProf)
        {
            int? id = _repo.LireAvatarActif(idProf);
            return id;
        }
    }
}

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

        public Profils? GetProfil(int idProf)
            => _repo.AfficheProfil(idProf);

        public List<Profils> GetProfilsUtilisateur(int idUtilisateur)
            => _repo.GetByUsersId(idUtilisateur);

        public Profils CreerProfil(Profils nouveau)
            => _repo.Create(nouveau);

        public bool ModifierPseudo(int idProf, string nouveauPseudo)
            => _repo.UpdatePseudo(idProf, nouveauPseudo);

        public bool AjouterPoints(int idProf, int points)
        {
            if (points <= 0) return false;
            return _repo.UpdatePoints(idProf, points);
        }

        public bool RetirerPoints(int idProf, int points)
        {
            if (points <= 0) return false;
            return _repo.UpdatePoints(idProf, -points);
        }

        public bool MettreAJourProfil(
            int idProf,
            string? pseudo = null,
            int? niveau = null,
            DateOnly? dateNaissance = null,
            int? themeId = null,
            int? avatarId = null)
        {
            return _repo.UpdateCore(idProf, pseudo, niveau, dateNaissance, themeId, avatarId);
        }

        public bool ChangerProfilActif(int idUtilisateur, int idProf)
            => _repo.SetProfilActifPourUtilisateur(idUtilisateur, idProf);

        public int? LireThemeActif(int idProf)
            => _repo.GetThemeActifId(idProf);

        public int? LireAvatarActif(int idProf)
            => _repo.GetAvatarActifId(idProf);
    }
}

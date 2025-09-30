using LearnPlay.Models;
using System;
using System.Collections.Generic;

namespace LearnPlay.Interfaces
{
    public interface IProfilsMt
    {
        Profils? GetProfil(int idProf);
        List<Profils> GetProfilsUtilisateur(int idUtilisateur);
        Profils CreerProfil(Profils nouveau);
        bool ModifierPseudo(int idProf, string nouveauPseudo);
        bool AjouterPoints(int idProf, int points);
        bool RetirerPoints(int idProf, int points);
        bool MettreAJourProfil(
            int idProf,
            string? pseudo = null,
            int? niveau = null,
            DateOnly? dateNaissance = null,
            int? themeId = null,
            int? avatarId = null
        );
        bool ChangerProfilActif(int idUtilisateur, int idProf);
        int? LireThemeActif(int idProf);
        int? LireAvatarActif(int idProf);
    }
}

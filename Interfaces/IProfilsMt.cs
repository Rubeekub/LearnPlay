using LearnPlay.Models;
using System;
using System.Collections.Generic;

namespace LearnPlay.Interfaces
{
    public interface IProfilsMt
    {
        Profils? AfficheProfil(int idProf);
        List<Profils> ListerUtilisateur(int idUtilisateur);
        Profils CreerProfil(Profils nouveau);
        bool ModifierPseudo(int idProf, string nouveauPseudo);
        bool AjouterPoints(int idProf, int points);
        bool RetirerPoints(int idProf, int points);
        bool MettreAJourCoeurProfil(
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

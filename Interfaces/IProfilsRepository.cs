using LearnPlay.Models;
using System;
using System.Collections.Generic;

namespace LearnPlay.Interfaces
{
    public interface IProfilsRepository
    {
        // Afficher un profil (numéro, pseudo, points, niveau, date de naissance)
        Profils? AfficheProfil(int idProf);

        // Lister les profils d’un utilisateur
        List<Profils> GetByUsersId(int idUti);

        // Créer un profil
        Profils Create(Profils p);

        // Mettre à jour le pseudo (avec contrôle d’unicité)
        bool UpdatePseudo(int idProf, string newPseudo);


        // Incrémenter/décrémenter les points (clamp à 0)
        bool UpdatePoints(int idProf, int delta);

        // Mise à jour “cœur” : pseudo/niveau/date de naissance (+ thème & avatar actifs)
 
        bool UpdateCore(
            int idProf,
            string? pseudo = null,
            int? niveau = null,
            DateOnly? dateNaissance = null,
            int? themeId = null,
            int? avatarId = null
        );

        // Changer de profil actif pour l’utilisateur (utilisateurs.IdProfActif) 
        bool SetProfilActifPourUtilisateur(int idUti, int idProf);

        // Obtenir l’ID du thème actif (ou null si aucun)
        int? GetThemeActifId(int idProf);

        // Obtenir l’ID de l’avatar actif (ou null si aucun)
        int? GetAvatarActifId(int idProf);
    }
}

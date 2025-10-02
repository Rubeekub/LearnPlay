using System;
using System.ComponentModel.DataAnnotations;

namespace LearnPlay.DTO
{
    // Création d’un profil
    // - le client choisit le pseudo, le niveau (optionnel), la date de naissance (optionnelle), et le rôle
    // - le client DOIT indiquer l’utilisateur cible (idUtiProf), sauf si tu le passes dans l’URL
    // - numProf et pointsProf sont calculés côté Métier; idProf est auto par la DB
    public class CreateProfilDto
    {
        [Required]
        public int IdUtiProf { get; set; }   // ou dans le route param, j hesite

        [Required, MaxLength(50)]
        public string Pseudo { get; set; } = string.Empty;

        
        public int? Niveau { get; set; } = default;


        public DateOnly? DateNaissance { get; set; }

        
        [Required]
        public int RoleId { get; set; }

        // (facultatif) si tu veux permettre de définir un thème/avatar actifs à la création
        public int? ThemeId { get; set; }
        public int? AvatarId { get; set; }

    }

    // Mise à jour "cœur" d’un profil
    // - tout est optionnel : seuls les champs non-null sont pris en compte
    public class UpdateProfilDto
    {
        [MaxLength(50)]
        public string? Pseudo { get; set; }

        public int? Niveau { get; set; }

        public DateOnly? DateNaissance { get; set; }

        // si renseigné, on change le rôle
        public int? RoleId { get; set; }

        // si > 0 : activer ce thème/avatar
        // si null : ne pas toucher
        // (évite d’envoyer 0 → sinon mappe 0 -> null côté contrôleur)
        public int? ThemeId { get; set; }
        public int? AvatarId { get; set; }
    }

    // Ajout / retrait de points (on garde la sémantique claire)
    public class AddPointsDto
    {
        [Range(1, int.MaxValue)]
        public int Points { get; set; }
    }

    public class RemovePointsDto
    {
        [Range(1, int.MaxValue)]
        public int Points { get; set; }
    }
}

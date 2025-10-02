using System;
using System.ComponentModel.DataAnnotations;

namespace LearnPlay.DTO
{
    // Création d'un utilisateur : on attend un hash de mot de passe (byte[])
    // En JSON, envoie la propriété "mdpUti" encodée en base64, le modèle .NET fera la conversion vers byte[].
    public class CreateUtilisateurDto
    {
        [MaxLength(50)]
        public string? NomUti { get; set; }

        [MaxLength(50)]
        public string? PrenomUti { get; set; }

        [Required, MaxLength(255), EmailAddress]
        public string MailUti { get; set; } = default!;

        [Required, MinLength(64), MaxLength(64)]
        public byte[] MdpUti { get; set; } = Array.Empty<byte>();
    }

    // Mise à jour identité : nom/prénom optionnels (null => pas de changement)
    public class UpdateIdentiteDto
    {
        [MaxLength(50)]
        public string? NomUti { get; set; }

        [MaxLength(50)]
        public string? PrenomUti { get; set; }
    }

    // Mise à jour email
    public class UpdateEmailDto
    {
        [Required, MaxLength(255), EmailAddress]
        public string MailUti { get; set; } = default!;
    }

    // Mise à jour mot de passe : hash (byte[])
    public class UpdatePasswordDto
    {
        [Required, MinLength(64), MaxLength(64)]
        public byte[] MdpUti { get; set; } = Array.Empty<byte>();
    }
}

using System;

namespace LearnPlay.DTO
{
    // Création d'un utilisateur : on attend un hash de mot de passe (byte[])
    // En JSON, envoie la propriété "mdpUti" encodée en base64, le modèle .NET fera la conversion vers byte[].
    public class CreateUtilisateurDto
    {
        public string NomUti { get; set; }
        public string PrenomUti { get; set; }
        public string MailUti { get; set; }
        public byte[] MdpUti { get; set; }
    }

    // Mise à jour identité : nom/prénom optionnels (null => pas de changement)
    public class UpdateIdentiteDto
    {
        public string NomUti { get; set; }
        public string PrenomUti { get; set; }
    }

    // Mise à jour email
    public class UpdateEmailDto
    {
        public string MailUti { get; set; }
    }

    // Mise à jour mot de passe : hash (byte[])
    public class UpdatePasswordDto
    {
        public byte[] MdpUti { get; set; }
    }
}

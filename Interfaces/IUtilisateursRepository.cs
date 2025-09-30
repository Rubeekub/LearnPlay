using LearnPlay.Models;
using System.Collections.Generic;

namespace LearnPlay.Interfaces
{
    public interface IUtilisateursRepository
    {
        Utilisateurs? GetById(int idUti);
        Utilisateurs? GetByEmail(string mailUti);
        List<Utilisateurs> GetAll();

        // Création : le hash est déjà calculé par la couche métier
        Utilisateurs Create(Utilisateurs u);

        // Mises à jour ciblées
        bool UpdateIdentite(int idUti, string? nom, string? prenom);
        bool UpdateEmail(int idUti, string newEmail);
        bool UpdatePasswordHash(int idUti, byte[] newHash);

        // Suppression
        bool Delete(int idUti);
    }
}

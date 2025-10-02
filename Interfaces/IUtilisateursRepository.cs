using LearnPlay.Models;
using System.Collections.Generic;

namespace LearnPlay.Interfaces
{
    public interface IUtilisateursRepository
    {
        Utilisateurs? Lire(int idUti);
        Utilisateurs? LireParEmail(string mailUti);
        List<Utilisateurs> Lister();

        // Création : le hash est déjà calculé par la couche métier
        Utilisateurs Enregistrer(Utilisateurs u);

        // Mises à jour ciblées
        bool ModifierIdentite(int idUti, string? nom, string? prenom);
        bool ModifierEmail(int idUti, string newEmail);
        bool ModifierMotDePasse(int idUti, byte[] newHash);

        // Suppression
        bool Supprimer(int idUti);
    }
}

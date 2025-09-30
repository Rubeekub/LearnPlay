using LearnPlay.Models;
using System.Collections.Generic;

namespace LearnPlay.Interfaces
{
    public interface IUtilisateursMt
    {
        Utilisateurs Lire(int idUti);
        Utilisateurs LireParEmail(string email);
        List<Utilisateurs> Lister();
        Utilisateurs Enregistrer(string nom, string prenom, string email, byte[] hashMdp);
        bool ModifierIdentite(int idUti, string nom, string prenom);
        bool ModifierEmail(int idUti, string email);
        bool ModifierMotDePasse(int idUti, byte[] hashMdp);
        bool Supprimer(int idUti);
    }
}

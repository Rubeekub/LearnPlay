using exoModelsProjet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Dtos
{
    public interface IUserRepository
    {
        Utilisateurs GetById(int idUti);
        List<Utilisateurs> GetAll();
        int Create(string nom, string prenom, string mail, byte[] mdpHash, DateTime dateInscription);
        bool UpdateEmail(int idUti, string newMail);
        bool UpdatePassword(int idUti, byte[] oldHash, byte[] newHash);
        int? GetActiveProfileId(int idUti);
        bool SetActiveProfile(int idUti, int idProf);       // vérifie l’appartenance
        bool RequestDeletion(int idUti);                    // placeholder (si tu ajoutes une SP)
    }

}

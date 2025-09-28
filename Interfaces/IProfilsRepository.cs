using exoModelsProjet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnPlay.Interfaces
{
    public interface IProfilsRepository
    {
        Profils GetById(int idProf);
        List<Profils> GetByUserId(int idUti);
        int Create(Profils p);                               // retourne l’ID créé
        bool UpdatePseudo(int idProf, string newPseudo);
        bool UpdatePoints(int idProf, int delta);           // + ou - ; protège contre négatif
    }
}

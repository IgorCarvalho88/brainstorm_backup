using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brainstorm.Models;
using Brainstorm.ViewModel;

namespace Brainstorm.Repository
{
    interface BrainstormRepository
    {
        DataRow GuardarReuniao(BrainstormViewModel model);
        DataRow GuardarTema(Tema tema, int id);

        ReuniaoBrainstorm GetReuniaoBrainstorm(int id);
        List<Tema> GetBrainstormTemas(int id);
    }
}

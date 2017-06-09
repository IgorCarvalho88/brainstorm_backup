using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Brainstorm.Models;

namespace Brainstorm.Repository
{
    public interface IIntervenientes
    {
        //IEnumerable<NaoConformidade> GetNaoConformidades();
        //DataTable GetUt();

        List<Interveniente> getUT();
        List<Interveniente> getBrainstormIntervenientes(int id);
    }
}
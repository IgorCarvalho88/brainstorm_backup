﻿using System;
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
        DataRow guardarReuniao(BrainstormViewModel model);
        DataRow guardarTema(Tema tema, int id);

        ReuniaoBrainstorm getReuniaoBrainstorm(int id);
        List<Tema> getBrainstormTemas(int id);
    }
}
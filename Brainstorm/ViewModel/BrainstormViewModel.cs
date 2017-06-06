using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Brainstorm.Models;

namespace Brainstorm.ViewModel
{
    public class BrainstormViewModel
    {
        public Models.ReuniaoBrainstorm ReuniaoBrainstorm { get; set; }
        public List<Interveniente> Intervenientes { get; set; }
        public List<Tema> Temas { get; set; }
    }
}
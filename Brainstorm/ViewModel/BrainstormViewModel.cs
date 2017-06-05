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
        // It was made a change to IEnumerable to be possible to show in views the list of intervenientes
        public List<Interveniente> Intervenientes { get; set; }
        public List<Tema> Temas { get; set; }
    }
}
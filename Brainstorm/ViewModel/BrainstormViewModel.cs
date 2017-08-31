using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Brainstorm.Models;
using ExpressiveAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Brainstorm.ViewModel
{
    public class BrainstormViewModel
    {
        public Models.ReuniaoBrainstorm ReuniaoBrainstorm { get; set; }
        public List<Interveniente> Intervenientes { get; set; }
        //[RequiredIf(@"ReuniaoBrainstorm.Estado == 'A'")]
        //[MinLength(1)]
        public List<Tema> Temas { get; set; }
        public List<Interveniente> IntervenientesSelecionados { get; set; }

        public List<WorkflowEstados> SequenciaEstados { get; set; }
        public List<Anexo> Anexos { get; set; }
        [Required]
        [MinLength(1)]
        public string[] IntervenientesView { get; set; }
    }
}
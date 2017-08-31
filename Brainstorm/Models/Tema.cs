using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressiveAnnotations.Attributes;

namespace Brainstorm.Models
{
    public class Tema
    {
        public int? Id { get; set; }
        //[RequiredIf(@"Descricao != null || Comentarios != null",
        //    ErrorMessage = "Por favor, preencha todos os campos necessários dos requisitos")]
        [RequiredIf(@"Descricao != null || Comentarios != null")]
        public string Titulo { get; set; }
        //[RequiredIf(@"Titulo != null || Comentarios != null",
        //    ErrorMessage = "Por favor, preencha todos os campos necessários dos requisitos")]
        [RequiredIf(@"Titulo != null || Comentarios != null")]
        public string Descricao { get; set; }
        //[RequiredIf(@"Descricao != null || Titulo != null || Importancia != null",
        //    ErrorMessage = "Por favor, preencha todos os campos necessários dos requisitos")]
        public string Comentarios { get; set; }

        //public string Decisao { get; set; }
        //[RequiredIf(@"Descricao != null || Comentarios != null || Titulo != null",
        //    ErrorMessage = "Por favor, preencha todos os campos necessários dos requisitos")]
        //public string Importancia { get; set; }
        public string Estado { get; set; }
        public bool GestaoInov { get; set; }
        public bool Actividade { get; set; }

        public bool SemEfeito { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brainstorm.Models
{
    public class Tema
    {
        public int? Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public string Comentarios { get; set; }

        //public string Decisao { get; set; }

        public string Importancia { get; set; }
        public string Estado { get; set; }
        public int? GestaoInov { get; set; }

        
    }
}
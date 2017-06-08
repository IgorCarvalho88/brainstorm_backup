using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace Brainstorm.Models
{
    public class ReuniaoBrainstorm
    {
        //[Display(Name = "Data")]
        [Required(ErrorMessage = "Data é obrigatório")]
        public string Data { get; set; }
        [Required(ErrorMessage = "Duração é obrigatório")]
        public int? Duracao { get; set; }
        [Required(ErrorMessage = "Duração real é obrigatório")]
        public int? DuracaoReal { get; set; }
        public string Estado { get; set; }
    }
}
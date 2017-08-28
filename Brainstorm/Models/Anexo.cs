using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brainstorm.Models
{
    public class Anexo
    {
        public long AnexoID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Tamanho { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Brainstorm.Models;
using Brainstorm.Repository;
using Brainstorm.Repository.Database;
using Brainstorm.ViewModel;
using Brainstorm = Brainstorm.Models.ReuniaoBrainstorm;

namespace Brainstorm.Controllers
{
    public class BrainstormController : Controller
    {
        IIntervenientes repo = new IntervenientesDB();

        // GET: Brainstorm
        public ActionResult Reuniao()
        {

            var reuniaoBrainstorm = new ReuniaoBrainstorm() {Duracao = 30};
            var intervenientes = new List<Interveniente>();
            intervenientes = repo.getUT();           

            var temas = new List<Tema>
            {
                new Tema {Descricao = "Inovacao", Importancia = "Alta", Comentarios = "teste", Decisao = "teste", Titulo = "titulo1", Estado = "P", GestaoInov = false},
                new Tema {Descricao = "Inovacao2", Importancia = "Alta", Comentarios = "teste2", Decisao = "teste2", Titulo = "titulo2", Estado = "P", GestaoInov = false }
            };

            var viewModel = new BrainstormViewModel
            {
                ReuniaoBrainstorm = reuniaoBrainstorm,
                Intervenientes = intervenientes,
                Temas = temas

            };

           // repo.GetUt();

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reuniao(BrainstormViewModel model)
        {
            BrainstormRepository brainRepo = new BrainstormDB();

            for (int i = 0; i < model.Intervenientes.Count; i++)
            {
                string s = model.Intervenientes[i].NomeAndCodigo;               
                if (!(String.IsNullOrWhiteSpace(s)))
                {
                    var split = s.Split(new[] {"  "}, StringSplitOptions.None);                    
                    split[0] = split[0].Substring(1, split[0].Length - 2);
                    model.Intervenientes[i].Nome = split[1];
                    model.Intervenientes[i].Codigo = split[0];
                }
            }

            model.ReuniaoBrainstorm.Estado = "";

            DataRow id = brainRepo.GuardarReuniao(model);
            int idBrainstorm = int.Parse(id[0].ToString());

            // guardar temas consoante o numero de temas presentes no model
            for (int i = 0; i < model.Temas.Count; i++)
            {
                DataRow teste = brainRepo.GuardarTema(model.Temas[i], idBrainstorm);
            }
           
            // guardar dados recebidos pelo utilizador aquando da criação da reuniao com os dados preenchidos na form////////////
            

           // return View();
            return RedirectToAction("Reuniao");
        }
    }
}
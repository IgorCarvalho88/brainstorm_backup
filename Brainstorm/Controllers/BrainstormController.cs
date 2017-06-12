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
using Microsoft.Owin.Security.Provider;
using Brainstorm = Brainstorm.Models.ReuniaoBrainstorm;

namespace Brainstorm.Controllers
{
    public class BrainstormController : Controller
    {
        IIntervenientes repo = new IntervenientesDB();

        // GET: Brainstorm
        public ActionResult Reuniao()
        {

            var reuniaoBrainstorm = new ReuniaoBrainstorm();
            var intervenientes = new List<Interveniente>();
            intervenientes = repo.getUT();           

            var temas = new List<Tema>
            {
                new Tema {Descricao = "Inovacao", Importancia = "Alta", Comentarios = "teste",  Titulo = "titulo1", Estado = "P", GestaoInov = 0},
                new Tema {Descricao = "Inovacao2", Importancia = "Alta", Comentarios = "teste2",  Titulo = "titulo2", Estado = "P", GestaoInov = 0 }
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
          
            // valida campos
            if (!ModelState.IsValid)
            {
                IIntervenientes repo = new IntervenientesDB();
                var intervenientes = new List<Interveniente>();
                intervenientes = repo.getUT();
                model.Intervenientes = intervenientes;
                return View("Reuniao", model);
            }

            BrainstormRepository brainRepo = new BrainstormDB();

            // inicializa estado para pendente
            model.ReuniaoBrainstorm.Estado = "P";
            // split do codigo e nome
            for (int i = 0; i < model.Intervenientes.Count; i++)
            {
                string s = model.Intervenientes[i].NomeAndCodigo;               
                if (!(String.IsNullOrWhiteSpace(s)))
                {
                    var split = s.Split(new[] {"   "}, StringSplitOptions.None);                    
                    split[0] = split[0].Substring(1, split[0].Length - 2);
                    model.Intervenientes[i].Nome = split[1];
                    model.Intervenientes[i].Codigo = split[0];
                }
            }

            // guardar dados recebidos pelo utilizador aquando da criação da reuniao com os dados preenchidos na form////////////

            DataRow id = brainRepo.guardarReuniao(model);
            int idBrainstorm = int.Parse(id[0].ToString());

            // guardar temas consoante o numero de temas presentes no model
            for (int i = 0; i < model.Temas.Count; i++)
            {
                DataRow teste = brainRepo.guardarTema(model.Temas[i], idBrainstorm);
            }

           


            // return View();
            TempData["additionalData"] = "Reunião criada com sucesso";
            //return RedirectToAction("Reuniao");
            return RedirectToAction("EditarReuniao", new {id = idBrainstorm});
        }

        public ActionResult EditarReuniao(int id)
        {
            BrainstormRepository brainRepo = new BrainstormDB();
            IIntervenientes repo = new IntervenientesDB();

            var reuniaoBrainstorm = new ReuniaoBrainstorm();
            var intervenientesSelecionados = new List<Interveniente>();
            var intervenientes = new List<Interveniente>();
            var temas = new List<Tema>();
            //var viewModel = new BrainstormViewModel();

            reuniaoBrainstorm = brainRepo.getReuniaoBrainstorm(id);
            temas = brainRepo.getBrainstormTemas(id);
            intervenientesSelecionados = repo.getBrainstormIntervenientes(id);
            intervenientes = repo.getUT();
            //getTemasbyID
            //getReuniaobyID
            //getIntervenientesID

            // carregar da base de dados a reuniao criada ?
            //if model.
            //getReuniaobyID

            var viewModel = new BrainstormViewModel
            {
                ReuniaoBrainstorm = reuniaoBrainstorm,
                Intervenientes = intervenientes,
                Temas = temas,
                IntervenientesSelecionados = intervenientesSelecionados

            };

            return View("Reuniao", viewModel);
        }
    }
}
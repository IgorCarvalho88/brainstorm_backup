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
            //var temas = new List<Tema>();
            intervenientes = repo.getUT();

            //var temas = new List<Tema>
            //{
            //    new Tema {Descricao = "Inovacao", Importancia = "Alta", Comentarios = "teste",  Titulo = "titulo1", Estado = "P", GestaoInov = 0},
            //    new Tema {Descricao = "Inovacao2", Importancia = "Alta", Comentarios = "teste2",  Titulo = "titulo2", Estado = "P", GestaoInov = 0 }
            //};

             List<Tema> temas = new List<Tema>(new Tema[10]);

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
                    
            // ModelState.Remove(nameof(model.Temas.Id));
            // valida campos
            if (!ModelState.IsValid)
            {
                IIntervenientes repo = new IntervenientesDB();
                var intervenientes = new List<Interveniente>();
                intervenientes = repo.getUT();
                model.Intervenientes = intervenientes;

                if (model.ReuniaoBrainstorm.Estado == "Pendente")
                {
                    model.ReuniaoBrainstorm.Estado = "P";
                }
                else if (model.ReuniaoBrainstorm.Estado == "Aprovado")
                {
                    model.ReuniaoBrainstorm.Estado = "A";
                }
                else if (model.ReuniaoBrainstorm.Estado == "Encerrado")
                {
                    model.ReuniaoBrainstorm.Estado = "E";
                }

                else if (model.ReuniaoBrainstorm.Estado == "Anulada")
                {
                    model.ReuniaoBrainstorm.Estado = "X";
                }


                return View("Reuniao", model);
            }

            // nova reuniao
            if (model.ReuniaoBrainstorm.Id == 0)
            {
                BrainstormRepository brainRepo = new BrainstormDB();

                // inicializa estado para pendente
                model.ReuniaoBrainstorm.Estado = "P";
                // split do codigo e nome
                model.Intervenientes = tratarInterv2(model.IntervenientesView);

                // guardar dados recebidos pelo utilizador aquando da criação da reuniao com os dados preenchidos na form////////////

                DataRow id = brainRepo.guardarReuniao(model);
                int idBrainstorm = int.Parse(id[0].ToString());

                // guardar intervenientes consoante o numero de intervenientes presentes no model
                for (int i = 0; i < model.Intervenientes.Count; i++)
                {
                    DataRow idIntervenienteAux = brainRepo.guardarInterveniente(model.Intervenientes[i], idBrainstorm);
                    int idInterveniente = int.Parse(idIntervenienteAux[0].ToString());
                   
                }

                // guardar temas consoante o numero de temas presentes no model
                for (int i = 0; i < model.Temas.Count; i++)
                {
                    // estado pendente como default-- mudar a logica depois de falar com rui
                    model.Temas[i].Estado = "P";
                    DataRow idTemaAux = brainRepo.guardarTema(model.Temas[i], idBrainstorm);
                    int idTema = int.Parse(idTemaAux[0].ToString());
                    model.Temas[i].Id = idTema;
                }

                // guardar estado da reuniao (falta guardar utlizador que modifica estado, usar sessions?)
                // ver hora em que é alterado o estado
                DataRow estadoSeq = brainRepo.guardarEstado(model.ReuniaoBrainstorm.Estado, idBrainstorm);

                TempData["additionalData"] = "Reunião criada com sucesso";
                //return RedirectToAction("Reuniao");
                return RedirectToAction("EditarReuniao", new {id = idBrainstorm});
            }
            else
            {
                // update reuniao
               
                BrainstormRepository brainRepo = new BrainstormDB();
                IIntervenientes repo = new IntervenientesDB();
               

                if (model.ReuniaoBrainstorm.Estado == "X")
                {
                     model.Intervenientes = tratarInterv2(model.IntervenientesView);

                    brainRepo.alterarReuniao(model, model.ReuniaoBrainstorm.Id);

                    repo.deleteIntervenientes(model.ReuniaoBrainstorm.Id);
                    for (int i = 0; i < model.Intervenientes.Count; i++)
                    {
                        DataRow idIntervenienteAux = brainRepo.guardarInterveniente(model.Intervenientes[i], model.ReuniaoBrainstorm.Id);
                        int idInterveniente = int.Parse(idIntervenienteAux[0].ToString());

                    }

                    for (int i = 0; i < model.Temas.Count; i++)
                    {
                        brainRepo.alterarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                    }

                    brainRepo.guardarEstado(model.ReuniaoBrainstorm.Estado, model.ReuniaoBrainstorm.Id);

                    return View("ReuniaoAnulada", model);

                }


                model.Intervenientes = tratarInterv2(model.IntervenientesView);

                DataRow teste = brainRepo.alterarReuniao(model, model.ReuniaoBrainstorm.Id);
                // adicionar funcao de fazer delete aos intervenientes associados ao id
                repo.deleteIntervenientes(model.ReuniaoBrainstorm.Id);
                for (int i = 0; i < model.Intervenientes.Count; i++)
                {
                    DataRow idIntervenienteAux = brainRepo.guardarInterveniente(model.Intervenientes[i], model.ReuniaoBrainstorm.Id);
                    int idInterveniente = int.Parse(idIntervenienteAux[0].ToString());

                }

                for (int i = 0; i < model.Temas.Count; i++)
                {
                    DataRow teste2 = brainRepo.alterarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                }

                // se escolhi estado actualizo o estado
                if (model.ReuniaoBrainstorm.EstadoFlag)
                {
                   
                        brainRepo.guardarEstado(model.ReuniaoBrainstorm.Estado, model.ReuniaoBrainstorm.Id);
                }

                return RedirectToAction("EditarReuniao", new { model.ReuniaoBrainstorm.Id });
            }

            return View();
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
            reuniaoBrainstorm.Id = id;
            temas = brainRepo.getBrainstormTemas(id);
            intervenientesSelecionados = repo.getBrainstormIntervenientes(id);
            intervenientes = repo.getUT();           

            var viewModel = new BrainstormViewModel
            {
                ReuniaoBrainstorm = reuniaoBrainstorm,
                Intervenientes = intervenientes,
                Temas = temas,
                IntervenientesSelecionados = intervenientesSelecionados

            };

            // Se a reuniao estiver anulada redireciona para a view reuniaoAnulada
            if (reuniaoBrainstorm.Estado == "X")
            {
                return View("ReuniaoAnulada", viewModel);
            }
            return View("Reuniao", viewModel);
        }

        public ActionResult GestaoBrainstorm()
        {
            return View();
        }


        public List<Interveniente> tratarInterv(List<Interveniente> intervs)
        {

            for (int i = 0; i < intervs.Count; i++)
            {
                string s = intervs[i].NomeAndCodigo;
                if (!(String.IsNullOrWhiteSpace(s)))
                {
                    var split = s.Split(new[] { "   " }, StringSplitOptions.None);
                    split[0] = split[0].Substring(1, split[0].Length - 2);
                    intervs[i].Nome = split[1];
                    intervs[i].Codigo = split[0];
                }
            }
            return intervs;
        }

        public List<Interveniente> tratarInterv2(string[] intervsAux)
        {
            var intervs = new List<Interveniente>();
            for (int i = 0; i < intervsAux.Length; i++)
            {
                string s = intervsAux[i];
                Interveniente interveniente = new Interveniente();
                if (!(String.IsNullOrWhiteSpace(s)))
                {
                    var split = s.Split(new[] { "   " }, StringSplitOptions.None);
                    split[0] = split[0].Substring(1, split[0].Length - 2);
                    interveniente.Nome = split[1];
                    interveniente.Codigo = split[0];
                }

                intervs.Add(interveniente);
            }
            return intervs;
        }
    }
}
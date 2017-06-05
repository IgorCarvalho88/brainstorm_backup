using System;
using System.Collections.Generic;
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

            var reuniaoBrainstorm = new ReuniaoBrainstorm() {Data = "21-05-2017", Duracao = 30};
            var intervenientes = new List<Interveniente>();
            intervenientes = repo.getUT();
            /*var intervenientes = new List<Interveniente>
            {
                new Interveniente {Name = "Igor", Codigo = "IGC"},
                new Interveniente {Name = "Teste", Codigo = "T"},
                new Interveniente {Name = "Teste2", Codigo = "T2"}
            };*/

            //Informaçao que vem da base de dados   
            /* var options = new Dictionary<string, string>();        
             options.Add("codigo_utilizador", "igor");
             options.Add("descricao", "igor2");
             ViewBag.users = new SelectList(options, "Key", "Value"); ;*/

            var temas = new List<Tema>
            {
                new Tema {Descricao = "Inovacao", Importancia = "Alta", Comentarios = "teste", Decisao = "teste", Titulo = "titulo1" },
                new Tema {Descricao = "Inovacao2", Importancia = "Alta", Comentarios = "teste2", Decisao = "teste2", Titulo = "titulo2" }
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

            // guardar dados recebidos pelo utilizador aquando da criação da reuniao com os dados preenchidos na form////////////


            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //var result = await UserManager.CreateAsync(user, model.Password);


            

            //var viewModel = new BrainstormViewModel
            //{
            //    ReuniaoBrainstorm = "",
            //    Intervenientes = "",
            //    Temas = temas

            //};

            return View();
        }
    }
}
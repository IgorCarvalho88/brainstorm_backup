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
using Brainstorm.Traducoes;
using Microsoft.Owin.Security.Provider;
using Brainstorm = Brainstorm.Models.ReuniaoBrainstorm;
using System.IO;
using System.Text;
using Brainstorm.Ideia;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using System.Data.SqlClient;

namespace Brainstorm.Controllers
{
    public class BrainstormController : Controller
    {
        IIntervenientes repo = new IntervenientesDB();

        // GET: Brainstorm
        public ActionResult Reuniao()
        {

            //if (Session["utilizador_codigo"] != null)
            //{
            //    var ut = Session["utilizador_codigo"].ToString();
            //}

            Sessoes();

            var reuniaoBrainstorm = new ReuniaoBrainstorm();
            var intervenientes = new List<Interveniente>();

            //var ut = Session["utilizador_codigo"].ToString();
            //reuniaoBrainstorm.Utilizador_alt = ut;
            //var temas = new List<Tema>();
            intervenientes = repo.getUT();          

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
            Sessoes();
            if(model.Temas[0].Id == null && model.ReuniaoBrainstorm.Estado == "A")
            {
                //IIntervenientes repo = new IntervenientesDB();
                var intervenientes = new List<Interveniente>();
                intervenientes = repo.getUT();
                model.Intervenientes = intervenientes;

                if(model.ReuniaoBrainstorm.Estado == "A")
                    model.ReuniaoBrainstorm.Estado = "P";
                //TempData["Erro"] = "Erro";


                TempData["EmptyTema"] = "EmptyTema";
               

                return View("Reuniao", model);
            }
            // valida campos
            if (!ModelState.IsValid)
                    {
                        //IIntervenientes repo = new IntervenientesDB();
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
                        if (model.ReuniaoBrainstorm.Estado == "A")
                           model.ReuniaoBrainstorm.Estado = "P";

                        if (model.ReuniaoBrainstorm.Estado == "E")
                            model.ReuniaoBrainstorm.Estado = "A";

                        TempData["Erro"] = "Erro";

                        //if(model.Temas == null)
                        //{
                        //    TempData["EmptyTema"] = "EmptyTema";
                        //}

                        return View("Reuniao", model);
                    }
          
            // nova reuniao
            if (model.ReuniaoBrainstorm.Id == 0)
            {
                BrainstormRepository brainRepo = new BrainstormDB();
                var ut = Session["utilizador_codigo"].ToString();

                // inicializa estado para pendente
                model.ReuniaoBrainstorm.Estado = "P";
                //inicializa utilizador que ins/alt e data de ins/alt.
                model.ReuniaoBrainstorm.Utilizador_ins = ut;
                model.ReuniaoBrainstorm.Data_ins = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                model.ReuniaoBrainstorm.Utilizador_alt = ut;
                model.ReuniaoBrainstorm.Data_alt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
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
                    if (model.Temas[i].Comentarios != null || model.Temas[i].Descricao != null || model.Temas[i].Titulo != null /*|| model.Temas[i].Comentarios != null*/)
                    {
                        model.Temas[i].Estado = "P";
                        DataRow idTemaAux = brainRepo.guardarTema(model.Temas[i], idBrainstorm);
                        int idTema = int.Parse(idTemaAux[0].ToString());
                        model.Temas[i].Id = idTema;
                    }
                }

                // guardar estado da reuniao (falta guardar utlizador que modifica estado, usar sessions?)
                // ver hora em que é alterado o estado
                string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                DataRow estadoSeq = brainRepo.guardarEstado(model.ReuniaoBrainstorm.Estado, data, idBrainstorm, ut);

                TempData["additionalData"] = "Reunião criada com sucesso";
                //return RedirectToAction("Reuniao");
                return RedirectToAction("EditarReuniao", new {id = idBrainstorm});
            }
            else
            {
                // update reuniao
                 BrainstormRepository brainRepo = new BrainstormDB();
                IIntervenientes repo = new IntervenientesDB();
                var ut = Session["utilizador_codigo"].ToString();
                             
               

                //if (model.ReuniaoBrainstorm.Estado == "E")
                //{
                //    model.Intervenientes = tratarInterv2(model.IntervenientesView);

                //    brainRepo.alterarReuniao(model, model.ReuniaoBrainstorm.Id);

                //    repo.deleteIntervenientes(model.ReuniaoBrainstorm.Id);
                //    for (int i = 0; i < model.Intervenientes.Count; i++)
                //    {
                //        DataRow idIntervenienteAux = brainRepo.guardarInterveniente(model.Intervenientes[i], model.ReuniaoBrainstorm.Id);
                //        int idInterveniente = int.Parse(idIntervenienteAux[0].ToString());

                //    }

                //    // guardar temas consoante o numero de temas presentes no model
                //    for (int i = 0; i < model.Temas.Count; i++)
                //    {
                //        // se existe tema onde, comentarios, descricao =! null etc faço update
                //        if (model.Temas[i].Id != null)
                //        {
                //            if (model.Temas[i].Comentarios != null || model.Temas[i].Descricao != null || model.Temas[i].Titulo != null /*|| model.Temas[i].Comentarios != null*/)
                //            {
                //                model.Temas[i].Estado = "P";
                //                //DataRow idTemaAux = brainRepo.guardarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                //                //int idTema = int.Parse(idTemaAux[0].ToString());
                //                //model.Temas[i].Id = idTema;
                //                brainRepo.alterarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);

                //                //if (model.Temas[i].Actividade)
                //                //{
                //                //    brainRepo.gerarActividade(model.Temas[i],tabelaID,ut);
                //                //}
                //            }
                //        }
                //        // se novo tema inserir na base de dados
                //        else
                //        {
                //            if (model.Temas[i].Comentarios != null || model.Temas[i].Descricao != null || model.Temas[i].Titulo != null /*|| model.Temas[i].Comentarios != null*/)
                //            {
                //                DataRow idTemaAux = brainRepo.guardarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                //                int idTema = int.Parse(idTemaAux[0].ToString());
                //                model.Temas[i].Id = idTema;
                //            }
                //        }
                //    }
                //    string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //    brainRepo.guardarEstado(model.ReuniaoBrainstorm.Estado, data, model.ReuniaoBrainstorm.Id, ut);

                //    return View("ReuniaoAnulada", model);

                //}


                model.Intervenientes = tratarInterv2(model.IntervenientesView);

                //model.ReuniaoBrainstorm.Utilizador_ins = "IGC";
                //model.ReuniaoBrainstorm.Data_ins = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                /*** adicionar quando anulada? bloco em cima***/
                model.ReuniaoBrainstorm.Utilizador_alt = ut;
                model.ReuniaoBrainstorm.Data_alt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                // adicionar funcao de fazer delete aos intervenientes associados ao id
                repo.deleteIntervenientes(model.ReuniaoBrainstorm.Id);
                for (int i = 0; i < model.Intervenientes.Count; i++)
                {
                    DataRow idIntervenienteAux = brainRepo.guardarInterveniente(model.Intervenientes[i], model.ReuniaoBrainstorm.Id);
                    int idInterveniente = int.Parse(idIntervenienteAux[0].ToString());

                }

                //for (int i = 0; i < model.Temas.Count; i++)
                //{

                //    if(model.Temas[i].Id != null)
                //    {
                //        brainRepo.deleteTemas(model.ReuniaoBrainstorm.Id);
                //    }
                    
                //}

                // id da tabela auxiliar aglutinaçao de custos
               // string tabelaID = brainRepo.getTabelaId();

                // guardar temas consoante o numero de temas presentes no model
                for (int i = 0; i < model.Temas.Count; i++)
                {
                    // se existe tema onde, comentarios, descricao =! null etc faço update
                    if (model.Temas[i].Id != null)
                    {
                        if (model.Temas[i].Comentarios != null || model.Temas[i].Descricao != null || model.Temas[i].Titulo != null /*|| model.Temas[i].Comentarios != null*/)
                        {
                            model.Temas[i].Estado = "P";
                            //DataRow idTemaAux = brainRepo.guardarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                            //int idTema = int.Parse(idTemaAux[0].ToString());
                            //model.Temas[i].Id = idTema;
                            brainRepo.alterarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);

                            //if (model.Temas[i].Actividade)
                            //{
                            //    brainRepo.gerarActividade(model.Temas[i],tabelaID,ut);
                            //}
                        }
                    }
                    // se novo tema inserir na base de dados
                    else
                    {
                        if (model.Temas[i].Comentarios != null || model.Temas[i].Descricao != null || model.Temas[i].Titulo != null /*|| model.Temas[i].Comentarios != null*/)
                        {
                            DataRow idTemaAux = brainRepo.guardarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                            int idTema = int.Parse(idTemaAux[0].ToString());
                            model.Temas[i].Id = idTema;
                        }
                    }

                    
                }

                //for (int i = 0; i < model.Temas.Count; i++)
                //{
                //    DataRow teste2 = brainRepo.alterarTema(model.Temas[i], model.ReuniaoBrainstorm.Id);
                //}

                // se escolhi estado actualizo o estado
                if (model.ReuniaoBrainstorm.EstadoFlag)
                {
                    string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    brainRepo.guardarEstado(model.ReuniaoBrainstorm.Estado, data, model.ReuniaoBrainstorm.Id, ut);
                    // Se estado encerrado e checkbox(actividade) entao gera actividade ou checkbox(gestInov)
                    if (model.ReuniaoBrainstorm.Estado == "E")
                    {
                        var intervenientesSelecionados = new List<Interveniente>();
                        intervenientesSelecionados = repo.getBrainstormIntervenientes(model.ReuniaoBrainstorm.Id);
                        // id da tabela auxiliar aglutinaçao de custos
                        string tabelaID = brainRepo.getTabelaId();
                        string moduloID = brainRepo.getModulo();
                        string componenteID = brainRepo.getComponente();
                        int idReuniao = model.ReuniaoBrainstorm.Id;
                        for (int i = 0; i < model.Temas.Count; i++)
                        {
                            if (model.Temas[i].Actividade)
                            {
                                brainRepo.gerarActividade(model.Temas[i], tabelaID, ut, moduloID, componenteID, idReuniao);
                            }
                        }


                       // Se checkbox gestInov entao gera ideia
                        for (int i = 0; i < model.Temas.Count; i++)
                        {
                            if (model.Temas[i].GestaoInov)
                            {
                                criarIdeias(model.ReuniaoBrainstorm, model.Temas[i], intervenientesSelecionados);
                            }
                        }
                    }

                }

               
                DataRow teste = brainRepo.alterarReuniao(model, model.ReuniaoBrainstorm.Id);

                return RedirectToAction("EditarReuniao", new { model.ReuniaoBrainstorm.Id });
            }

            return View();
        }

       

        public ActionResult EditarReuniao(int id)
        {
            Sessoes();
            BrainstormRepository brainRepo = new BrainstormDB();
            IIntervenientes repo = new IntervenientesDB();

            var reuniaoBrainstorm = new ReuniaoBrainstorm();
            var intervenientesSelecionados = new List<Interveniente>();
            var intervenientes = new List<Interveniente>();
            var temas = new List<Tema>();
            //List<Tema> temasAux = new List<Tema>(new Tema[10]);

            string workflow;
            //var viewModel = new BrainstormViewModel();

            reuniaoBrainstorm = brainRepo.getReuniaoBrainstorm(id);
            reuniaoBrainstorm.Id = id;

            // temas
            temas = brainRepo.getBrainstormTemas(id);
            // se temas nao é null, pois da primeira vez que e registada uma reuniao, poderá nao haver temas
            if(temas != null)
            {
                // adicionar sem efeito se actividade ou ideia estao a 0
                for (int i = 0; i < temas.Count; i++)
                {
                    if (temas[i].GestaoInov == false && temas[i].Actividade == false)
                    {
                        temas[i].SemEfeito = true;
                    }
                }

                int tamanhoLista = 10;
                tamanhoLista = tamanhoLista - temas.Count;
                List<Tema> temasAux = new List<Tema>(new Tema[tamanhoLista]);
                temas = temas.Concat(temasAux).ToList();
            }

            else
            {
                temas = new List<Tema>(new Tema[10]);
            }
           
           

           

            //intervenientes
            intervenientesSelecionados = repo.getBrainstormIntervenientes(id);
            intervenientes = repo.getUT();

            //anexos
            var anexos = new List<Anexo>();
            anexos = brainRepo.getBrainstormAnexos(id);

            // tratar workflow
            workflow = brainRepo.getWorkflow(reuniaoBrainstorm.Id);          
            var workflows = new List<WorkflowEstados>();
            workflows = tratarWorkflow(workflow);

            var viewModel = new BrainstormViewModel
            {
                ReuniaoBrainstorm = reuniaoBrainstorm,
                Intervenientes = intervenientes,
                Temas = temas,
                IntervenientesSelecionados = intervenientesSelecionados,
                SequenciaEstados = workflows,
                Anexos = anexos

            };
            // Se a reuniao estiver anulada redireciona para a view reuniaoAnulada
            if (reuniaoBrainstorm.Estado == "X" || reuniaoBrainstorm.Estado == "E")
            {
                return View("ReuniaoAnulada", viewModel);
            }
            return View("Reuniao", viewModel);
        }
        // ACABAR esta action result
        public ActionResult EditarReuniaoNotificacao(int id, int chat, string utilizador)
        {
            Sessoes();
            BrainstormRepository brainRepo = new BrainstormDB();
            IIntervenientes repo = new IntervenientesDB();
            //Session["utilizador_codigo"] = utilizador;
            AspToAspxSessionUser(utilizador);
            ViewBag.Chat = chat;

            var reuniaoBrainstorm = new ReuniaoBrainstorm();
            var intervenientesSelecionados = new List<Interveniente>();
            var intervenientes = new List<Interveniente>();
            var temas = new List<Tema>();
            //List<Tema> temasAux = new List<Tema>(new Tema[10]);

            string workflow;
            //var viewModel = new BrainstormViewModel();

            reuniaoBrainstorm = brainRepo.getReuniaoBrainstorm(id);
            reuniaoBrainstorm.Id = id;

            // temas
            temas = brainRepo.getBrainstormTemas(id);
            // se temas nao é null, pois da primeira vez que e registada uma reuniao, poderá nao haver temas
            if (temas != null)
            {
                // adicionar sem efeito se actividade ou ideia estao a 0
                for (int i = 0; i < temas.Count; i++)
                {
                    if (temas[i].GestaoInov == false && temas[i].Actividade == false)
                    {
                        temas[i].SemEfeito = true;
                    }
                }

                int tamanhoLista = 10;
                tamanhoLista = tamanhoLista - temas.Count;
                List<Tema> temasAux = new List<Tema>(new Tema[tamanhoLista]);
                temas = temas.Concat(temasAux).ToList();
            }

            else
            {
                temas = new List<Tema>(new Tema[10]);
            }

            //intervenientes
            intervenientesSelecionados = repo.getBrainstormIntervenientes(id);
            intervenientes = repo.getUT();

            //anexos
            var anexos = new List<Anexo>();
            anexos = brainRepo.getBrainstormAnexos(id);

            // tratar workflow
            workflow = brainRepo.getWorkflow(reuniaoBrainstorm.Id);
            var workflows = new List<WorkflowEstados>();
            workflows = tratarWorkflow(workflow);

            var viewModel = new BrainstormViewModel
            {
                ReuniaoBrainstorm = reuniaoBrainstorm,
                Intervenientes = intervenientes,
                Temas = temas,
                IntervenientesSelecionados = intervenientesSelecionados,
                SequenciaEstados = workflows,
                Anexos = anexos

            };
            // Se a reuniao estiver anulada redireciona para a view reuniaoAnulada
            if (reuniaoBrainstorm.Estado == "X" || reuniaoBrainstorm.Estado == "E")
            {
                return View("ReuniaoAnulada", viewModel);
            }
            return View("Reuniao", viewModel);
        }

        public ActionResult SaveUploadedFile(int id)
        {
          
            bool isSavedSuccessfully = true;
            BrainstormRepository brainRepo = new BrainstormDB();
            string fName = "";
            //int idAux = id;
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        // file.SaveAs(path);

                        string LocationFilePath = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + "WallImages\\";

                        string pathAux = LocationFilePath + file.FileName/*+ Path.GetExtension(file.FileName)*/;

                        //long length = new System.IO.FileInfo(file.FileName).Length;


                        //Response.Write(pathAux);
                        StreamWriter sw = new StreamWriter(new FileStream(pathAux, FileMode.CreateNew, FileAccess.Write), Encoding.GetEncoding("iso-8859-1"));
                        StreamReader reader = new StreamReader(file.InputStream, System.Text.Encoding.UTF8);
                        using (StreamReader r = new StreamReader(file.InputStream, Encoding.GetEncoding("iso-8859-1")))
                            sw.WriteLine(r.ReadToEnd());
                        sw.Close();

                        //long length = new System.IO.FileInfo(pathAux).Length;
                        string length = new System.IO.FileInfo(pathAux).Length.ToString();
                        brainRepo.guardarBrainstorm_anexo(id, pathAux, file.FileName, length);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        /***ANEXOS***/
        public ActionResult getFileByID(int id)
        {
            BrainstormRepository brainRepo = new BrainstormDB();
            Anexo anexo = new Anexo();
            anexo = brainRepo.getBrainstormAnexoByID(id);

            var extension = Path.GetExtension(anexo.Path);

            StreamReader r = new StreamReader(anexo.Path, Encoding.GetEncoding("iso-8859-1"));
            string content = r.ReadToEnd();

            MemoryStream ms = new MemoryStream();
            ms.Position = 0;

            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("iso-8859-1"));

            sw.WriteLine(content);
            sw.Close();

            byte[] bytesInStream = ms.ToArray(); // simpler way of converting to array
            ms.Close();

            Response.Clear();

            Response.ClearHeaders();

            Response.ClearContent();

            Response.AddHeader("Content-Disposition", "attachment; filename=" + anexo.FileName);

            Response.AddHeader("Content-Length", anexo.Tamanho);

            var ext = getMimeType(extension);

            Response.ContentType = ext;

            Response.ContentEncoding = System.Text.Encoding.UTF8;

            Response.BinaryWrite(bytesInStream);

            Response.Flush();

            Response.TransmitFile(anexo.FileName);

            Response.Close();

            Response.End();
            return View();

            
        }


        public ActionResult GetAnexos(int tipo)
        {
            //Get the images list from repository
            string LocationFilePath;
            // compara se rota edit ou rota da reuniao
            if (tipo == 1)
            {
                LocationFilePath = "..\\..\\Images\\" + "WallImages\\";
            }
            else
            {
                LocationFilePath = "..\\Images\\" + "WallImages\\";
            }
            //string LocationFilePath = "..\\..\\Images\\" + "WallImages\\";
            //string LocationFilePath = System.Web.HttpContext.Current.Server.MapPath("//Brainstorm//Images//WallImages//");
            //string LocationFilePath = System.Web.HttpContext.Current.Server.MapPath("//doccom//Gestão da Inovação//");

            //StreamReader r = new StreamReader(LocationFilePath + "2d83af42-def6-4d4d-bbe7-d9b4d8c1edce.png", Encoding.GetEncoding("iso-8859-1"));
            //string content = r.ReadToEnd();
            //r.Close();


            //string path = System.IO.Path.GetTempPath()+ "2d83af42-def6-4d4d-bbe7-d9b4d8c1edce.png";

            //MemoryStream ms = new MemoryStream();
            //ms.Position = 0;
            //StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.CreateNew, FileAccess.Write), Encoding.GetEncoding("iso-8859-1"));
            //sw.WriteLine(content);
            //sw.Close();

            //byte[] bytesInStream = ms.ToArray(); // simpler way of converting to array
            //ms.Close();

            var attachmentsList = new List<Anexo>
            {
                //new Anexos {AnexoID = 1, FileName = "/images/wallimages/2d83af42-def6-4d4d-bbe7-d9b4d8c1edce.png", Path = path}
                new Anexo {AnexoID = 1, FileName = "/images/wallimages/screen5", Path = LocationFilePath + "screen5.png"},
                //new Anexos {AnexoID = 1, FileName = "/images/wallimages/capture1.png", Path = LocationFilePath + "capture1.png"}
            }.ToList();

            return Json(new { Data = attachmentsList }, JsonRequestBehavior.AllowGet);
        }

        /*** GESTAO DE BRAINSTORM***/
        public ActionResult GestaoBrainstorm()
        {
            Sessoes();
            BrainstormRepository brainRepo = new BrainstormDB();


            var reunioes = new List<GestaoBrainstorm>();
            reunioes = brainRepo.getReunioesBrainstorm();

            var viewModel = new GestaoBrainstormViewModel()
            {
               Reunioes = reunioes

            };
            return View(viewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GestaoBrainstorm(int id)
        //{
        //    BrainstormRepository brainRepo = new BrainstormDB();

        //    return RedirectToAction("EditarReuniao", new { id});


            
        //}      
        public ActionResult GestaoEdit(int id)
        {
            Sessoes();
            BrainstormRepository brainRepo = new BrainstormDB();

            return RedirectToAction("EditarReuniao", new { id });


        }


        
        public ActionResult GestaoDelete(int id)
        {
            BrainstormRepository brainRepo = new BrainstormDB();
            brainRepo.eliminarBrainstorm(id);

            //var reunioes = new List<GestaoBrainstorm>();
            //reunioes = brainRepo.getReunioesBrainstorm();

            //var viewModel = new GestaoBrainstormViewModel()
            //{
            //    Reunioes = reunioes

            //};
            return null;
        }
        [HttpPost]
        public string TraduzTermo(string termo)
        {
            //string termoTraduzido;
            //termoTraduzido = Traduz(termo);
            return Traducoes.Traducoes.Traduz(termo); ;
        }

        [HttpPost]
        public ActionResult Chat(string mensagem, int idTema, int idReuniao)
        {
            BrainstormRepository brainRepo = new BrainstormDB();
           
            var ut = Session["utilizador_codigo"].ToString();
            var data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            brainRepo.guardarChatMensagens(mensagem, idTema, idReuniao, ut, data);

            return null;
        }

        [HttpPost]
        public ActionResult getChatMensagens(int idTema, int idReuniao)
        {
            var mensagens = new List<ChatMensagem>();
            BrainstormRepository brainRepo = new BrainstormDB();
            mensagens = brainRepo.getBrainstormChatMensagens(idTema, idReuniao);

            return Json(mensagens);
        }

        public async Task<ActionResult> VisualizarAnexos(int id)
        {
            BrainstormRepository brainRepo = new BrainstormDB();
            var anexos = new List<Anexo>();
            anexos = brainRepo.getBrainstormAnexos(id);

            var viewModel = new BrainstormViewModel
            {               
                Anexos = anexos
            };

            return PartialView("VisualizarAnexos", viewModel);
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


        /*** FUNÇÔES***/
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

        public List<WorkflowEstados> tratarWorkflow(string workflow)
        {
            //var intervs = new List<Interveniente>();
            Char delimiter = '|';
            Char delimiter2 = ';';
            var workflows = new List<WorkflowEstados>();
           

            String[] substrings = workflow.Split(delimiter);
            int i = 1;
            foreach (var substring in substrings)
            {
                // last element
                WorkflowEstados workflowEstado = new WorkflowEstados();
                int tamanho = substrings.Length;
                if (i!=tamanho)
                {
                    String[] subsubstrings = substring.Split(delimiter2);
                    Estado estado = (Estado)Enum.Parse(typeof(Estado), subsubstrings[0]);
                    workflowEstado.estado = estado;
                    workflowEstado.data = subsubstrings[1];
                    workflowEstado.UtilizadorDescritivo = subsubstrings[2];
                    workflows.Add(workflowEstado);

                }
               
                i++;
            }
               

            return workflows;
        }

        public ActionResult Sessoes()
        {
            if (Session["utilizador_codigo"] != null)
            {
                var ut = Session["utilizador_codigo"].ToString();
            }

            else
            {
                //return RedirectToAction("Reuniao");
                return HttpNotFound();
            }

            return null;
        }

        public static string getMimeType(string extension)
        {
            string ext = "application/octet-stream";
            switch (extension)
            {
                case ".pdf":
                    ext = "application/pdf";
                    break;
                case ".png":
                    ext = "image/png";
                    break;
                case ".jpg":
                    ext = "image/jpg";
                    break;
                case ".jpeg":
                    ext = "image/jpeg";
                    break;
                case ".gif":
                    ext = "image/gif";
                    break;
                case ".mp3":
                    ext = "audio/mpeg";
                    break;
                case ".m4a":
                    ext = "audio/mpeg";
                    break;
                case ".docx":
                    ext = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".doc":
                    ext = "application/msword";
                    break;
                case ".dotx":
                    ext = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;
                case ".ppt":
                    ext = "application/vnd.ms-powerpointtd>";
                    break;
                case ".ppsx":
                    ext = "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                    break;
                case ".potx":
                    ext = "application/vnd.openxmlformats-officedocument.presentationml.template";
                    break;
                case ".xls":
                    ext = "application/vnd.ms-excel";
                    break;
                case ".xlsb":
                    ext = "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                    break;
                case ".xlsx":
                    ext = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".xltx":
                    ext = "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                    break;
                case ".zip":
                    ext = "application/zip, application/x-compressed-zip";
                    break;
                case ".txt":
                    ext = "text/plain";
                    break;
                default:
                    ext = "application/octet-stream";
                    break;
            }
            return ext;
        }



        public static string criarIdeias(ReuniaoBrainstorm reuniao, Tema tema, List<Interveniente> intervenientes)
        {
            
            BrainstormRepository brainRepo = new BrainstormDB();
            var ut_codigo = System.Web.HttpContext.Current.Session["utilizador_codigo"].ToString();
            var ut_descritivo = System.Web.HttpContext.Current.Session["utilizador_descritivo"].ToString();
            //formato_data == "DDMMYYYY"
            //    @Session["FormatoData"].ToString()'
            //string dataFormato1 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            //string dataFormato2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string dataFormato = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (System.Web.HttpContext.Current.Session["FormatoData"].ToString() == "DDMMYYYY")
            {
                dataFormato = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else if(System.Web.HttpContext.Current.Session["FormatoData"].ToString() == "YYYYMMDD")
            {
                dataFormato = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }

           

            if (brainRepo.Verifica_GestInov_Configurado() == "1")
            {
                if (GestInovWebApiHttpClient.grantAccess() == true)
                {
                    using (var client = GestInovWebApiHttpClient.GetClient())
                    {
                        IdeiaViewModel ideia = new IdeiaViewModel();
                        ideia.UtilizadorID = ut_codigo;
                        ideia.UtilizadorDescritivo = ut_descritivo;
                        ideia.tipoIdeiaID = 27;// perguntar ao pedro
                        ideia.titulo = tema.Titulo;
                        ideia.descricao = tema.Descricao;
                        ideia.origemID = 92; //? perguntar ao pedro
                        ideia.acessoID = "1"; //?
                        //ideia.observacoes = id[7];
                        ideia.estado = EstadoIdeiaViewModel.Registada;
                        ideia.dataRegisto = Convert.ToDateTime(dataFormato);
                        ideia.autores = new List<AutorIdeiaViewModel>();

                        //var intervenientes = JsonConvert.DeserializeObject<List<string>>(id[15]);

                        for (var i = 1; i < intervenientes.Count; i++)
                        {
                            AutorIdeiaViewModel autor = new AutorIdeiaViewModel();
                            autor.userID = intervenientes[i].Codigo;//codigo
                            ideia.autores.Add(autor); //??
                        }

                        AutorIdeiaViewModel autorPrincipal = new AutorIdeiaViewModel();
                        autorPrincipal.userID = ut_codigo;
                        ideia.autores.Add(autorPrincipal);

                        //var autores = id[10];

                        HttpResponseMessage response = client.PostAsJsonAsync("api/Ideia", ideia).Result;

                        IdeiaViewModel ideiaResult = null;
                        if (response.IsSuccessStatusCode)
                        {
                            string contentResponse = response.Content.ReadAsStringAsync().Result;
                            ideiaResult = JsonConvert.DeserializeObject<IdeiaViewModel>(contentResponse);

                            //GestInovGuardarIdIdeia(ideiaResult.ID /*intervenientes*/); //??

                            WorkflowEstadosIdeiaViewModel wflow = new WorkflowEstadosIdeiaViewModel();
                            wflow.ideiaID = ideiaResult.ID;
                            wflow.data = ideiaResult.dataRegisto;
                            wflow.estado = ideiaResult.estado.ToString();
                            wflow.UtilizadorID = ut_codigo;//??
                            wflow.UtilizadorDescritivo = ut_descritivo;//??
                            HttpResponseMessage responseWF = client.PostAsJsonAsync("api/WorkflowEstadosIdeia", wflow).Result;

                            //GestInovCriarComentario(ut_codigo, ut_descritivo, tema.Comentarios, ideia.dataRegisto, ideiaResult.ID);
                            //GestInovCriarClassificacao(intervenientes, ideiaResult.ID, ideia.tipoIdeiaID, ideiaResult);
                           // GestInovCriarAvaliacaoIdeiaIndividual(ideiaResult.ID, ut_codigo, "18-07-2017"); // deadline harcoded perguntar ao pedro
                             //Create(AtividadeViewModel atividade)
                            //GestInovFinalizarAvaliacao();
                            //brainRepo.CriarActividade(reuniao, tema, ut_codigo);
                            //var infoAtividade = new string[5];
                            //infoAtividade[0] = id[11];
                            //infoAtividade[1] = id[12];
                            //infoAtividade[2] = id[13];
                            //infoAtividade[3] = id[14];
                            //infoAtividade[4] = ideiaResult.ID.ToString();

                            //brainRepo.UpdateActividadeIdeia(infoAtividade);
                        }

                        return "";
                    }
                }
            }
            return null;
        }

        private static void GestInovCriarComentario(string ut_codigo, string ut_descritivo, string comentario, DateTime data, Guid ideiaID)
        {
            using (var client = GestInovWebApiHttpClient.GetClient())
            {
                ComentarioViewModel comentariovm = new ComentarioViewModel();
                comentariovm.UtilizadorID = ut_codigo;
                comentariovm.AutorEmail = ut_descritivo;
                comentariovm.ComentarioTexto = comentario;
                comentariovm.Data = data;
                comentariovm.ideiaID = ideiaID;
                HttpResponseMessage response = client.PostAsJsonAsync("api/Comentario", comentariovm).Result;
            }
        }

        private static void GestInovCriarClassificacao(List<Interveniente> intervenientes, Guid ideiaID, int tipoIdeiaID, IdeiaViewModel ideia)
        {
            using (var client = GestInovWebApiHttpClient.GetClient())
            {
                TipoIdeiaViewModel tipoIdeia = null;
                HttpResponseMessage response = client.GetAsync("api/TipoIdeia/" + tipoIdeiaID).Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentResponse = response.Content.ReadAsStringAsync().Result;
                    tipoIdeia = JsonConvert.DeserializeObject<TipoIdeiaViewModel>(contentResponse);

                    for (var i = 0; i < intervenientes.Count; i++)
                    {
                        ClassificacaoIdeiaViewModel classificacao = new ClassificacaoIdeiaViewModel();
                        classificacao.ideiaID = ideiaID;
                        classificacao.utilizadorID = intervenientes[i].Codigo;
                       // DataTable dt = JsonConvert.DeserializeObject<DataTable>(AcessoBD.Controllers.RTPController.getDescritivoPorCodigo(intervenientes[i]));

                        var ut_descritivo = intervenientes[i].Nome;
                        classificacao.utilizadorEmail = ut_descritivo;
                        classificacao.classificacoesTopicos = new List<ClassificacaoTopicoViewModel>();

                        for (var j = 0; j < tipoIdeia.topicosAvaliacao.Count; j++)
                        {
                            ClassificacaoTopicoViewModel ct = new ClassificacaoTopicoViewModel();
                            ct.utilizadorID = intervenientes[i].Codigo;
                            ct.utilizadorEmail = ut_descritivo;
                            ct.topicoAvaliacaoID = tipoIdeia.topicosAvaliacao[j].ID;
                            ct.classificacao = tipoIdeia.topicosAvaliacao[j].valorDefeito;
                            classificacao.classificacoesTopicos.Add(ct);
                        }

                        HttpResponseMessage response4 = client.PostAsJsonAsync("api/ClassificacaoIdeia/", classificacao).Result;
                        if (response4.IsSuccessStatusCode && i == 0)
                        {
                            ideia.estado = EstadoIdeiaViewModel.Classificada;
                            HttpResponseMessage responseMudaEstadoIdeia = client.PutAsJsonAsync("api/Ideia/" + ideia.ID, ideia).Result;

                            WorkflowEstadosIdeiaViewModel wflow = new WorkflowEstadosIdeiaViewModel();
                            wflow.ideiaID = ideiaID;
                            wflow.data = DateTime.Now;
                            wflow.estado = EstadoIdeiaViewModel.Classificada.ToString();
                            wflow.UtilizadorID = intervenientes[i].Codigo;
                            wflow.UtilizadorDescritivo = ut_descritivo;
                            HttpResponseMessage responseWF = client.PostAsJsonAsync("api/WorkflowEstadosIdeia", wflow).Result;
                        }
                    }
                }
            }
        }

        private static void GestInovCriarAvaliacaoIdeiaIndividual(Guid ideiaID, string responsavel, string deadline)
        {
            using (var client = GestInovWebApiHttpClient.GetClient())
            {
                AvaliacaoIdeiaIndividual avIndividual = new AvaliacaoIdeiaIndividual();
                avIndividual.avaliacaoID = (int)System.Web.HttpContext.Current.Session["GestInovIdeiaID"];

                avIndividual.documentoDestino = TipoDocumentoDestinoViewModel.Atividade;
                avIndividual.ideiaID = ideiaID;
                avIndividual.parecer = "Aprovada para implementação";
                avIndividual.responsavel = responsavel;// perguntar ao pedro
                avIndividual.deadlineString = deadline;// perguntar ao pedro
                avIndividual.deadline = Convert.ToDateTime(deadline);
                avIndividual.novoEstado = EstadoIdeiaViewModel.Aprovada;
                avIndividual.isAvaliada = true;

                HttpResponseMessage response = client.PostAsJsonAsync("api/AvaliacaoIdeiaIndividual", avIndividual).Result;
            }
        }

        private static void GestInovGuardarIdIdeia(Guid id /*List<string> intervenientes*/)
        {
            List<Guid> ideiaIds = (List<Guid>)System.Web.HttpContext.Current.Session["GestInovIdeiaIds"];
            if (ideiaIds == null)
                ideiaIds = new List<Guid>();

            if (ideiaIds.Count == 0)
            {
                GestInovCriarAvaliacao();
            }

            ideiaIds.Add(id);
            System.Web.HttpContext.Current.Session["GestInovIdeiaIds"] = ideiaIds;
        }


        private static void GestInovCriarAvaliacao()
        {
            BrainstormRepository brainRepo = new BrainstormDB();
            using (var client = GestInovWebApiHttpClient.GetClient())
            {
                //??????
                List<string> intervenientes = new List<string>();

                DataTable dt = new DataTable();
                dt = brainRepo.Get_Eq_Agendamento();

                bool temdados = false;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        temdados = true;
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            intervenientes.Add(dt.Rows[i][0].ToString());
                        }
                    }
                }

                if (temdados)
                {
                    AvaliacaoIdeiaViewModel avaliacaoIdeia = new AvaliacaoIdeiaViewModel();
                    avaliacaoIdeia.participantes = new List<ParticipanteReuniaoIdeiaViewModel>();

                    for (var i = 1; i < intervenientes.Count; i++)
                    {
                        ParticipanteReuniaoIdeiaViewModel participante = new ParticipanteReuniaoIdeiaViewModel();
                        participante.userID = intervenientes[i];//??
                        avaliacaoIdeia.participantes.Add(participante);
                    }

                    ParticipanteReuniaoIdeiaViewModel participantePrincipal = new ParticipanteReuniaoIdeiaViewModel();
                    participantePrincipal.userID = intervenientes[0];
                    avaliacaoIdeia.participantes.Add(participantePrincipal);
                    avaliacaoIdeia.dataInicio = DateTime.Now;
                    avaliacaoIdeia.dataFim = DateTime.Now;
                    avaliacaoIdeia.estado = EstadoAvaliacaoIdeiaViewModel.Pendente;

                    AvaliacaoIdeiaViewModel responseAv = null;
                    HttpResponseMessage response = client.PostAsJsonAsync("api/AvaliacaoIdeia", avaliacaoIdeia).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = response.Content.ReadAsStringAsync().Result;
                        responseAv = JsonConvert.DeserializeObject<AvaliacaoIdeiaViewModel>(contentResponse);

                        System.Web.HttpContext.Current.Session["GestInovIdeiaID"] = responseAv.ID;
                    }
                }

            }
        }


        public static void GestInovFinalizarAvaliacao()
        {
            BrainstormRepository brainRepo = new BrainstormDB();
           // var ut_codigo = System.Web.HttpContext.Current.Session["utilizador_codigo"].ToString();
           // var ut_descritivo = System.Web.HttpContext.Current.Session["utilizador_descritivo"].ToString();

            using (var client = GestInovWebApiHttpClient.GetClient())
            {
                var avId = (int)System.Web.HttpContext.Current.Session["GestInovIdeiaID"];// id da avaliacao??

                AvaliacaoIdeiaViewModel avaliacaoIdeia = null;

                HttpResponseMessage response4 = client.GetAsync("api/AvaliacaoIdeia/" + avId).Result;
                if (response4.IsSuccessStatusCode)
                {
                    string contentResponse2 = response4.Content.ReadAsStringAsync().Result;
                    avaliacaoIdeia = JsonConvert.DeserializeObject<AvaliacaoIdeiaViewModel>(contentResponse2);

                    avaliacaoIdeia.dataFim = DateTime.Now;
                    avaliacaoIdeia.estado = EstadoAvaliacaoIdeiaViewModel.Finalizada;

                    HttpResponseMessage response = client.PutAsJsonAsync("api/AvaliacaoIdeia/" + avaliacaoIdeia.ID, avaliacaoIdeia).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (avaliacaoIdeia.ideiasAvaliadas != null)
                        {
                            for (int i = 0; i < avaliacaoIdeia.ideiasAvaliadas.Count; i++)
                            {
                                IdeiaViewModel idvm = null;

                                HttpResponseMessage response6 = client.GetAsync("api/Ideia/" + avaliacaoIdeia.ideiasAvaliadas[i].ideiaID).Result;

                                if (response6.IsSuccessStatusCode)
                                {
                                    string contentResponse = response6.Content.ReadAsStringAsync().Result;
                                    idvm = JsonConvert.DeserializeObject<IdeiaViewModel>(contentResponse);

                                    idvm.estado = avaliacaoIdeia.ideiasAvaliadas[i].novoEstado;

                                    HttpResponseMessage response3 = client.PutAsJsonAsync("api/Ideia/" + idvm.ID, idvm).Result;

                                    if (response3.IsSuccessStatusCode)
                                    {
                                        DataTable dt = new DataTable();
                                        dt = brainRepo.Get_Eq_Agendamento();

                                        bool temdados = false;
                                        if (dt != null)
                                        {
                                            if (dt.Rows.Count > 0)
                                            {
                                                temdados = true;

                                            }
                                        }
                                        if (temdados)
                                        {
                                            WorkflowEstadosIdeiaViewModel wflow = new WorkflowEstadosIdeiaViewModel();
                                            wflow.ideiaID = idvm.ID;
                                            wflow.data = DateTime.Now;
                                            wflow.estado = idvm.estado.ToString();
                                            wflow.avaliacaoID = avaliacaoIdeia.ideiasAvaliadas[i].ID;
                                            DataTable dtUt = brainRepo.getDescritivoPorCodigo(dt.Rows[0][0].ToString());

                                            wflow.UtilizadorID = dt.Rows[0][0].ToString(); //??
                                            wflow.UtilizadorDescritivo = dtUt.Rows[0][0].ToString(); //??
                                            HttpResponseMessage responseWF = client.PostAsJsonAsync("api/WorkflowEstadosIdeia", wflow).Result;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            System.Web.HttpContext.Current.Session["GestInovIdeiaID"] = null;
            System.Web.HttpContext.Current.Session["GestInovIdeiaIds"] = null;
        }

        public void AspToAspxSessionUser(string user)
        {
            string sSql = "";
            string strConnString = "";
            try
            {
                sSql = "select variavel, valor from asptoaspxsession where utilizador='" + user + "'";
            }
            catch
            {
                sSql = "select variavel, valor from asptoaspxsession";
            }

            try
            {
                strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            catch (Exception d)
            {
                Response.Write(d.Message);
            }

            try
            {
                SqlConnection con = new SqlConnection(strConnString);

                SqlCommand com = new SqlCommand(sSql, con);
                com.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader RDB = com.ExecuteReader();
                string ses = "";

                while (RDB.Read())
                {
                    //BFC 26-02-2013
                    System.Web.HttpContext.Current.Session[RDB.GetValue(0).ToString().ToLower()] = RDB.GetValue(1).ToString();
                    ses += RDB.GetValue(0).ToString().ToLower() + " = " + RDB.GetValue(1).ToString() + ";";
                }

                RDB.Close();
                con.Close();

              
            }
            catch (SqlException erro)
            {
                Response.Write(erro.Message);
            }


        }

        //for (int i = 0; i < intervsAux.Length; i++)
        //{
        //    string s = intervsAux[i];
        //    Interveniente interveniente = new Interveniente();
        //    if (!(String.IsNullOrWhiteSpace(s)))
        //    {
        //        var split = s.Split(new[] { "   " }, StringSplitOptions.None);
        //        split[0] = split[0].Substring(1, split[0].Length - 2);
        //        interveniente.Nome = split[1];
        //        interveniente.Codigo = split[0];
        //    }

        //    intervs.Add(interveniente);
        //}
        //    return null;
        //}
    }
}
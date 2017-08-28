using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using Brainstorm.Models;
using Brainstorm.ViewModel;

namespace Brainstorm.Repository.Database
{
    public class BrainstormDB : BrainstormRepository
    {
        public DataRow guardarReuniao(BrainstormViewModel model)
        {
         
            SqlConnection con = null;
         
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[guardarBrainstorm]", con);
                //(intervenientes valor como default ???????)
                sqlComm.Parameters.AddWithValue("@brainstorm_data", model.ReuniaoBrainstorm.Data);              
                sqlComm.Parameters.AddWithValue("@brainstorm_est_codigo", model.ReuniaoBrainstorm.Estado);
                sqlComm.Parameters.AddWithValue("@brainstorm_duracaoPrev", model.ReuniaoBrainstorm.Duracao);
                sqlComm.Parameters.AddWithValue("@brainstorm_duracaoReal", model.ReuniaoBrainstorm.DuracaoReal);
                sqlComm.Parameters.AddWithValue("@brainstorm_observacoes", ((object)model.ReuniaoBrainstorm.Observacoes) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_ut_ins", ((object)model.ReuniaoBrainstorm.Utilizador_ins) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_ut_alt", ((object)model.ReuniaoBrainstorm.Utilizador_alt) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_data_ins", ((object)model.ReuniaoBrainstorm.Data_ins) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_data_alt", ((object)model.ReuniaoBrainstorm.Data_alt) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv1_codigo", model.Intervenientes[0].Codigo);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv2_codigo",((object)model.Intervenientes[1].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv3_codigo", ((object)model.Intervenientes[2].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv4_codigo", ((object)model.Intervenientes[3].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv5_codigo", ((object)model.Intervenientes[4].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv1_descritivo", model.Intervenientes[0].Nome);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv2_descritivo", ((object)model.Intervenientes[1].Nome) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv3_descritivo", ((object)model.Intervenientes[2].Nome) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv4_descritivo", ((object)model.Intervenientes[3].Nome) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv5_descritivo", ((object)model.Intervenientes[4].Nome) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_local", ((object)model.ReuniaoBrainstorm.Local) ?? DBNull.Value);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

               


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    
                    return dt.Rows[0];
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }



        public DataRow guardarTema(Tema tema, int id)
        {
            // tratar model



            SqlConnection con = null;
            //day = new DateTime(2017, 3, 6);
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("[dbo].[guardarBrainstormTema]", con);
                
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_brainstorm_id", id);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_titulo", ((object)tema.Titulo)?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_descricao", ((object)tema.Descricao) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_tema_importancia", ((object)tema.Importancia) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_comentarios", ((object)tema.Comentarios) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_estado", ((object)tema.Estado) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_tema_estado", tema.Estado);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_gestInov", ((object)tema.GestaoInov) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_actividade", ((object)tema.Actividade) ?? DBNull.Value);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);




                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0];
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public DataRow guardarInterveniente(Interveniente interveniente, int idBrainstorm)
        {
            
            SqlConnection con = null;
            
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                
                SqlCommand sqlComm = new SqlCommand("[dbo].[guardarBrainstormInterveniente]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_interveniente_brainstorm_id", idBrainstorm);
                sqlComm.Parameters.AddWithValue("@brainstorm_interveniente_codigo", ((object)interveniente.Codigo) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_interveniente_descritivo", ((object)interveniente.Nome) ?? DBNull.Value);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);




                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0];
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }
        public ReuniaoBrainstorm getReuniaoBrainstorm(int id)
        {
            
            SqlConnection con = null;         
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
              
                SqlCommand sqlComm = new SqlCommand("[dbo].[getReuniaoBrainstorm]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_id", id);
               

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    ReuniaoBrainstorm reuniaoBrainstorm = new ReuniaoBrainstorm();
                    //dt.Rows[0]["brainstorm_data"].Convert(val => DateTime.Parse(val.ToString()).ToString("dd/MMM/yyyy"));
                    reuniaoBrainstorm.Data = ((DateTime)dt.Rows[0]["brainstorm_data"]).ToString("dd/MM/yyyy");
                    reuniaoBrainstorm.Estado = dt.Rows[0]["brainstorm_est_codigo"].ToString();
                    reuniaoBrainstorm.Duracao = Convert.ToInt32(dt.Rows[0]["brainstorm_duracaoPrev"].ToString());
                    reuniaoBrainstorm.DuracaoReal = Convert.ToInt32(dt.Rows[0]["brainstorm_duracaoReal"].ToString());
                    reuniaoBrainstorm.Observacoes = dt.Rows[0]["brainstorm_observacoes"].ToString();
                    reuniaoBrainstorm.Local = dt.Rows[0]["brainstorm_local"].ToString();
                    reuniaoBrainstorm.Utilizador_ins = dt.Rows[0]["brainstorm_ut_ins"].ToString();
                    reuniaoBrainstorm.Utilizador_alt = dt.Rows[0]["brainstorm_ut_alt"].ToString();
                    reuniaoBrainstorm.Data_ins = ((DateTime)dt.Rows[0]["brainstorm_data_ins"]).ToString("dd/MM/yyyy");
                    reuniaoBrainstorm.Data_alt = ((DateTime)dt.Rows[0]["brainstorm_data_alt"]).ToString("dd/MM/yyyy");


                    return reuniaoBrainstorm;
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public List<Tema> getBrainstormTemas(int id)
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("[dbo].[getBrainstormTemas]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_tema_brainstorm_id", id);


                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);
               // var intervenientes = new List<Interveniente>();
                var temas = new List<Tema>();
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Tema tema = new Tema();
                        tema.Id = Convert.ToInt32(row["brainstorm_tema_id"].ToString());
                        tema.Titulo = row["brainstorm_tema_titulo"].ToString();
                        tema.Descricao = row["brainstorm_tema_descricao"].ToString();
                        //tema.Importancia = row["brainstorm_tema_importancia"].ToString();
                        tema.Comentarios = row["brainstorm_tema_comentarios"].ToString();
                        tema.Estado = row["brainstorm_tema_estado"].ToString();
                        if (row["brainstorm_tema_gestInov"].ToString() == "1")
                        {
                            tema.GestaoInov = true;
                        }
                        else
                            tema.GestaoInov = false;
                        if (row["brainstorm_tema_actividade"].ToString() == "1")
                        {
                            tema.Actividade = true;
                        }
                        else
                            tema.Actividade = false;

                        temas.Add(tema);
                    }


                    return temas;
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public DataRow alterarReuniao(BrainstormViewModel model, int id)
        {
          

           SqlConnection con = null;
          
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[alterarBrainstorm]", con);
                //(intervenientes valor como default ???????)
                sqlComm.Parameters.AddWithValue("@brainstorm_id", id);
                sqlComm.Parameters.AddWithValue("@brainstorm_data", model.ReuniaoBrainstorm.Data);
                sqlComm.Parameters.AddWithValue("@brainstorm_est_codigo", model.ReuniaoBrainstorm.Estado);
                sqlComm.Parameters.AddWithValue("@brainstorm_duracaoPrev", model.ReuniaoBrainstorm.Duracao);
                sqlComm.Parameters.AddWithValue("@brainstorm_duracaoReal", model.ReuniaoBrainstorm.DuracaoReal);
                sqlComm.Parameters.AddWithValue("@brainstorm_observacoes", ((object)model.ReuniaoBrainstorm.Observacoes) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_local", ((object)model.ReuniaoBrainstorm.Local) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_ut_ins", ((object)model.ReuniaoBrainstorm.Utilizador_ins) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_ut_alt", ((object)model.ReuniaoBrainstorm.Utilizador_alt) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_data_ins", ((object)model.ReuniaoBrainstorm.Data_ins) ?? DBNull.Value);
                sqlComm.Parameters.AddWithValue("@brainstorm_data_alt", ((object)model.ReuniaoBrainstorm.Data_alt) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv1_codigo", model.Intervenientes[0].Codigo);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv2_codigo", ((object)model.Intervenientes[1].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv3_codigo", ((object)model.Intervenientes[2].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv4_codigo", ((object)model.Intervenientes[3].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv5_codigo", ((object)model.Intervenientes[4].Codigo) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv1_descritivo", model.Intervenientes[0].Nome);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv2_descritivo", ((object)model.Intervenientes[1].Nome) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv3_descritivo", ((object)model.Intervenientes[2].Nome) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv4_descritivo", ((object)model.Intervenientes[3].Nome) ?? DBNull.Value);
                //sqlComm.Parameters.AddWithValue("@brainstorm_interv5_descritivo", ((object)model.Intervenientes[4].Nome) ?? DBNull.Value);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);




                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0];
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public void alterarTema(Tema tema, int id)
        {
           

            SqlConnection con = null;
         
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                //@brainstorm_tema_brainstorm_id int,
                //@brainstorm_tema_titulo nvarchar(4000),
                //@brainstorm_tema_descricao nvarchar(4000),
                //@brainstorm_tema_importancia nvarchar(4000),
                //@brainstorm_tema_comentarios nvarchar(4000),
                //@brainstorm_tema_estado codigo_pequeno,
                //    @brainstorm_tarefa_gestInov int
                SqlCommand sqlComm = new SqlCommand("[dbo].[alterarBrainstormTema]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_tema_id", tema.Id);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_brainstorm_id", id);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_titulo", tema.Titulo);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_descricao", tema.Descricao);
                //sqlComm.Parameters.AddWithValue("@brainstorm_tema_importancia", tema.Importancia);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_comentarios", tema.Comentarios);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_estado", tema.Estado);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_gestInov", tema.GestaoInov);
                sqlComm.Parameters.AddWithValue("@brainstorm_tema_actividade", tema.Actividade);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

              
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
               
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

           

        }

        public DataRow guardarEstado(string estado, string data, int id, string utilizador)
        {

            SqlConnection con = null;

            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[guardarBrainstormEstado]", con);
             
                sqlComm.Parameters.AddWithValue("@brainstorm_wf_estado_brainstorm_id", id);
                sqlComm.Parameters.AddWithValue("@brainstorm_estado_ut_codigo", utilizador);
                sqlComm.Parameters.AddWithValue("@brainstorm_estado_data", data);
                sqlComm.Parameters.AddWithValue("@brainstorm_estado_est_codigo", estado);              
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);




                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0];
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public List<GestaoBrainstorm> getReunioesBrainstorm()
        {

            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("select *, (select top 1 brainstorm_tema_descricao from brainstorm_tema where brainstorm_tema_brainstorm_id = brainstorm_id order by brainstorm_tema_id desc) as descricao from brainstorm", con);

                //sqlComm.Parameters.AddWithValue("@brainstorm_id", id);


                //sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                var reunioes = new List<GestaoBrainstorm>();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        GestaoBrainstorm reuniaoBrainstorm = new GestaoBrainstorm();

                        reuniaoBrainstorm.Id = Convert.ToInt32(row["brainstorm_id"].ToString());
                        // reuniaoBrainstorm.Titulo = row["brainstorm_est_codigo"].ToString();
                        reuniaoBrainstorm.Data = ((DateTime)row["brainstorm_data"]).ToString("dd/MM/yyyy");
                        reuniaoBrainstorm.Estado = row["brainstorm_est_codigo"].ToString();
                        reuniaoBrainstorm.Duracao = Convert.ToInt32(row["brainstorm_duracaoPrev"].ToString());
                        reuniaoBrainstorm.DuracaoReal = Convert.ToInt32(row["brainstorm_duracaoReal"].ToString());
                        reuniaoBrainstorm.Observacoes = row["brainstorm_observacoes"].ToString();
                        reuniaoBrainstorm.Local = row["brainstorm_local"].ToString();
                        reuniaoBrainstorm.Descricao = row["descricao"].ToString();
                        //reuniaoBrainstorm.Utilizador_ins = row["brainstorm_ut_ins"].ToString();
                        //reuniaoBrainstorm.Utilizador_alt = row["brainstorm_ut_alt"].ToString();
                        //reuniaoBrainstorm.Data_ins = ((DateTime)row["brainstorm_data_ins"]).ToString("dd/MM/yyyy");
                        //reuniaoBrainstorm.Data_alt = ((DateTime)row["brainstorm_data_alt"]).ToString("dd/MM/yyyy");
                        reunioes.Add(reuniaoBrainstorm);
                    }
                    return reunioes;
                }
           

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public void eliminarBrainstorm(int id)
        {

            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("[dbo].[eliminarBrainstorm]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_id", id);


                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);
               
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }           

        }


        public string getWorkflow(int id)
        {

            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("SELECT dbo.brainstormWorkflow(@brainstorm_id) AS Workflow", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_id", id);

                //SqlParameter brainstorm_id = new SqlParameter("@brainstorm_id", SqlDbType.Int);
                //brainstorm_id.Value = id;

                // sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);
                //string workflow;
                //workflow = sqlComm.ExecuteScalar();
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    string workflow;
                    workflow = dt.Rows[0]["Workflow"].ToString();

                    return workflow;
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return null;

        }

        public void deleteTemas(int? id)
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("Delete FROM brainstorm_tema WHERE brainstorm_tema_brainstorm_id =" + id, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                var intervenientes = new List<Interveniente>();

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


        }

        public DataRow gerarActividade(Tema tema, string idTabelaAux, string ut, string moduloID, string componenteID, int idReuniao)
        {
           
                SqlConnection conn = null;
                try
                {
                    string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                    conn = new SqlConnection(strConnString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
                }
                try
                {
                    SqlCommand sqlComm = new SqlCommand("[dbo].[adiciona_actividade_proj]", conn);
                    sqlComm.Parameters.AddWithValue("@id", idTabelaAux);
                    sqlComm.Parameters.AddWithValue("@tpAct", "proj");
                    sqlComm.Parameters.AddWithValue("@recCod", "");
                    sqlComm.Parameters.AddWithValue("@recIDIObs", "");
                    sqlComm.Parameters.AddWithValue("@tempoHoras", 0);
                    sqlComm.Parameters.AddWithValue("@dataIni", "");
                    sqlComm.Parameters.AddWithValue("@dataFim", "");
                    sqlComm.Parameters.AddWithValue("@modCod", moduloID);
                    sqlComm.Parameters.AddWithValue("@modSeq", 0);
                    sqlComm.Parameters.AddWithValue("@comCod", componenteID);
                    sqlComm.Parameters.AddWithValue("@comSeq", 0);
                    sqlComm.Parameters.AddWithValue("@actCod", "DV");
                    sqlComm.Parameters.AddWithValue("@actseq", 0);
                    sqlComm.Parameters.AddWithValue("@actDesc", tema.Titulo);
                    sqlComm.Parameters.AddWithValue("@versao", "");
                    sqlComm.Parameters.AddWithValue("@testes", "");
                    sqlComm.Parameters.AddWithValue("@especificacao", tema.Descricao);
                    sqlComm.Parameters.AddWithValue("@comentarios", tema.Comentarios);
                    sqlComm.Parameters.AddWithValue("@tipo", 1);
                    sqlComm.Parameters.AddWithValue("@natureza", 0);
                    sqlComm.Parameters.AddWithValue("@dfprioridade", 0);
                    sqlComm.Parameters.AddWithValue("@sem_agendamento", 0);
                    sqlComm.Parameters.AddWithValue("@act_critic", 0);
                    sqlComm.Parameters.AddWithValue("@act_typeDesc", "Reunião");
                    sqlComm.Parameters.AddWithValue("@trabalho_realizado", "D");
                    sqlComm.Parameters.AddWithValue("@objectos", "");
                    sqlComm.Parameters.AddWithValue("@estado", "P");
                    sqlComm.Parameters.AddWithValue("@utilizador", ut);
                    sqlComm.Parameters.AddWithValue("@tipoActividade", "I");
                    sqlComm.Parameters.AddWithValue("@publicar_actividade", 0);
                    sqlComm.Parameters.AddWithValue("@prazo_exec", "");
                    sqlComm.Parameters.AddWithValue("@DocumentarAct", 0);
                    sqlComm.Parameters.AddWithValue("@DocumentarObs", "");
                    //sqlComm.Parameters.AddWithValue("@rtp_id", 0);
                    sqlComm.Parameters.AddWithValue("@brainstorm_id", idReuniao);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        return null;
                    }
                    else return dt.Rows[0];
                }
                catch (SqlException ex)
                {
                    System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                    return null;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            
        }
        public string getTabelaId()
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("select  dbo.GetTab_Config('OF_FO_ID',1) as id", con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                var intervenientes = new List<Interveniente>();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                   
                    return dt.Rows[0]["id"].ToString();
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public string getComponente()
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("select  dbo.GetTab_Config('OF_COMP_CODIGO',1) as componenteID", con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                var intervenientes = new List<Interveniente>();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0]["componenteID"].ToString();
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public string getModulo()
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("select  dbo.GetTab_Config('OF_MOD_CODIGO',1) as moduloID", con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                var intervenientes = new List<Interveniente>();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0]["moduloID"].ToString();
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }


        public DataRow guardarBrainstorm_anexo(int id_brainstorm, string path, string nome, string length)
        {

            SqlConnection con = null;

            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[guardarBrainstorm_anexo]", con);
                sqlComm.Parameters.AddWithValue("@brainstorm_anexo_brainstorm_id",id_brainstorm);
                sqlComm.Parameters.AddWithValue("@brainstorm_anexo_path", path);
                sqlComm.Parameters.AddWithValue("@brainstorm_anexo_nome", nome);
                sqlComm.Parameters.AddWithValue("@brainstorm_anexo_length", length);               
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {

                    return dt.Rows[0];
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public List<Anexo> getBrainstormAnexos(int id)
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("[dbo].[getBrainstormAnexos]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_anexo_brainstorm_id", id);


                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);
                // var intervenientes = new List<Interveniente>();
                var anexos = new List<Anexo>();
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                       
                        Anexo anexo = new Anexo();
                        anexo.AnexoID = Convert.ToInt32(row["brainstorm_anexo_id"].ToString());
                        anexo.Path = row["brainstorm_anexo_path"].ToString();
                        anexo.FileName = row["brainstorm_anexo_nome"].ToString();
                        //tema.Importancia = row["brainstorm_tema_importancia"].ToString();
                        anexo.Tamanho = row["brainstorm_anexo_length"].ToString();
                                            
                        anexos.Add(anexo);
                    }


                    return anexos;
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }
        public Anexo getBrainstormAnexoByID(int id)
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("SELECT * FROM brainstorm_anexos where brainstorm_anexo_id=" + id, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

              


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                   
                        Anexo anexo = new Anexo();
                        anexo.AnexoID = Convert.ToInt64(dt.Rows[0]["brainstorm_anexo_id"].ToString());
                        anexo.FileName = dt.Rows[0]["brainstorm_anexo_nome"].ToString();
                        anexo.Path = dt.Rows[0]["brainstorm_anexo_path"].ToString();
                        anexo.Tamanho = dt.Rows[0]["brainstorm_anexo_length"].ToString();
                       
                    
                    return anexo;
                }

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }

        public void UpdateActividadeIdeia(string[] infoAtividade)
        {
            SqlConnection conn = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                conn = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[Update_Actividade_Ideia]", conn);

                sqlComm.Parameters.AddWithValue("@of_proj_art_op_of_id", infoAtividade[0]);
                sqlComm.Parameters.AddWithValue("@of_proj_art_op_ttrab_codigo", infoAtividade[1]);
                sqlComm.Parameters.AddWithValue("@of_proj_art_op_forma_codigo", infoAtividade[2]);
                sqlComm.Parameters.AddWithValue("@of_proj_art_op_seq", infoAtividade[3]);
                sqlComm.Parameters.AddWithValue("@of_proj_art_op_ideia_id", infoAtividade[4]);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public string Verifica_GestInov_Configurado()
        {
            SqlConnection conn = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                conn = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }

            try
            {
                string sqlquery = "select dbo.[GetTAB_Config] ('C_GESTINOV_NOVO',2)";

                SqlDataAdapter adp = new SqlDataAdapter(sqlquery, conn);
                DataTable dt = new DataTable();

                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    return "0";
                }
                else return "1";
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  : " + ex.Message);
                return "0";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public IHttpActionResult CriarActividade(ReuniaoBrainstorm brainstorm, Tema tema, string responsavel)
        {
            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[adiciona_actividade_IDI]", con);
                sqlComm.Parameters.AddWithValue("@actividade_ideia_id", brainstorm.Id);
                sqlComm.Parameters.AddWithValue("@ideia_titulo", tema.Titulo);
                sqlComm.Parameters.AddWithValue("@actividade_especificacao", tema.Descricao);
                sqlComm.Parameters.AddWithValue("@actividade_prazo_exec", "18-07-2017");
                sqlComm.Parameters.AddWithValue("@actividade_comentarios", tema.Comentarios);
                sqlComm.Parameters.AddWithValue("@resp_exec", responsavel);

                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else return null;
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public DataTable Get_Eq_Agendamento()
        {
            SqlConnection conn = null;

            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                conn = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }

            try
            {

                string sqlquery = "select distinct tadet_codigo,ut_email from tadet inner join ut on ut_codigo=tadet_codigo where tadet_ta_codigo ='ACT_EQAG' and ISNULL(ut_email,'')<>''";


                //
                // criar um Datadapter para executar o comando
                //
                SqlDataAdapter adp = new SqlDataAdapter(sqlquery, conn);
                DataTable dt = new DataTable();


                //
                // executar o comando e preencher o DataTable
                //
                adp.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else return dt;

            }
            catch (SqlException ex)
            {
                //
                // tratar a excepção!!!!
                System.Console.WriteLine("EXCEPÇÃO  : " + ex.Message);
                return null;
            }
            finally
            {
                //
                // fechar a conexão
                //
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public DataTable getDescritivoPorCodigo(string cod_ut)
        {

            SqlConnection conn = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                conn = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {
                SqlCommand sqlComm = new SqlCommand("[dbo].[get_descritivoUt_por_codigo]", conn);
                sqlComm.Parameters.AddWithValue("@ut_codigo", cod_ut);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void guardarChatMensagens(string mensagem, int idTema, int idReuniao, string ut, string data)
        {

            SqlConnection con = null;

            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("[dbo].[guardarBrainstormChatMensagens]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_chat_brainstorm_id", idReuniao);
                sqlComm.Parameters.AddWithValue("@brainstorm_chat_tema_id", idTema);
                sqlComm.Parameters.AddWithValue("@brainstorm_chat_mensagem", mensagem);
                sqlComm.Parameters.AddWithValue("@brainstorm_chat_ut", ut);
                sqlComm.Parameters.AddWithValue("@brainstorm_chat_data", data);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);             

            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
               
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }           

        }

        public List<ChatMensagem> getBrainstormChatMensagens(int idTema, int idReuniao)
        {

            SqlConnection con = null;
            try
            {
                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                con = new SqlConnection(strConnString);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
            }
            try
            {

                SqlCommand sqlComm = new SqlCommand("[dbo].[getBrainstormChatMensagens]", con);
                sqlComm.Parameters.AddWithValue("@brainstorm_chat_brainstorm_id", idReuniao);
                sqlComm.Parameters.AddWithValue("@brainstorm_chat_tema_id", idTema);

                //sqlComm.Parameters.AddWithValue("@brainstorm_id", id);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                DataTable dt = new DataTable();
                da.Fill(dt);

                var mensagens = new List<ChatMensagem>();


                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ChatMensagem mensagem = new ChatMensagem();

                        mensagem.Mensagem = row["brainstorm_chat_mensagem"].ToString();
                        mensagem.CodigoUtilizador = row["brainstorm_chat_ut"].ToString();
                        mensagem.Data = ((DateTime)row["brainstorm_chat_data"]).ToString("dd/MM/yyyy HH:mm");                                              
                        mensagens.Add(mensagem);
                    }
                    return mensagens;
                }


            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return null;

        }
    }
}
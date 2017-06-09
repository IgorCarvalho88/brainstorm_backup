﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Brainstorm.Models;

namespace Brainstorm.Repository.Database
{
    public class IntervenientesDB : IIntervenientes
    {

        public List<Interveniente> getUT()
        {
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
                SqlCommand sqlComm = new SqlCommand("SELECT * FROM ut", con);
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
                    foreach (DataRow row in dt.Rows)
                    {
                        Interveniente interveniente = new Interveniente();                      
                        interveniente.Nome = row["ut_descritivo"].ToString();
                        interveniente.Codigo = row["ut_codigo"].ToString();
                        interveniente.NomeAndCodigo = "(" + row["ut_codigo"].ToString() + ")" + "   " + row["ut_descritivo"].ToString();
                        intervenientes.Add(interveniente);
                    }
                    return intervenientes;
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

        public List<Interveniente> getBrainstormIntervenientes(int id)
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

                SqlCommand sqlComm = new SqlCommand("[dbo].[getBrainstormIntervenientes]", con);

                sqlComm.Parameters.AddWithValue("@brainstorm_id", id);


                sqlComm.CommandType = CommandType.StoredProcedure;
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
                    /*foreach (DataRow row in dt.Rows)
                    {

                        //Interveniente interveniente = new Interveniente();
                        //interveniente.Nome = row["ut_descritivo"].ToString();
                        //interveniente.Codigo = row["ut_codigo"].ToString();
                        //interveniente.NomeAndCodigo = "(" + row["ut_codigo"].ToString() + ")" + "   " + row["ut_descritivo"].ToString();
                        //intervenientes.Add(interveniente);
                        Interveniente interveniente = new Interveniente();
                        interveniente.Nome = row["ut_descritivo"].ToString();
                        interveniente.Codigo = row["ut_codigo"].ToString();
                        interveniente.NomeAndCodigo = "(" + row["ut_codigo"].ToString() + ")" + "   " + row["ut_descritivo"].ToString();
                        tema.Titulo = row["brainstorm_tema_titulo"].ToString();
                        tema.Descricao = row["brainstorm_tema_descricao"].ToString();
                        tema.Importancia = row["brainstorm_tema_importancia"].ToString();
                        tema.Comentarios = row["brainstorm_tema_comentarios"].ToString();
                        tema.Estado = row["brainstorm_tema_estado"].ToString();
                        tema.GestaoInov = Convert.ToInt32(dt.Rows[0]["brainstorm_tarefa_gestInov"].ToString());
                        temas.Add(tema);
                    }*/

                    Interveniente interveniente1 = new Interveniente();
                    interveniente1.Nome = dt.Rows[0]["brainstorm_interv1_codigo"].ToString();
                    interveniente1.Codigo = dt.Rows[0]["brainstorm_interv1_descritivo"].ToString();
                    interveniente1.Nome = dt.Rows[0]["brainstorm_interv1_codigo"].ToString();        
                    interveniente1.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv1_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv1_descritivo"].ToString();

                    Interveniente interveniente2 = new Interveniente();
                    interveniente2.Nome = dt.Rows[0]["brainstorm_interv2_codigo"].ToString();
                    interveniente2.Codigo = dt.Rows[0]["brainstorm_interv2_descritivo"].ToString();
                    interveniente2.Nome = dt.Rows[0]["brainstorm_interv2_codigo"].ToString();
                    interveniente2.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv2_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv2_descritivo"].ToString();

                    Interveniente interveniente3 = new Interveniente();
                    interveniente3.Nome = dt.Rows[0]["brainstorm_interv3_codigo"].ToString();
                    interveniente3.Codigo = dt.Rows[0]["brainstorm_interv3_descritivo"].ToString();
                    interveniente3.Nome = dt.Rows[0]["brainstorm_interv3_codigo"].ToString();
                    interveniente3.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv3_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv3_descritivo"].ToString();

                    Interveniente interveniente4 = new Interveniente();
                    interveniente4.Nome = dt.Rows[0]["brainstorm_interv4_codigo"].ToString();
                    interveniente4.Codigo = dt.Rows[0]["brainstorm_interv4_descritivo"].ToString();
                    interveniente4.Nome = dt.Rows[0]["brainstorm_interv4_codigo"].ToString();
                    interveniente4.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv4_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv4_descritivo"].ToString();

                    Interveniente interveniente5 = new Interveniente();
                    interveniente5.Nome = dt.Rows[0]["brainstorm_interv5_codigo"].ToString();
                    interveniente5.Codigo = dt.Rows[0]["brainstorm_interv5_descritivo"].ToString();
                    interveniente5.Nome = dt.Rows[0]["brainstorm_interv5_codigo"].ToString();
                    interveniente5.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv5_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv5_descritivo"].ToString();

                    intervenientes.Add(interveniente1);
                    intervenientes.Add(interveniente2);
                    intervenientes.Add(interveniente3);
                    intervenientes.Add(interveniente4);
                    intervenientes.Add(interveniente5);

                    return intervenientes;
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
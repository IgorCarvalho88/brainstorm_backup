using System;
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
                        interveniente.NomeAndCodigo = "(" + row["ut_codigo"].ToString() + ")" + "    " + row["ut_descritivo"].ToString();
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
    }
}
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
                SqlCommand sqlComm = new SqlCommand("SELECT * FROM ut where ut_estado = 'activo'", con);
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
                        interveniente.NomeAndCodigo = "(" + row["ut_codigo"].ToString() + ")" + "   " +
                                                      row["ut_descritivo"].ToString();
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

        //public List<Interveniente> getBrainstormIntervenientes(int id)
        //{
        //    SqlConnection con = null;
        //    try
        //    {
        //        string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        //        con = new SqlConnection(strConnString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Impossível de momento ligar a base de dados, tente mais tarde", ex.InnerException);
        //    }
        //    try
        //    {

        //        SqlCommand sqlComm = new SqlCommand("[dbo].[getBrainstormIntervenientes]", con);

        //        sqlComm.Parameters.AddWithValue("@brainstorm_id", id);


        //        sqlComm.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = sqlComm;
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        var intervenientes = new List<Interveniente>();

        //        if (dt.Rows.Count == 0)
        //        {
        //            return null;
        //        }
        //        else
        //        {

        //            Interveniente interveniente1 = new Interveniente();
        //            interveniente1.Nome = dt.Rows[0]["brainstorm_interv1_descritivo"].ToString();
        //            interveniente1.Codigo = dt.Rows[0]["brainstorm_interv1_codigo"].ToString();                           
        //            interveniente1.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv1_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv1_descritivo"].ToString();

        //            Interveniente interveniente2 = new Interveniente();
        //            // if necessario caso venham valores null da base de dados
        //            if (dt.Rows[0]["brainstorm_interv2_codigo"] != DBNull.Value)
        //            {
        //                interveniente2.Nome = dt.Rows[0]["brainstorm_interv2_descritivo"].ToString();
        //                interveniente2.Codigo = dt.Rows[0]["brainstorm_interv2_codigo"].ToString();
        //                interveniente2.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv2_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv2_descritivo"].ToString();
        //            }

        //            Interveniente interveniente3 = new Interveniente();
        //            if (dt.Rows[0]["brainstorm_interv3_codigo"] != DBNull.Value)
        //            {

        //                interveniente3.Nome = dt.Rows[0]["brainstorm_interv3_descritivo"].ToString();
        //                interveniente3.Codigo = dt.Rows[0]["brainstorm_interv3_codigo"].ToString();
        //                interveniente3.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv3_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv3_descritivo"].ToString();
        //            }

        //            Interveniente interveniente4 = new Interveniente();
        //            if (dt.Rows[0]["brainstorm_interv4_codigo"] != DBNull.Value)
        //            {

        //                interveniente4.Nome = dt.Rows[0]["brainstorm_interv4_descritivo"].ToString();
        //                interveniente4.Codigo = dt.Rows[0]["brainstorm_interv4_codigo"].ToString();
        //                interveniente4.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv4_codigo"].ToString() + ")" +  "   " + dt.Rows[0]["brainstorm_interv4_descritivo"].ToString();
        //            }

        //            Interveniente interveniente5 = new Interveniente();
        //            if (dt.Rows[0]["brainstorm_interv5_codigo"] != DBNull.Value)
        //            {

        //                interveniente5.Nome = dt.Rows[0]["brainstorm_interv5_descritivo"].ToString();
        //                interveniente5.Codigo = dt.Rows[0]["brainstorm_interv5_codigo"].ToString();
        //                interveniente5.NomeAndCodigo = "(" + dt.Rows[0]["brainstorm_interv5_codigo"].ToString() + ")" + "   " + dt.Rows[0]["brainstorm_interv5_descritivo"].ToString();
        //            }

        //            intervenientes.Add(interveniente1);
        //            intervenientes.Add(interveniente2);
        //            intervenientes.Add(interveniente3);
        //            intervenientes.Add(interveniente4);
        //            intervenientes.Add(interveniente5);

        //            return intervenientes;
        //        }

        //    }
        //    catch (SqlException ex)
        //    {
        //        System.Console.WriteLine("EXCEPÇÃO  get periodos ultimas 8 horas: " + ex.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }

        //    return null;
        //}


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

                //@brainstorm_interveniente_brainstorm_id int,
                //@brainstorm_interveniente_codigo nvarchar(50),
                //@brainstorm_interveniente_descritivo nvarchar(200)
                SqlCommand sqlComm = new SqlCommand("SELECT * FROM brainstorm_interveniente WHERE brainstorm_interveniente_brainstorm_id =" + id, con);
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
                        interveniente.Nome = row["brainstorm_interveniente_descritivo"].ToString();
                        interveniente.Codigo = row["brainstorm_interveniente_codigo"].ToString();
                        interveniente.NomeAndCodigo = "(" + row["brainstorm_interveniente_codigo"].ToString() + ")" + "   " +
                                                      row["brainstorm_interveniente_descritivo"].ToString();
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

        public void deleteIntervenientes(int id)
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
                SqlCommand sqlComm = new SqlCommand("Delete FROM brainstorm_interveniente WHERE brainstorm_interveniente_brainstorm_id =" + id, con);            
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
    }
}
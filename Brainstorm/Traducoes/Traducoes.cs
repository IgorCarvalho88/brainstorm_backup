using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Configuration;

namespace Brainstorm.Traducoes
{
    public class Traducoes
    {
        public static String Traduz(String Termo)
        {
            //var Termo = JsonConvert.DeserializeObject<dynamic>(Termo2);

           
                string idioma = "";

                if (HttpContext.Current.Session["Idioma"] != null)
                {
                    idioma = HttpContext.Current.Session["Idioma"].ToString();
                }
                else
                {
                    idioma = "POR";
                }

                string sSql = "FUN_TRAD";
                string Traducao = "";

                string strConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;


                try
                {
                    SqlConnection con = new SqlConnection(strConnString);

                    SqlCommand com = new SqlCommand(sSql, con);
                    con.Open();
                    com.Parameters.Add("@Termo", SqlDbType.VarChar, 8000);
                    com.Parameters["@Termo"].Value = Termo;
                    com.Parameters.Add("@Lingua", SqlDbType.VarChar, 20);
                    com.Parameters["@Lingua"].Value = idioma;


                //if (HttpContext.Current.Session["opcoes_codigo"] != null)
                //{
                //    com.Parameters.Add("@opcoes_codigo", SqlDbType.VarChar, 20);
                //    com.Parameters["@opcoes_codigo"].Value = HttpContext.Current.Session["opcoes_codigo"].ToString();
                //}
                string opcoesCodigo = null;
                if (HttpContext.Current.Session["opcoes_codigo"] != null)
                {
                    opcoesCodigo = HttpContext.Current.Session["opcoes_codigo"].ToString();
                }
                    if (opcoesCodigo != null)
                    {
                        com.Parameters.Add("@opcoes_codigo", SqlDbType.VarChar, 20);
                        com.Parameters["@opcoes_codigo"].Value = opcoesCodigo;
                    }

                    com.CommandType = CommandType.StoredProcedure;

                    SqlDataReader RDB = com.ExecuteReader();
                    RDB.Read();

                    Traducao = RDB.GetValue(0).ToString();
                    RDB.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    HttpContext.Current.Response.Write(e.Message);
                }
              
            
            return Traducao;

        }
    }
}
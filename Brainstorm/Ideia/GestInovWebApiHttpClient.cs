using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Brainstorm.Ideia
{
    public class GestInovWebApiHttpClient
    {
        public static HttpClient GetClient()
        {
            string WebApiBaseAddress = "http:" + HttpContext.Current.Session["URL"].ToString() + "GESTINOVAPI/";

            HttpClientHandler hch = new HttpClientHandler();
            hch.Proxy = null;
            hch.UseProxy = false;

            HttpClient client = new HttpClient(hch);

            client.BaseAddress = new Uri(WebApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            //auth
            var session = HttpContext.Current.Session;
            if (session["token"] != null)
            {
                TokenResponse tokenResponse = getToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenResponse.AccessToken);
            }

            return client;
        }

        public static void storeToken(TokenResponse token)
        {
            var session = HttpContext.Current.Session;
            session["token"] = token;
        }

        public static TokenResponse getToken()
        {

            var session = HttpContext.Current.Session;
            return (TokenResponse)session["token"];
        }

        public static bool grantAccess()
        {
            if (System.Web.HttpContext.Current.Session["utilizador_codigo"] != null)
            {
                //auth
                using (var client = GetClient())
                {
                    string username = System.Web.HttpContext.Current.Session["utilizador_codigo"].ToString();
                    string password = Uri.EscapeDataString(System.Web.HttpContext.Current.Session["utilizador_password"].ToString());

                    HttpContent content = new StringContent("grant_type=password&username=" + username + "&password=" + password, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                    var response = client.PostAsync("Token", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string contentResponse = response.Content.ReadAsStringAsync().Result;
                        TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(contentResponse);
                        storeToken(tokenResponse);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
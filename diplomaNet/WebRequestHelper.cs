using Newtonsoft.Json;
//using ServiceCenterApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Text.Json.Nodes;
using System.Web;

namespace diplomaNet
{
    /// <summary>
    /// Класс для связи с сервером
    /// </summary>
    internal static class WebRequestHelper
    {
        static string Url { get; set; } = Properties.Settings.Default.Url;
        public static class Get<T>
        {
            /// <summary>
            /// Отправляет GET запрос на сервер и возращает коллекцию объектов типа T
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public static ObservableCollection<T> Send(string uri, string param)
            {
                if (!string.IsNullOrEmpty(param))
                    param = "?" + param;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{Url}{uri}{param}");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string dataObjects = response.Content.ReadAsStringAsync().Result;
                            return JsonConvert.DeserializeObject<ObservableCollection<T>>(dataObjects);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.InnerException.Message;
                    }
                }
                return null;
            }

            /// <summary>
            /// Отправляет GET запрос на сервер и возвращает один объект типа T
            /// </summary>
            /// <param name="uri">Маршрут API контроллера</param>
            /// <param name="id">Id объекта в базе данных</param>
            /// <returns>Объект типа T</returns>
            public static T Send(string uri, int id)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{Url}{uri}?id={id}");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string dataObject = response.Content.ReadAsStringAsync().Result;
                            return JsonConvert.DeserializeObject<T>(dataObject);
                        }
                    }
                    catch(Exception ex)
                    {
                        string message = ex.InnerException.Message;
                    }
                }
                return default(T);
            }
        }

        /// <summary>
        /// Проверяет подключение к серверу
        /// </summary>
        public static class TestConnection
        {
            public static string Test()
            {
                string message = "";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{Url}");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = client.GetAsync("").Result;
                        return "ok";
                    }
                    catch (Exception ex)
                    {
                        message = ex.InnerException.InnerException.Message;
                    }
                }
                return message;
            }
        }


        public static class Post
        {
            private static string jsonObject;

            public static object JsonObject
            {
                get { return jsonObject; }
                set { jsonObject = JsonConvert.SerializeObject(value); }
            }

            public static bool Send(object jsonObject, string uri)
            {
                JsonObject = jsonObject;

                WebRequest request = (HttpWebRequest)WebRequest.Create($"{Url}{uri}");
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var requestStream = request.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(JsonObject);
                }
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return true;
                }
                return false;
            }

        }

        public static class Delete
        {

            public static bool Send(int[] ids, string uri)
            {
                string json = JsonConvert.SerializeObject(ids);
                WebRequest request = WebRequest.Create($"{Url}{uri}");
                request.Method = "DELETE";
                request.ContentType = "application/json";
                using (var requestStream = request.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(json);
                }
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return true;
                }
                return false;
            }

            
        }

        public static class Put
        {
            private static string jsonObject;
            public static object JsonObject
            {
                get { return jsonObject; }
                set { jsonObject = JsonConvert.SerializeObject(value); }
            }
            public static bool Send(object jsonObject, string uri)
            {
                JsonObject = jsonObject;

                WebRequest request = (HttpWebRequest)WebRequest.Create($"{Url}{uri}");
                request.Method = "PUT";
                request.ContentType = "application/json";
                using (var requestStream = request.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(JsonObject);
                }
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return true;
                }
                return false;
            }
        }

    }
}

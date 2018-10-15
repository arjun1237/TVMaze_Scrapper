using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TVMaze_Scrapper.Services
{
    public class HTTPServices
    {
        // HTTP service for scraping
        public static T GetHTTPService<T>(string URL)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            string urlParameters = "";

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(result);  // deserialize
            }
            else
            {
                return default(T);
            }
        }
    }
}

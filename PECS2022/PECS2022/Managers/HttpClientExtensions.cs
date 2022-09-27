using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PECS2022.Managers
{
  public static  class HttpClientExtensions
    {

        public async static Task<T> ReadAsAsync<T>(this HttpContent content) where T : class
        {
            var  contentStr =  await content.ReadAsStringAsync();

            return  JsonConvert.DeserializeObject<T>(contentStr);

        }

        //ReadStructAsAsync

        public async static Task<T> ReadStructAsAsync<T>(this HttpContent content) where T : struct
        {
            var contentStr = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(contentStr);

        }


        public  static HttpResponseMessage PostAsJsonAsync(this HttpClient client, string requestUri, object item)
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            response =  client.PostAsync(requestUri, content).Result;

            return response;

        }
 



    }
}

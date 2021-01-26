using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AiurTodo.CSharp.Models;
using Newtonsoft.Json;

namespace AiurTodo.CSharp.Utils
{
    public static class HttpClientUtils
    {
        static readonly HttpClient client = new HttpClient();
        
        public static async Task<string> GetJson(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyWeatherApp.Helper
{
    public class ApiCaller
    {
        public static async Task<ApiResponse> Get(string url, string auth = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(auth))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", auth);

                var request = await client.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    return new ApiResponse { Response = await request.Content.ReadAsStringAsync() };
                }
                else
                {
                    return new ApiResponse { Error = request.ReasonPhrase };
                }
            }
        }
    }

    public class ApiResponse
    {
        public bool Successful => Error == null;

        public string Error { get; set; }

        public string Response { get; set; }
    }
}

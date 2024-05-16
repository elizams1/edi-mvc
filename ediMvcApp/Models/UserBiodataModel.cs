using ediMvcApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace ediMvcApp.Models
{
    public class UserBiodataModel
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        private Response? apiResponse;
        private string jsonData;

        public UserBiodataModel(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }

        public List<Biodata>? GetBiodataById(int id)
        {
            List<Biodata>? biodata = null;
            try
            {
                apiResponse = JsonConvert.DeserializeObject<Response?>(httpClient.GetStringAsync(apiUrl + $"Biodata/GetBiodataById?bioId={id}").Result);

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode == HttpStatusCode.OK)
                    {
                        biodata = JsonConvert.DeserializeObject<List<Biodata>?>(JsonConvert.SerializeObject(apiResponse.data));
                    }
                    else
                    {
                        throw new Exception(apiResponse.message);
                    }
                }
                else
                {
                    throw new Exception(apiResponse.message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return biodata;
        }
    }
}

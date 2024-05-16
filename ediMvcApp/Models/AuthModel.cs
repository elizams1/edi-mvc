using ediMvcApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ediMvcApp.Models
{
    public class AuthModel
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        private Response? apiResponse;
        private string jsonData;
        private HttpContent content;

        public AuthModel(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }

        public User? LoginUser(string email, string password)
        {
            User? user = null;
            try
            {
                apiResponse = JsonConvert.DeserializeObject<Response?>(httpClient.GetStringAsync(apiUrl+$"User/LoginUser?email={email}&password={password}").Result);

                if(apiResponse != null)
                {
                    if(apiResponse.statusCode == HttpStatusCode.OK)
                    {
                        user = JsonConvert.DeserializeObject<User?>(JsonConvert.SerializeObject(apiResponse.data));
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }


        public async Task<Response?> CreateUser(User user)
        {
            try
            {
                jsonData = JsonConvert.SerializeObject(user);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<Response?>(await httpClient.PostAsync(apiUrl + "User/CreateUser", content).Result.Content.ReadAsStringAsync());


                if(apiResponse == null)
                {
                    throw new Exception("Tambah user tidak dapat dilakukan!");
                }
            }
            catch (Exception ex) 
            {
                apiResponse.statusCode = HttpStatusCode.InternalServerError;
                apiResponse.message = ex.Message;
            }
            return apiResponse;
        }
    }
}

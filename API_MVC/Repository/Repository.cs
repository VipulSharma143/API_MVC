using API_MVC.Models;
using API_MVC.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace API_MVC.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateAsync(string url, T ObjToCreate)
        {
            var request=new HttpRequestMessage(HttpMethod.Post, url);
            if(ObjToCreate != null) 
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(ObjToCreate), Encoding.UTF8, "application/json");
            }
            var Client=_httpClientFactory.CreateClient();
            HttpResponseMessage response=await Client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request=new HttpRequestMessage(HttpMethod.Delete, url+"/"+id.ToString());
            var client=_httpClientFactory.CreateClient();
            HttpResponseMessage response=await client.SendAsync(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
           var request=new HttpRequestMessage(HttpMethod.Get, url);
            var client=_httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var jsonstring=await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonstring);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+"/"+id.ToString());
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonstring=await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonstring);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T ObjToUpdate)
        {
           var request= new HttpRequestMessage(HttpMethod.Put, url);
            if(ObjToUpdate != null)
            {
                request.Content=new StringContent(JsonConvert.SerializeObject(ObjToUpdate),Encoding.UTF8, "application/json");
            }
            var Client=_httpClientFactory.CreateClient();   
            HttpResponseMessage response=await Client.SendAsync(request);
            if(response.StatusCode==System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }
        public async Task<List<Bookings>> GetBookingsForDateAsync(string url, DateTime date)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{url}?date={date.ToString("yyyy-MM-dd")}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var bookings = JsonConvert.DeserializeObject<List<Bookings>>(jsonString);
                    return bookings;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                Console.WriteLine("Exception: " + ex.Message);
                throw; // Rethrow the exception to propagate it upwards
            }
        }

    }
}

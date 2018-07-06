using DataService.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataService
{
    public interface IWebClient
    {
        List<User> GetUsersList();
        List<Post> GetPostsList();
        List<Comment> GetCommentsList();
        List<Todo> GetTodosList();
        List<Addres> GetAddressList();
    }
    public class WebClient : IWebClient
    {
        private string baseAddress = "https://5b128555d50a5c0014ef1204.mockapi.io/";

        private async Task<string> GetJsonDataAsync(string endpoint)
        {
            Console.WriteLine($"Waiting for a response from the server {endpoint}");
            using (var client = new HttpClient())
            {
                try
                {
                    using (var response = client.GetAsync(baseAddress + endpoint).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();

                            return jsonString;
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                            return null;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public List<User> GetUsersList()
        {
            string usersJson = new WebClient().GetJsonDataAsync("users").GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<User>>(usersJson);
        }

        public List<Post> GetPostsList()
        {
            string postsJson = new WebClient().GetJsonDataAsync("posts").GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<Post>>(postsJson);
        }

        public List<Comment> GetCommentsList()
        {
            string commentsJson = new WebClient().GetJsonDataAsync("comments").GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<Comment>>(commentsJson);
        }

        public List<Todo> GetTodosList()
        {
            string todosJson = new WebClient().GetJsonDataAsync("todos").GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<Todo>>(todosJson);
        }

        public List<Addres> GetAddressList()
        {
            string addressJson = new WebClient().GetJsonDataAsync("address").GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<Addres>>(addressJson);
        }

    }
}

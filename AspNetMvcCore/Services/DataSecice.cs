
using AspNetMvcCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetMvcCore.Services
{
    public interface IDataSecice
    {
        List<User> GetUsersList();
        List<Post> GetPostsList();
        List<Comment> GetCommentsList();
        List<Todo> GetTodosList();
        List<Addres> GetAddressList();
    }

    public class DataSecice : IDataSecice
    {
        private string baseAddress = "https://5b128555d50a5c0014ef1204.mockapi.io/";

        private string GetJsonData(string endpoint)
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
                            var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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
            string usersJson = GetJsonData("users");

            return JsonConvert.DeserializeObject<List<User>>(usersJson);
        }

        public List<Post> GetPostsList()
        {
            string postsJson = GetJsonData("posts");

            return JsonConvert.DeserializeObject<List<Post>>(postsJson);
        }

        public List<Comment> GetCommentsList()
        {
            string commentsJson = GetJsonData("comments");

            return JsonConvert.DeserializeObject<List<Comment>>(commentsJson);
        }

        public List<Todo> GetTodosList()
        {
            string todosJson = GetJsonData("todos");

            return JsonConvert.DeserializeObject<List<Todo>>(todosJson);
        }

        public List<Addres> GetAddressList()
        {
            string addressJson = GetJsonData("address");

            return JsonConvert.DeserializeObject<List<Addres>>(addressJson);
        }
    }
}

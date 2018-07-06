using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetMvcCore.Models
{
    public class User
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public string Name { set; get; }
        public string Avatar { set; get; }
        public string Email { set; get; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Todo> Todos { get; set; }
        public IEnumerable<Addres> Address { get; set; }
    }
}

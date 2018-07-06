using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetMvcCore.Models
{
    public class Todo
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public string name { set; get; }
        public bool IsComplete { set; get; }
        public int UserId { set; get; }
    }
}

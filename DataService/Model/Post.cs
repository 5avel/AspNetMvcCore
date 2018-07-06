using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Model
{
    public class Post
    {
        public int Id { set; get; }
        public DateTime CreatedAt { set; get; }
        public string Title { set; get; }
        public string Body { set; get; }
        public int UserId { set; get; }
        public int Likes { set; get; }
        public IEnumerable<Comment> Comments { get; set; }

    }
}

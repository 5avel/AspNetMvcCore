
using AspNetMvcCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetMvcCore.Services
{
    public interface IQueryService
    {
        IEnumerable<(Post, int)> GetPostCommentsCountByUserId(int userId);
        IEnumerable<Comment> GetPostCommentsBodyLessThan50ByUserId(int userId);
        IEnumerable<(int, string)> GetTodoIdNameByUserId(int userId);
        IEnumerable<User> GetUsetsSortByNameAndTodosSortByNameDesc();
        (User, Post, int, int, Post, Post) Query5(int userId);
        (Post, Comment, Comment, int) Query6(int postId);
        User GetUserById(int userId);
        Post GetPostById(int postId);
        Todo GetTodoById(int todoId);

        List<User> GetUsers();

    }

    public class QueryService : IQueryService
    {
        private IDataSecice _dataSecice;

        private IEnumerable<User> _collection;

        public QueryService(IDataSecice dataSecice)
        {
            _dataSecice = dataSecice;
            CreateAssociateEntities();
        }

        private void CreateAssociateEntities()
        {
            var users = _dataSecice.GetUsersList();
            var posts = _dataSecice.GetPostsList();
            var comments = _dataSecice.GetCommentsList();
            var todos = _dataSecice.GetTodosList();
            var address = _dataSecice.GetAddressList();

            posts.ForEach(p => p.User = users.FirstOrDefault(u => u.Id == p.UserId));
            todos.ForEach(t => t.User = users.FirstOrDefault(u => u.Id == t.UserId));
            address.ForEach(a => a.User = users.FirstOrDefault(u => u.Id == a.UserId));
            comments.ForEach(c => c.User = users.FirstOrDefault(u => u.Id == c.UserId));


            var postsComments = posts.GroupJoin(comments, p => p.Id, c => c.PostId,
               (p, c) => new Post()
               {
                   Id = p.Id,
                   CreatedAt = p.CreatedAt,
                   Title = p.Title,
                   Body = p.Body,
                   UserId = p.UserId,
                   User = p.User,
                   Likes = p.Likes,
                   Comments = c
               });

            var usersPostComments = users.GroupJoin(postsComments, u => u.Id, p => p.UserId,
                (u, ps) => new User()
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreatedAt = u.CreatedAt,
                    Email = u.Email,
                    Avatar = u.Avatar,
                    Posts = ps
                });

            var usersPostCommentsTodos = usersPostComments.GroupJoin(todos, u => u.Id, t => t.UserId,
                (u, t) => new User()
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreatedAt = u.CreatedAt,
                    Email = u.Email,
                    Avatar = u.Avatar,
                    Posts = u.Posts,
                    Todos = t
                });

            _collection = usersPostCommentsTodos.GroupJoin(address, u => u.Id, a => a.UserId,
                (u, a) => new User()
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreatedAt = u.CreatedAt,
                    Email = u.Email,
                    Avatar = u.Avatar,
                    Posts = u.Posts,
                    Todos = u.Todos,
                    Address = a
                }).ToList();
        }

        //query 1
        public IEnumerable<(Post, int)> GetPostCommentsCountByUserId(int userId)
        {
            if (_collection.Count() == 0) return null;
           
                IEnumerable<Post> posts = _collection.FirstOrDefault(x => x.Id == userId)?.Posts;
                if (posts != null && posts.Count() > 0)
                {
                    return posts.Select(p => (p, p.Comments.Count()));
                }
          
            return null;
        }

        //query 2
        public IEnumerable<Comment> GetPostCommentsBodyLessThan50ByUserId(int userId)
        {
            var res = from u in _collection
                   where u.Id == userId
                   from p in u.Posts
                   from c in p.Comments
                   where c.Body.Length < 50
                   select c;
            return res;
        }

        //query 3
        public IEnumerable<(int, string)> GetTodoIdNameByUserId(int userId)
        {
            return from u in _collection
                   where u.Id == userId
                   from t in u.Todos
                   where t.IsComplete
                   select (t.Id, t.name);
        }

        //query 4
        public IEnumerable<User> GetUsetsSortByNameAndTodosSortByNameDesc()
        {
            return _collection.OrderBy(u => u.Name)
                .Select(u =>
                {
                    u.Todos = u.Todos.OrderByDescending(t => t.name.Length);
                    return u;
                });
        }

        //query 5
        public (User, Post, int, int, Post, Post) Query5(int userId)
        {
            return _collection.Where(u => u.Id == userId)
                      .Select(x =>
                      (
                          x,
                          x.Posts.OrderBy(p => p.CreatedAt).LastOrDefault(),
                          x.Posts.OrderBy(p => p.CreatedAt).LastOrDefault().Comments.Count(),
                          x.Todos.Count(t => t.IsComplete == false),
                          x.Posts.OrderBy(p => p.Comments.Count(c => c.Body.Length > 80)).Last(),
                          x.Posts.OrderBy(p => p.Likes).LastOrDefault()
                      )).FirstOrDefault();
        }

        //query 6
        public (Post, Comment, Comment, int) Query6(int postId)
        {
            var res2 = _collection.SelectMany(u => u.Posts.Where(p => p.Id == postId)
            .Select(x =>
            (
                x,
                x.Comments.OrderBy(c => c.Body.Length).FirstOrDefault(),
                x.Comments.OrderBy(c => c.Likes).FirstOrDefault(),
                x.Comments.Count(c => c.Likes == 0 || c.Body.Length < 80)
            ))).FirstOrDefault();
            return res2;
        }

        public User GetUserById(int userId)
        {
            var res2 = _collection.FirstOrDefault(x => x.Id == userId);
            return res2;
        }

        public Post GetPostById(int postId)
        {
            var res2 = _collection.SelectMany(u => u.Posts.Where(p => p.Id == postId)).Select(x => x).FirstOrDefault();
            return res2;
        }

        public Todo GetTodoById(int todoId)
        {
            var res2 = _collection.SelectMany(u => u.Todos.Where(t => t.Id == todoId)).Select(x => x).FirstOrDefault();
            return res2;
        }

        public List<User> GetUsers()
        {
            var res2 = _collection.ToList();
            return res2;
        }
    }
}

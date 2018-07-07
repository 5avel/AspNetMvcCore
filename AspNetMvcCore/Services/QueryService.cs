
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
        (User, Post, int, int, Post, Post) GetUserById(int userId);
        (Post, Comment, Comment, int) GetPostById(int postId);

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

            var postsComments = posts.GroupJoin(comments, p => p.Id, c => c.PostId,
               (p, c) => new Post()
               {
                   Id = p.Id,
                   CreatedAt = p.CreatedAt,
                   Title = p.Title,
                   Body = p.Body,
                   UserId = p.UserId,
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
            return _collection.SingleOrDefault(x => x.Id == userId)?.Posts
                .Select(p => (p, p.Comments.Count()));
        }

        //query 2
        public IEnumerable<Comment> GetPostCommentsBodyLessThan50ByUserId(int userId)
        {
            return from u in _collection
                   where u.Id == userId
                   from p in u.Posts
                   from c in p.Comments
                   where c.Body.Length < 50
                   select c;
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
        public (User, Post, int, int, Post, Post) GetUserById(int userId)
        {
            return _collection.Where(u => u.Id == userId)
                      .Select(x =>
                      (
                          x,
                          x.Posts.OrderBy(p => p.CreatedAt).Last(),
                          x.Posts.OrderBy(p => p.CreatedAt).Last().Comments.Count(),
                          x.Todos.Count(t => t.IsComplete == false),
                          x.Posts.OrderBy(p => p.Comments.Count(c => c.Body.Length > 80)).Last(),
                          x.Posts.OrderBy(p => p.Likes).Last()
                      )).First();
        }

        //query 6
        public (Post, Comment, Comment, int) GetPostById(int postId)
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
    }
}

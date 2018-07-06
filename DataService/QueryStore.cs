using DataService.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataService
{
    public class QueryStore
    {
        private IWebClient _webClient;
        private IEnumerable<User> _collection;
        public QueryStore(IWebClient webClient)
        {
            _webClient = webClient;
            CreateAssociateEntities();
        }

        private void CreateAssociateEntities()
        {
            var users = _webClient.GetUsersList();
            var posts = _webClient.GetPostsList();
            var comments = _webClient.GetCommentsList();
            var todos = _webClient.GetTodosList();
            var address = _webClient.GetAddressList();

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

        public IEnumerable<(Post, int)> GetPostCommentsCountByUserId(int userId)
        {
            return _collection.SingleOrDefault(x => x.Id == userId).Posts
                .Select(p => (p, p.Comments.Count()));
        }

        public IEnumerable<Comment> GetPostCommentsBodyLessThan50ByUserId(int userId)
        {
            return from u in _collection
                   where u.Id == userId
                   from p in u.Posts
                   from c in p.Comments
                   where c.Body.Length < 50
                   select c;
        }

        public IEnumerable<(int, string)> GetTodoIdNameByUserId(int userId)
        {
            return from u in _collection
                   where u.Id == userId
                   from t in u.Todos
                   where t.IsComplete
                   select (t.Id, t.name);
        }

        public IEnumerable<User> GetUsetsSortByNameAndTodosSortByNameDesc()
        {
            return _collection.OrderBy(u => u.Name)
                .Select(u => 
                {
                    u.Todos = u.Todos.OrderByDescending(t => t.name.Length);
                    return u;
                });
        }

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

        public (Post, Comment, Comment, int) GetPostById(int postId)
        {
            return (from u in _collection
                    from p in u.Posts
                    where p.Id == postId
                    select
                    (
                         p,
                         p.Comments.OrderBy(c => c.Body.Length).First(),
                         p.Comments.OrderBy(c => c.Likes).First(),
                         p.Comments.Count(c => c.Likes == 0 || c.Body.Length < 80)
                     )).First();
        }

    }
}

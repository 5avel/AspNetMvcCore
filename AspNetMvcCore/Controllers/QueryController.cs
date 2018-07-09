using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcCore.Models;
using AspNetMvcCore.Services;

using Microsoft.AspNetCore.Mvc;

namespace AspNetMvcCore.Controllers
{
    public class QueryController : Controller
    {
        private IQueryService _queryService;

        public QueryController(IQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("[controller]/Query1")]
        public IActionResult Query1(int id = 0)
        {
            if (id > 0)
            {
                var query = _queryService.GetPostCommentsCountByUserId(id);
                if(query != null)
                    return View(query.ToList());
            }
            return View(new List<(Post, int)>());
        }

        [HttpGet("[controller]/Query2")]
        public IActionResult Query2(int id = 0)
        {
            if (id > 0)
            {
                var query = _queryService.GetPostCommentsBodyLessThan50ByUserId(id);
                if(query != null)
                    return View(query.ToList());
            }
            return View(new List<Comment>());
        }

        [HttpGet("[controller]/Query3")]
        public IActionResult Query3(int id = 0)
        {
            if (id > 0)
            {
                var query = _queryService.GetTodoIdNameByUserId(id);
                if(query != null)
                    return View(query.ToList());
            }
            return View(new List<(int, string)>());
        }

        [HttpGet("[controller]/Query4")]
        public IActionResult Query4()
        {
            var query = _queryService.GetUsetsSortByNameAndTodosSortByNameDesc();
            if(query != null)
                return View(query.ToList());
            
            return View(new List<User>());
        }

        [HttpGet("[controller]/Query5")]
        public IActionResult Query5(int id = 0)
        {
            if (id > 0)
            {
                var query = _queryService.Query5(id);
                return View(query);
            }
            return View(new ValueTuple<User, Post, int, int, Post, Post>(null, null, 0, 0, null, null));
        }

        [HttpGet("[controller]/Query6")]
        public IActionResult Query6(int id = 0)
        {
            if (id > 0)
            {
                var query = _queryService.GetPostById(id);
                return View(query);
            }
            return View(new ValueTuple<Post, Comment, Comment, int>(null, null, null, 0));
        }
    }
}
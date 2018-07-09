using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetMvcCore.Models;
using AspNetMvcCore.Services;

namespace AspNetMvcCore.Controllers
{
    public class HomeController : Controller
    {
        private IQueryService _queryService;
        public HomeController(IQueryService queryService)
        {
            _queryService = queryService;
        }
        public IActionResult Index()
        {
            return View(_queryService.GetUsers());
        }

        public IActionResult User(int id)
        {
            return View(_queryService.GetUserById(id));
        }

        public IActionResult Post(int id)
        {
            var post = _queryService.GetPostById(id);
            //post.User = _queryService.GetUserById(post.UserId);
            return View(post);
        }

        public IActionResult Todo(int id)
        {
            var todo = _queryService.GetTodoById(id);
            //todo.User = _queryService.GetUserById(todo.UserId);
            return View(todo);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

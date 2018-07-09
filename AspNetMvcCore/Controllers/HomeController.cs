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
            return View();
        }

        public IActionResult User(int id)
        {
            return View(_queryService.GetUserById(id));
        }

        public IActionResult Post(int id)
        {
            return View(_queryService.GetPostById(id));
        }

        public IActionResult Todo(int id)
        {
            return View(_queryService.GetTodoById(id));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

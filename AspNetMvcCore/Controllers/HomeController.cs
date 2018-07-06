using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetMvcCore.Models;
using DataService;

namespace AspNetMvcCore.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IQueryStore queryStore)
        {
            var q1 = queryStore.GetPostCommentsCountByUserId(5);
            var q2 = queryStore.GetPostCommentsBodyLessThan50ByUserId(5);
            var q3 = queryStore.GetTodoIdNameByUserId(5);
            var q4 = queryStore.GetUsetsSortByNameAndTodosSortByNameDesc();
            var q5 = queryStore.GetUserById(5);
            var q6 = queryStore.GetPostById(5);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

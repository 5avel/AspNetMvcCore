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
                var query1 = _queryService.GetPostCommentsCountByUserId(id);
                if(query1 != null)
                    return View(query1.ToList());
            }
            return View(new List<(Post, int)>());
        }
    }
}
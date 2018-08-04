using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapraiScheduler.Models;

namespace MapraiScheduler.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}

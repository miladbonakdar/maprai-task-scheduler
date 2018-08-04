using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.ApiController
{
    public class Test : Controller
    {
        [HttpPost]
        public string ForTest() => "testtttt";
    }
}

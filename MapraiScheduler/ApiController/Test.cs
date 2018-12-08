using Microsoft.AspNetCore.Mvc;

namespace MapraiScheduler.ApiController
{
    public class Test : Controller
    {
        [HttpPost]
        public string ForTest() => "testtttt";
    }
}
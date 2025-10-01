using System.Diagnostics;
using System.Text;
using Company.G02.PL.Models;
using Company.G02.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService scoped01;
        private readonly IScopedService scoped02;
        private readonly ITransientService transientService01;
        private readonly ITransientService transientService02;
        private readonly ISiingletonService siingletonService01;
        private readonly ISiingletonService siingletonService02;

        public HomeController(ILogger<HomeController> logger,
            IScopedService scoped01,
            IScopedService scoped02,
            ITransientService transientService01,
            ITransientService transientService02,
            ISiingletonService siingletonService01,
            ISiingletonService siingletonService02
            )
        {
            _logger = logger;
            this.scoped01 = scoped01;
            this.scoped02 = scoped02;
            this.transientService01 = transientService01;
            this.transientService02 = transientService02;
            this.siingletonService01 = siingletonService01;
            this.siingletonService02 = siingletonService02;
        }
        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"scoped01 :: {scoped01.GetGuid}");
            builder.Append($"scoped01 :: {scoped02.GetGuid}");
            builder.Append($"transientService01 :: {transientService01.GetGuid}");
            builder.Append($"transientService02 :: {transientService02.GetGuid}");
            builder.Append($"siingletonService01 :: {siingletonService01.GetGuid}");
            builder.Append($"siingletonService02 :: {siingletonService02.GetGuid}");

            return builder.ToString();
        }
        public IActionResult Index()
        {
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

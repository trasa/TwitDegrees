using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Blackfin.Core.Mvc.Controllers;
using TwitDegrees.Presentation.Core.Services;

namespace TwitDegrees.Web.Controllers
{
    public class PathController : SmartController
    {
        private readonly IPathService pathService;

        public PathController(IPathService pathService)
        {
            this.pathService = pathService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string start, string dest)
        {
            IEnumerable<string> path = pathService.FindPath(start, dest);
            return View(new SearchModel {StartUser = start, DestinationUser = dest, Path = path});
        }
    }

    public class SearchModel
    {
        public string StartUser { get; set; }
        public string DestinationUser { get; set; }
        public IEnumerable<string> Path { get; set; }

        public bool PathFound { get { return Path.Count() > 0; } }
        }
}

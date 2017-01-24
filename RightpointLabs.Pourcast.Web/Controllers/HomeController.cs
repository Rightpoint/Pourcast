using System.Web.Mvc;

namespace RightpointLabs.Pourcast.Web.Controllers
{
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

    public class HomeController : Controller
    {
        //private readonly ITapOrchestrator _tapOrchestrator;

        //private readonly IIdentityOrchestrator _identityOrchestrator;

        //public HomeController(ITapOrchestrator tapOrchestrator, IIdentityOrchestrator identityOrchestrator)
        //{
        //    _tapOrchestrator = tapOrchestrator;
        //    _identityOrchestrator = identityOrchestrator;
        //}

        //
        // GET: /Home/
        public ActionResult Index()
        {

            return View();
        }
    }
}
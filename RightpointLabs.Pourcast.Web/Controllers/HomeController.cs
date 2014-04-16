using System.Web.Mvc;

namespace RightpointLabs.Pourcast.Web.Controllers
{
    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

    public class HomeController : Controller
    {
        private readonly ITapOrchestrator _tapOrchestrator;

        public HomeController(ITapOrchestrator tapOrchestrator)
        {
            _tapOrchestrator = tapOrchestrator;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            _tapOrchestrator.PourBeerFromTap("534a14b1aed2bf2a00045509", 10);

            return View();
        }
	}
}
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
            // Replace this with a Mock so it doesn't blow up the app
            //_tapOrchestrator.PourBeerFromTap("534a14b1aed2bf2a00045509", .01);

            return View();
        }

        public ActionResult Split()
        {
            return View();
        }
	}
}
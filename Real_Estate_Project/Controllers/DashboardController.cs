using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.Business_services;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.ViewModels.Dashboard_View_Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Real_Estate_Project.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private const string NOT_AUTHORIZED_PATH = "RoleNotAuthorized";
        private SimpleErrorModel errorModel;
        private DashboardViewModel DashboardViewModel;
        private IDashboardService _dashboardService;

        public DashboardController
            (IDashboardService dashboardService,
            DashboardViewModel dashbaordVM)
        {
            _dashboardService = dashboardService;
            DashboardViewModel = dashbaordVM;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                if (User.IsInRole(RoleNames.AGENT))
                {
                    errorModel = new SimpleErrorModel
                    { ErrorMessage = "Agent cannot access the Dashboard resources." };

                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }

                DashboardViewModel = await _dashboardService.GetDashboardViewModel();

                return View(DashboardViewModel);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new DashboardViewModel());
            }

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _dashboardService.Dispose();
            }
        }
    }
}

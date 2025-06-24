using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Controllers
{
    [Authorize]
    [Route("InsuranceProcess")]
    public class InsuranceProcessController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly ILogger<EstimatesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public InsuranceProcessController(UserManager<ApplicationUser> userManager,ILogger<EstimatesController> logger, VehicleInsuranceManagementContext context)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpGet("CollectInfo")]
        public async Task<IActionResult> CollectInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");  // Redirect to a login page if user is not authenticated
            }
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            if (estimate == null)
            {
                return RedirectToAction("Create", "Estimate"); // or any appropriate action
            }
            var collectinfo = new CollectInfoViewModel
            {
                VehicleRate = estimate.VehicleRate,
                

            };
            return View(collectinfo);
        }

        [HttpPost("CollectInfo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CollectInfo(CollectInfoViewModel viewModel)
        {
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            if (estimate == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }
            
            if (ModelState.IsValid)
            {
                var collectinfo = new CollectInfoViewModel
                {
                    VehicleRate = estimate.VehicleRate,
                    DriverAge = viewModel.DriverAge,
                    DrivingHistory = viewModel.DrivingHistory,
                    CustomerAdd = viewModel.CustomerAdd,
                    Usage  = viewModel.Usage,
                    AntiTheftDevice = viewModel.AntiTheftDevice,
                    MultiPolicy = viewModel.MultiPolicy,
                    SafeDriver = viewModel.SafeDriver,
                    SelectedCoverages = viewModel.SelectedCoverages,
                };
                // Store the viewModel data in session
                HttpContext.Session.SetObject("CollectInfoData", collectinfo);

                // Log the session data to console
                var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
                _logger.LogInformation("Session Insurance Input Data: {@SessionData}", sessionData);

                return RedirectToAction("Create", "InsuranceProcess");
            }

            return View(viewModel);
        }

        [Route("process")]
        [HttpGet]
        public IActionResult Create()
        {
            Random random = new Random();
            string randomNumber = random.Next(10000000, 99999999).ToString();
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");

            if (estimate == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }

            var insuranceProcess = new InsuranceProcessViewModel
            {
                CustomerId = estimate.CustomerId,
                CustomerName = estimate.CustomerName,
                CustomerPhoneNumber = estimate.CustomerPhoneNumber,
                VehicleId = estimate.VehicleId,
                VehicleName = estimate.VehicleName,
                VehicleModel = estimate.VehicleModel,
                VehicleRate = estimate.VehicleRate,
                WarrantyId = estimate.WarrantyId,
                PolicyNumber = randomNumber,
                PolicyTypeId = estimate.PolicyTypeId,
                PolicyDate = DateTime.Now.ToString("yyyy-MM-dd"),
                PolicyDuration = 12, // Default duration, can be adjusted
                VehicleBodyNumber = vehicleinfo.VehicleBodyNumber,
                VehicleEngineNumber = vehicleinfo.VehicleEngineNumber
                  
            };

            return View(insuranceProcess);
        }

        [Route("process")]
        // POST: InsuranceProcess/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceProcessViewModel insuranceProcess)
        {
            Random random = new Random();
            string randomNumber = random.Next(10000000, 99999999).ToString();

            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            
            if (ModelState.IsValid)
            {
                var insurances = new InsuranceProcessViewModel
                {
                    CustomerId = insuranceProcess.CustomerId,
                    CustomerName = insuranceProcess.CustomerName,
                    CustomerPhoneNumber = insuranceProcess.CustomerPhoneNumber,
                    VehicleId = insuranceProcess.VehicleId,
                    VehicleName = insuranceProcess.VehicleName,
                    VehicleModel = insuranceProcess.VehicleModel,
                    VehicleRate = insuranceProcess.VehicleRate,
                    
                    WarrantyId = insuranceProcess.WarrantyId,
                    PolicyNumber = randomNumber,
                    PolicyTypeId = insuranceProcess.PolicyTypeId,
                    PolicyDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    PolicyDuration = 12, // Default duration, can be adjusted
                    VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
                    VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
                };

                HttpContext.Session.SetObject("InsuranceData", insurances);

                // Log the session data to console
                var sessionData = HttpContext.Session.GetObject<InsuranceProcessViewModel>("InsuranceData");
                var sessionData1 = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
                var sessionData2 = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
                

                _logger.LogInformation("Session Insurance Process: {@SessionData}", sessionData);
                _logger.LogInformation("Session Estimate: {@SessionData}", sessionData1);
                _logger.LogInformation("Session Vehicleinfo: {@SessionData}", sessionData2);
                


                return RedirectToAction("Create","CompanyBillingPolicy");
            }

            return View(insuranceProcess);
        }

        
    }
}

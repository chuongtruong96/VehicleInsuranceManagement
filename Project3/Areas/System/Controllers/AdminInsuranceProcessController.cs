using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system/insuranceprocess")]
    public class AdminInsuranceProcessController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly ILogger<AdminInsuranceProcessController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminInsuranceProcessController(UserManager<ApplicationUser> userManager, ILogger<AdminInsuranceProcessController> logger, VehicleInsuranceManagementContext context)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var insuranceProcesses = await _context.InsuranceProcesses.ToListAsync();
            var companybillingpolicies = await _context.CompanyBillingPolicies.ToListAsync();
            // Map the InsuranceProcess to InsuranceProcessViewModel
            var viewModel = insuranceProcesses.Select(ip =>
            {
            var billingPolicy = companybillingpolicies.FirstOrDefault(cb => cb.PolicyNumber == ip.PolicyNumber);
            return new InsuranceProcessViewModel
            {
                PolicyNumber = ip.PolicyNumber,
                CustomerName = ip.CustomerName,
                VehicleName = ip.VehicleName,
                PolicyDate = ip.PolicyDate,
                PolicyDuration = ip.PolicyDuration ?? 12,
                VehicleModel = ip.VehicleModel,
                VehicleRate = (float)ip.VehicleRate,
                CustomerId = ip.CustomerId,
                CustomerPhoneNumber = ip.CustomerPhoneNumber,
                VehicleId = ip.VehicleId,
                WarrantyId = ip.WarrantyId,
                PolicyTypeId = ip.PolicyTypeId,
                VehicleBodyNumber = ip.VehicleBodyNumber,
                VehicleEngineNumber = ip.VehicleEngineNumber
            };
            }).ToList();

            return View(viewModel);
        }



        [HttpGet("CollectInfo")]
        public async Task<IActionResult> CollectInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });  // Redirect to the login page if the user is not authenticated
            }
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            if (estimate == null)
            {
                return RedirectToAction("Create", "Estimate", new { area = "System" }); // or any appropriate action
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
                return RedirectToAction("Index", "Home", new { area = "System" }); // or any appropriate action
            }

            if (ModelState.IsValid)
            {
                var collectinfo = new CollectInfoViewModel
                {
                    VehicleRate = estimate.VehicleRate,
                    DriverAge = viewModel.DriverAge,
                    DrivingHistory = viewModel.DrivingHistory,
                    CustomerAdd = viewModel.CustomerAdd,
                    Usage = viewModel.Usage,
                    AntiTheftDevice = viewModel.AntiTheftDevice,
                    MultiPolicy = viewModel.MultiPolicy,
                    SafeDriver = viewModel.SafeDriver,
                };
                // Store the viewModel data in session
                HttpContext.Session.SetObject("CollectInfoData", collectinfo);

                // Log the session data to console
                var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
                _logger.LogInformation("Session Insurance Input Data: {@SessionData}", sessionData);

                return RedirectToAction("Create", "SystemInsuranceProcess", new { area = "System" });
            }

            return View(viewModel);
        }

        [Route("process")]
        [HttpGet]
        public IActionResult Create()
        {
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");

            if (estimate == null)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "System" }); // or any appropriate action
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

                return RedirectToAction("Create", "SystemCompanyBillingPolicy", new { area = "System" });
            }

            return View(insuranceProcess);
        }
        // GET: system/insuranceprocess/Edit/5
        [HttpGet("Edit/{policyNumber}")]
        public async Task<IActionResult> Edit(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return NotFound();
            }

            var insuranceProcess = await _context.InsuranceProcesses
                .FirstOrDefaultAsync(m => m.PolicyNumber == policyNumber);

            if (insuranceProcess == null)
            {
                return NotFound();
            }

            var viewModel = new InsuranceProcessViewModel
            {
                CustomerId = insuranceProcess.CustomerId,
                CustomerName = insuranceProcess.CustomerName,
                CustomerPhoneNumber = insuranceProcess.CustomerPhoneNumber,
                VehicleId = insuranceProcess.VehicleId,
                VehicleName = insuranceProcess.VehicleName,
                VehicleModel = insuranceProcess.VehicleModel,
                //VehicleRate = insuranceProcess.VehicleRate,
                WarrantyId = insuranceProcess.WarrantyId,
                PolicyNumber = insuranceProcess.PolicyNumber,
                PolicyTypeId = insuranceProcess.PolicyTypeId,
                PolicyDate = insuranceProcess.PolicyDate,
                PolicyDuration = insuranceProcess.PolicyDuration,
                VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
                VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
            };

            return View(viewModel);
        }

        // POST: system/insuranceprocess/Edit/5
        [HttpPost("Edit/{policyNumber}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string policyNumber, InsuranceProcessViewModel viewModel)
        {
            if (policyNumber != viewModel.PolicyNumber)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var insuranceProcess = await _context.InsuranceProcesses
                        .FirstOrDefaultAsync(m => m.PolicyNumber == policyNumber);

                    if (insuranceProcess == null)
                    {
                        return NotFound();
                    }

                    insuranceProcess.CustomerName = viewModel.CustomerName;
                    insuranceProcess.CustomerPhoneNumber = viewModel.CustomerPhoneNumber;
                    insuranceProcess.VehicleName = viewModel.VehicleName;
                    insuranceProcess.VehicleModel = viewModel.VehicleModel;
                    insuranceProcess.VehicleRate = viewModel.VehicleRate;
                    insuranceProcess.PolicyDate = viewModel.PolicyDate;
                    insuranceProcess.PolicyDuration = viewModel.PolicyDuration;
                    insuranceProcess.VehicleBodyNumber = viewModel.VehicleBodyNumber;
                    insuranceProcess.VehicleEngineNumber = viewModel.VehicleEngineNumber;

                    _context.Update(insuranceProcess);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));  // Redirect to a suitable action
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceProcessExists(viewModel.PolicyNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(viewModel);
        }
        // GET: system/insuranceprocess/Details/5
        [HttpGet("Details/{policyNumber}")]
        public async Task<IActionResult> Details(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return NotFound();
            }

            var insuranceProcess = await _context.InsuranceProcesses
                .FirstOrDefaultAsync(m => m.PolicyNumber == policyNumber);

            if (insuranceProcess == null)
            {
                return NotFound();
            }

            var viewModel = new InsuranceProcessViewModel
            {
                CustomerId = insuranceProcess.CustomerId,
                CustomerName = insuranceProcess.CustomerName,
                CustomerPhoneNumber = insuranceProcess.CustomerPhoneNumber,
                VehicleId = insuranceProcess.VehicleId,
                VehicleName = insuranceProcess.VehicleName,
                VehicleModel = insuranceProcess.VehicleModel,
                //VehicleRate = insuranceProcess.VehicleRate,
                WarrantyId = insuranceProcess.WarrantyId,
                PolicyNumber = insuranceProcess.PolicyNumber,
                PolicyTypeId = insuranceProcess.PolicyTypeId,
                PolicyDate = insuranceProcess.PolicyDate,
                PolicyDuration = insuranceProcess.PolicyDuration,
                VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
                VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
            };

            return View(viewModel);
        }
        // GET: system/insuranceprocess/Delete/5
        [HttpGet("Delete/{policyNumber}")]
        public async Task<IActionResult> Delete(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                return NotFound();
            }

            var insuranceProcess = await _context.InsuranceProcesses
                .FirstOrDefaultAsync(m => m.PolicyNumber == policyNumber);

            if (insuranceProcess == null)
            {
                return NotFound();
            }

            var viewModel = new InsuranceProcessViewModel
            {
                CustomerId = insuranceProcess.CustomerId,
                CustomerName = insuranceProcess.CustomerName,
                CustomerPhoneNumber = insuranceProcess.CustomerPhoneNumber,
                VehicleId = insuranceProcess.VehicleId,
                VehicleName = insuranceProcess.VehicleName,
                VehicleModel = insuranceProcess.VehicleModel,
                //VehicleRate = insuranceProcess.VehicleRate,
                WarrantyId = insuranceProcess.WarrantyId,
                PolicyNumber = insuranceProcess.PolicyNumber,
                PolicyTypeId = insuranceProcess.PolicyTypeId,
                PolicyDate = insuranceProcess.PolicyDate,
                PolicyDuration = insuranceProcess.PolicyDuration,
                VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
                VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
            };

            return View(viewModel);
        }

        // POST: system/insuranceprocess/Delete/5
        [HttpPost("Delete/{policyNumber}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string policyNumber)
        {
            var insuranceProcess = await _context.InsuranceProcesses
                .FirstOrDefaultAsync(m => m.PolicyNumber == policyNumber);

            if (insuranceProcess == null)
            {
                return NotFound();
            }

            _context.InsuranceProcesses.Remove(insuranceProcess);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));  // Redirect to a suitable action
        }

        private bool InsuranceProcessExists(string policyNumber)
        {
            return _context.InsuranceProcesses.Any(e => e.PolicyNumber == policyNumber);
        }

    }
}

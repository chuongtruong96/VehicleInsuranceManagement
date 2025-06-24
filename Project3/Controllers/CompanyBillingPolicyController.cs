using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using Project3.Services;
using Serilog;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;


namespace Project3.Controllers
{
    [Route("CompanyBillingPolicy")]
    public class CompanyBillingPolicyController : Controller
    {
        private readonly CarService _carService;
        private readonly VehicleInsuranceManagementContext _context;
        private readonly BillingCalculationService _billingCalculationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EstimatesController> _logger;
        public CompanyBillingPolicyController(ILogger<EstimatesController> logger, VehicleInsuranceManagementContext context, BillingCalculationService billingCalculationService, UserManager<ApplicationUser> userManager, CarService carService)
        {
            _context = context;
            _billingCalculationService = billingCalculationService;
            _userManager = userManager;
            _carService = carService;
            _logger = logger;
        }
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyBillingPolicies.ToListAsync());
        }
        [Route("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            return View(billingPolicy);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {


            //get user info
            //var user = await _userManager.GetUserAsync(User);

            var sessionData = HttpContext.Session.GetObject<InsuranceProcessViewModel>("InsuranceData");
            var sessionData1 = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var sessionData2 = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var collectInfoData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
            if (sessionData == null || sessionData1 == null || sessionData2 == null || collectInfoData == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }

            // Calculate the amount using the billing calculation service
            float amount = _billingCalculationService.CalculateAmount(collectInfoData);

            // Parse the PolicyDate string to DateTime
            DateTime? policyDate = null;
            if (DateTime.TryParse(sessionData.PolicyDate, out DateTime parsedDate))
            {
                policyDate = parsedDate;
            }

            var model = new CompanyBillingPolicyViewModel
            {
                CustomerId = sessionData.CustomerId,
                CustomerName = sessionData.CustomerName,
                CustomerPhoneNumber = sessionData.CustomerPhoneNumber,
                CustomerAddProve = collectInfoData.CustomerAdd,
                PolicyNumber = sessionData.PolicyNumber,

                VehicleName = sessionData.VehicleName,
                VehicleModel = sessionData.VehicleModel,
                VehicleRate = sessionData.VehicleRate,
                VehicleBodyNumber = sessionData.VehicleBodyNumber,
                VehicleEngineNumber = sessionData.VehicleEngineNumber,
                Date = policyDate,
                Amount = amount // Set the calculated amount


            };

            return View(model);
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyBillingPolicyViewModel model)
        {
            Random random = new Random();
            string randomNumber = random.Next(10000000, 99999999).ToString();
            var user = await _userManager.GetUserAsync(User);
            var collectInfoData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
            var sessionData = HttpContext.Session.GetObject<InsuranceProcessViewModel>("InsuranceData");

            DateTime? policyDate = null;
            if (DateTime.TryParse(sessionData.PolicyDate, out DateTime parsedDate))
            {
                policyDate = parsedDate;
            }

            var amount = _billingCalculationService.CalculateAmount(collectInfoData);
            model.BillNo = randomNumber;
            model.PaymentStatus = "Pending";
            if (ModelState.IsValid)
            {
                var companyBillingPolicy = new CompanyBillingPolicyViewModel
                {
                    CustomerId = user.Id,
                    CustomerName = model.CustomerName,
                    CustomerPhoneNumber = model.CustomerPhoneNumber,
                    CustomerAddProve = collectInfoData.CustomerAdd,
                    PolicyNumber = model.PolicyNumber,
                    BillNo = model.BillNo,
                    VehicleName = model.VehicleName,
                    VehicleModel = model.VehicleModel,
                    VehicleRate = model.VehicleRate,
                    VehicleBodyNumber = model.VehicleBodyNumber,
                    VehicleEngineNumber = model.VehicleEngineNumber,
                    Date = policyDate,
                    Amount = amount,
                    PaymentStatus = model.PaymentStatus
                };

                HttpContext.Session.SetObject("companyBilling", companyBillingPolicy);
                HttpContext.Session.SetString("Amount", amount.ToString(CultureInfo.InvariantCulture));

                return RedirectToAction("Payment");

                //var companysession = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");
                //var amountdata = HttpContext.Session.GetObject<CompanyBillingPolicy>("Amount");
                //_logger.LogInformation("Session Insurance Process: {@SessionData}", sessionData);
                //_logger.LogInformation("Session Insurance Process: {@SessionData}", sessionData);
            }
            else
            {
                // Log the validation errors for debugging
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogWarning("Validation error in {Key}: {Errors}", state.Key, string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage)));
                    }
                }
                return View(model);
            }
        }

        public IActionResult Payment()
        {
            var companysession = HttpContext.Session.GetObject<CompanyBillingPolicy>("companyBilling");

            _logger.LogInformation("Session Insurance Process: {@SessionData}", companysession);

            return View();
        }

        //     3. **Edit Action(GET)**:
        //- Fetches the billing policy by ID.
        //- Prepares the view model with all necessary data, including the fields for recalculating the amount.


        //[Route("edit")]
        //public async Task<IActionResult> Edit(int? id, CollectInfoViewModel viewCollectInfo)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");

        //    var cars = await _carService.GetAllCarsAsync();
        //    ViewBag.Manufacturers = cars.Select(c => new { c.Code, c.Name }).ToList();

        //    var viewModel = new CompanyBillingPolicyViewModel
        //    {
        //        VehicleName = sessionData.VehicleName,
        //        VehicleModel = sessionData.VehicleModel,
        //        VehicleVersion = sessionData.VehicleVersion,
        //        VehicleRate = sessionData.VehicleRate,
        //        VehicleBodyNumber = sessionData.VehicleBodyNumber,
        //        VehicleEngineNumber = sessionData.VehicleEngineNumber,
        //        DriverAge = sessionData.DriverAge,
        //        DriverGender = sessionData.DriverGender,
        //        DrivingHistory = sessionData.DrivingHistory,
        //        CustomerAdd = sessionData.CustomerAdd,
        //        Usage = sessionData.Usage,
        //        AntiTheftDevice = sessionData.AntiTheftDevice,
        //        MultiPolicy = sessionData.MultiPolicy,
        //        SafeDriver = sessionData.SafeDriver,
        //    };


        //    return View(viewModel);
        //}
        //[Route("edit")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, CompanyBillingPolicyViewModel model)
        //{
        //    var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("AmountCalData");
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var insurances = new InsuranceProcess
        //            {

        //                VehicleId = insuranceProcess.VehicleId,
        //                VehicleName = insuranceProcess.VehicleName,
        //                VehicleModel = insuranceProcess.VehicleModel,
        //                VehicleVersion = insuranceProcess.VehicleVersion,
        //                VehicleRate = insuranceProcess.VehicleRate,
        //                VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
        //                VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
        //            };

        //            HttpContext.Session.SetObject("InsuranceData", insurances);

        //            // Recalculate the amount
        //            var collectInfoData = new CompanyBillingPolicy
        //            {
        //                VehicleRate = model.VehicleRate,
        //                Amount = _billingCalculationService.CalculateAmount();
        //            };


        //            _context.Update(collectInfoData);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BillingPolicyExists(model.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}
        private bool BillingPolicyExists(int id)
        {
            return _context.CompanyBillingPolicies.Any(e => e.Id == id);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            return View(billingPolicy);
        }
        [Route("delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billingPolicy = await _context.CompanyBillingPolicies.FindAsync(id);
            _context.CompanyBillingPolicies.Remove(billingPolicy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Route("PaymentFail")]
        public IActionResult PaymentFail()
        {
            return View();
        }

        
    }
}

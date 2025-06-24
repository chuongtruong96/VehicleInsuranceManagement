using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using System.Linq;
using System.Threading.Tasks;
using Project3.ModelsView;
using Microsoft.AspNetCore.Identity;
using Project3.ModelsView.Identity;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using System.Web.Providers.Entities;
namespace Project3.Controllers
{
    [Authorize]
    [Route("Estimates")]
    public class EstimatesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EstimatesController> _logger;
        public EstimatesController(UserManager<ApplicationUser> userManager, VehicleInsuranceManagementContext context, ILogger<EstimatesController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }


        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var sessionData = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var user = await _userManager.GetUserAsync(User);
            var productSession = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch the policy type and warranty information
            var policyType = await _context.VehiclePolicyTypes.FindAsync(productSession.PolicyTypeId);
            var warranty = await _context.VehicleWarranties.FindAsync(productSession.WarrantyId);

            // Ensure these properties are not null before passing to the view
            if (policyType == null || warranty == null)
            {
                ModelState.AddModelError(string.Empty, "Policy Type or Warranty details could not be found.");
                return View();
            }

            var viewModel = new EstimateModelView
            {
                CustomerId = user.Id,
                CustomerName = user.Fullname,
                CustomerPhoneNumber = user.Phone,
                VehicleId = sessionData.Id,
                VehicleName = sessionData.VehicleName,
                VehicleModel = sessionData.VehicleModel,
                VehicleVersion = sessionData.VehicleVersion,
                VehicleRate = sessionData.VehicleRate,
                PolicyTypeId = productSession.PolicyTypeId,
                PolicyTypeName = policyType.PolicyName,  // Set policy type name
                PolicyTypeDetails = policyType.PolicyDetails,  // Set policy type details
                WarrantyId = productSession.WarrantyId,
                WarrantyType = warranty.WarrantyType,  // Set warranty type
                WarrantyDetails = warranty.WarrantyDetails  // Set warranty details
            };

            _logger.LogInformation("Warranty Type: {WarrantyType}, Warranty Details: {WarrantyDetails}", viewModel.WarrantyType, viewModel.WarrantyDetails);
            _logger.LogInformation("Policy Type Name: {PolicyTypeName}, Policy Type Details: {PolicyTypeDetails}", viewModel.PolicyTypeName, viewModel.PolicyTypeDetails);

            return View(viewModel);
        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstimateModelView viewModel)
        {
            
            
            var user = await _userManager.GetUserAsync(User);
             if (ModelState.IsValid)
            {
                try
                {
                    var sessionData = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
                    var productSession = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");
                    var policyType = await _context.VehiclePolicyTypes.FindAsync(productSession.PolicyTypeId);
                    var warranty = await _context.VehicleWarranties.FindAsync(productSession.WarrantyId);
                    // This line checks if the PolicyTypeId exists in the database, but the result is not used
                    viewModel.HasActivePolicies = await _context.VehiclePolicyTypes.AnyAsync(c => c.PolicyTypeId == viewModel.PolicyTypeId);

                    var estimate = new EstimateModelView
                    {
                        CustomerId = user.Id,
                        CustomerName = user.Fullname,
                        CustomerPhoneNumber = user.Phone,
                        VehicleId = sessionData.Id,
                        VehicleName = sessionData.VehicleName,
                        VehicleModel = sessionData.VehicleModel,
                        VehicleVersion = sessionData.VehicleVersion,
                        VehicleRate = sessionData.VehicleRate,
                        PolicyTypeId = productSession.PolicyTypeId,
                        PolicyTypeName = policyType.PolicyName,  // Set policy type name
                        PolicyTypeDetails = policyType.PolicyDetails,  // Set policy type details
                        WarrantyId = productSession.WarrantyId,
                        WarrantyType = warranty.WarrantyType,  // Set warranty type
                        WarrantyDetails = warranty.WarrantyDetails
                    };
                    
                    // Log successful creation
                    _logger.LogInformation("Estimate created successfully with ID {EstimateId}", estimate.EstimateNumber);

                    // Save estimate data into session
                    HttpContext.Session.SetObject("EstimateData", estimate);

                    // Log the session data to console
                    
                    _logger.LogInformation("Session Estimate: {@SessionData}", sessionData);

                    // Redirect to InsuranceProcess Create view
                    return RedirectToAction("CollectInfo", "InsuranceProcess");
                }
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError(ex, "Error occurred while creating estimate");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the estimate.");
                }
            }
            else
            {
                // Log validation errors
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Any())
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogWarning("ModelState Error: {Key} - {ErrorMessage}", state.Key, error.ErrorMessage);
                        }
                    }
                }
            }

            return View(viewModel);
        }



        private bool EstimateExists(int id)
        {
            return _context.Estimates.Any(e => e.EstimateNumber == id);
        }
    }
}

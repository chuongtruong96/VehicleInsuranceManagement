using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("System")]
    [Route("System/InsuranceProduct")]
    public class AdminInsuranceProductsController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public AdminInsuranceProductsController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            // Fetch all policy types and warranties separately
            var policyTypes = await _context.VehiclePolicyTypes.ToListAsync();
            var warranties = await _context.VehicleWarranties.ToListAsync();

            // Combine the data into a list of view models
            var insuranceProducts = policyTypes.Select(policy =>
            {
                // Match warranties based on some other criteria
                var matchingWarranty = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policy.PolicyName));

                return new InsuranceProductViewModel
                {
                    PolicyTypeId = policy.PolicyTypeId,
                    PolicyName = policy.PolicyName,
                    PolicyDetails = policy.PolicyDetails,
                    VehicleRate = (float)(policy.VehicleRate ?? 0),
                    WarrantyId = matchingWarranty?.WarrantyId ?? 0,
                    WarrantyType = matchingWarranty?.WarrantyType,
                    WarrantyDuration = matchingWarranty?.WarrantyDuration,
                    WarrantyDetails = matchingWarranty?.WarrantyDetails
                };
            }).ToList();

            return View(insuranceProducts);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // Fetch all policy types and warranties from the database
            var policyTypes = await _context.VehiclePolicyTypes.ToListAsync();
            var warranties = await _context.VehicleWarranties.ToListAsync();

            // This could be replaced by a ViewModel to combine data from both tables
            var viewModel = new InsuranceProductViewModel
            {
                // Initialize the properties based on the fetched data
                PolicyTypeId = policyTypes.FirstOrDefault()?.PolicyTypeId ?? 0,
                PolicyName = policyTypes.FirstOrDefault()?.PolicyName,
                PolicyDetails = policyTypes.FirstOrDefault()?.PolicyDetails,
                WarrantyId = warranties.FirstOrDefault()?.WarrantyId ?? 0,
                WarrantyType = warranties.FirstOrDefault()?.WarrantyType,
                WarrantyDuration = warranties.FirstOrDefault()?.WarrantyDuration,
                WarrantyDetails = warranties.FirstOrDefault()?.WarrantyDetails,
                VehicleRate = (float)(policyTypes.FirstOrDefault()?.VehicleRate ?? 0)
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add logic to save the new policy or warranty, if that's the intent
                    var policyType = new VehiclePolicyType
                    {
                        PolicyName = viewModel.PolicyName,
                        PolicyDetails = viewModel.PolicyDetails,
                        VehicleRate = viewModel.VehicleRate
                    };

                    _context.VehiclePolicyTypes.Add(policyType);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    // Handle exceptions if any
                }
            }

            return View(viewModel);
        }

        // GET: System/InsuranceProducts/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType == null)
            {
                return NotFound();
            }

            // Fetch all warranties (you can apply more specific logic as needed)
            var warranties = await _context.VehicleWarranties.ToListAsync();

            var viewModel = new InsuranceProductViewModel
            {
                PolicyTypeId = policyType.PolicyTypeId,
                PolicyName = policyType.PolicyName,
                PolicyDetails = policyType.PolicyDetails,
                WarrantyId = warranties.FirstOrDefault()?.WarrantyId ?? 0,
                WarrantyType = warranties.FirstOrDefault()?.WarrantyType,
                WarrantyDuration = warranties.FirstOrDefault()?.WarrantyDuration,
                WarrantyDetails = warranties.FirstOrDefault()?.WarrantyDetails,
                VehicleRate = (float)policyType.VehicleRate
            };

            return View(viewModel);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InsuranceProductViewModel viewModel)
        {
            if (id != viewModel.PolicyTypeId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
                if (policyType == null)
                {
                    return NotFound();
                }

                policyType.PolicyName = viewModel.PolicyName;
                policyType.PolicyDetails = viewModel.PolicyDetails;
                policyType.VehicleRate = viewModel.VehicleRate;

                _context.Update(policyType);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);

            if (policyType != null)
            {
                _context.VehiclePolicyTypes.Remove(policyType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            // Find the policy type by its ID
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType == null)
            {
                return NotFound();
            }

            // Fetch the associated warranty details (or all warranties, if needed)
            var warranties = await _context.VehicleWarranties.ToListAsync();
            var matchingWarranty = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policyType.PolicyName));

            // Create a view model to pass the details to the view
            var viewModel = new InsuranceProductViewModel
            {
                PolicyTypeId = policyType.PolicyTypeId,
                PolicyName = policyType.PolicyName,
                PolicyDetails = policyType.PolicyDetails,
                WarrantyId = matchingWarranty?.WarrantyId ?? 0,
                WarrantyType = matchingWarranty?.WarrantyType,
                WarrantyDuration = matchingWarranty?.WarrantyDuration,
                WarrantyDetails = matchingWarranty?.WarrantyDetails,
                VehicleRate = (float)(policyType.VehicleRate ?? 0)
            };

            // Pass the view model to the view
            return View(viewModel);
        }

        private bool PolicyTypeExists(int id)
        {
            return _context.VehiclePolicyTypes.Any(e => e.PolicyTypeId == id);
        }
    }
}

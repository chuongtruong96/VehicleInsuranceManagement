    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Project3.Models;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system/estimates")]
    public class AdminEstimatesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly ILogger<AdminEstimatesController> _logger;

        public AdminEstimatesController(VehicleInsuranceManagementContext context, ILogger<AdminEstimatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var estimates = await _context.Estimates.Include(e => e.PolicyType)
                                        .Include(e => e.Warranty)
                                        .ToListAsync();
            return View(estimates);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var estimate = await _context.Estimates.Include(e => e.PolicyType)
                                                   .Include(e => e.Warranty)
                                                   .FirstOrDefaultAsync(e => e.EstimateNumber == id);

            if (estimate == null)
            {
                return NotFound();
            }

            return View(estimate);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var policytypename = await _context.VehiclePolicyTypes.ToListAsync();
            var warrantyname = await _context.VehicleWarranties.ToListAsync();
            ViewData["PolicyTypes"] = new SelectList(_context.VehiclePolicyTypes, "PolicyTypeId", "PolicyName");
            ViewData["Warranties"] = new SelectList(_context.VehicleWarranties, "WarrantyId", "WarrantyType");
            ViewData["Vehicles"] = new SelectList(_context.VehicleInformations, "Id", "VehicleName");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estimate);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Admin created a new estimate with ID {EstimateId}", estimate.EstimateNumber);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PolicyTypes"] = new SelectList(_context.VehiclePolicyTypes, "PolicyTypeId", "PolicyName", estimate.PolicyTypeId);
            ViewData["Warranties"] = new SelectList(_context.VehicleWarranties, "WarrantyId", "WarrantyType", estimate.WarrantyId);
            ViewData["Vehicles"] = new SelectList(_context.VehicleInformations, "Id", "VehicleName", estimate.VehicleId);
            return View(estimate);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var estimate = await _context.Estimates.FindAsync(id);
            if (estimate == null)
            {
                return NotFound();
            }
            ViewData["PolicyTypes"] = new SelectList(_context.VehiclePolicyTypes, "PolicyTypeId", "PolicyName", estimate.PolicyTypeId);
            ViewData["Warranties"] = new SelectList(_context.VehicleWarranties, "WarrantyId", "WarrantyType", estimate.WarrantyId);
            ViewData["Vehicles"] = new SelectList(_context.VehicleInformations, "Id", "VehicleName", estimate.VehicleId);
            return View(estimate);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Estimate estimate)
        {
            if (id != estimate.EstimateNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estimate);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Admin edited estimate with ID {EstimateId}", estimate.EstimateNumber);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstimateExists(estimate.EstimateNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PolicyTypes"] = new SelectList(_context.VehiclePolicyTypes, "PolicyTypeId", "PolicyName", estimate.PolicyTypeId);
            ViewData["Warranties"] = new SelectList(_context.VehicleWarranties, "WarrantyId", "WarrantyType", estimate.WarrantyId);
            ViewData["Vehicles"] = new SelectList(_context.VehicleInformations, "Id", "VehicleName", estimate.VehicleId);
            return View(estimate);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var estimate = await _context.Estimates.Include(e => e.PolicyType)
                                                   .Include(e => e.Warranty)
                                                   .FirstOrDefaultAsync(e => e.EstimateNumber == id);

            if (estimate == null)
            {
                return NotFound();
            }

            return View(estimate);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estimate = await _context.Estimates.FindAsync(id);
            if (estimate != null)
            {
                _context.Estimates.Remove(estimate);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Admin deleted estimate with ID {EstimateId}", id);
            }
            else
            {
                // Handle the case where the estimate doesn't exist
                if (!EstimateExists(id))
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EstimateExists(int id)
        {
            return _context.Estimates.Any(e => e.EstimateNumber == id);
        }
    }
}

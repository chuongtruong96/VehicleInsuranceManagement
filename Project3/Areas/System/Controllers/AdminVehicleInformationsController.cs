using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using System.Threading.Tasks;

namespace Project3.Areas.System.Controllers {
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
[Route("system/vehicleinformation")]
public class AdminVehicleInformationsController : Controller
{
    private readonly VehicleInsuranceManagementContext _context;

        public AdminVehicleInformationsController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.VehicleInformations.ToListAsync();
            return View(vehicles);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleInformation model)
        {
            if (ModelState.IsValid)
            {
                _context.VehicleInformations.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _context.VehicleInformations.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleInformation model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.VehicleInformations.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Temporarily disable foreign key checks
                await _context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS=0");

                var vehicle = await _context.VehicleInformations.FindAsync(id);
                if (vehicle != null)
                {
                    _context.VehicleInformations.Remove(vehicle);
                    await _context.SaveChangesAsync();
                }

                // Re-enable foreign key checks
                await _context.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS=1");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the deletion process
                // Log the error or return an error message
                return BadRequest("An error occurred while deleting the vehicle.");
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var vehicle = await _context.VehicleInformations.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle == null)
        {
            return NotFound();
        }
        return View(vehicle);
    }
}
}
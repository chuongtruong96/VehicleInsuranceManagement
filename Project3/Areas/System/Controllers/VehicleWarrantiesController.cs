using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;

namespace Project3.Areas.System.Controllers
{
    [Area("system")]
    [Route("system/vehiclewarranty")]
    public class VehicleWarrantiesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public VehicleWarrantiesController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }
        [Route("index")]
        [HttpGet]
        // GET: System/VehicleWarranties
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleWarranties.ToListAsync());
        }

        // GET: System/VehicleWarranties/Details/5
        [Route("details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleWarranty = await _context.VehicleWarranties
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (vehicleWarranty == null)
            {
                return NotFound();
            }

            return View(vehicleWarranty);
        }

        // GET: System/VehicleWarranties/Create
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: System/VehicleWarranties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarrantyId,WarrantyType,WarrantyDuration,WarrantyDetails")] VehicleWarranty vehicleWarranty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleWarranty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleWarranty);
        }

        // GET: System/VehicleWarranties/Edit/5
        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleWarranty = await _context.VehicleWarranties.FindAsync(id);
            if (vehicleWarranty == null)
            {
                return NotFound();
            }
            return View(vehicleWarranty);
        }

        // POST: System/VehicleWarranties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WarrantyId,WarrantyDuration,WarrantyDetails")] VehicleWarranty vehicleWarranty)
        {
            if (id != vehicleWarranty.WarrantyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the current VehicleWarranty from the database
                    var existingWarranty = await _context.VehicleWarranties.FindAsync(id);

                    if (existingWarranty == null)
                    {
                        return NotFound();
                    }

                    // Update only the fields that are allowed to be edited
                    existingWarranty.WarrantyDuration = vehicleWarranty.WarrantyDuration;
                    existingWarranty.WarrantyDetails = vehicleWarranty.WarrantyDetails;

                    _context.Update(existingWarranty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleWarrantyExists(vehicleWarranty.WarrantyId))
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
            return View(vehicleWarranty);
        }


        // GET: System/VehicleWarranties/Delete/5
        [Route("delete/{id}")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleWarranty = await _context.VehicleWarranties
                .FirstOrDefaultAsync(m => m.WarrantyId == id);
            if (vehicleWarranty == null)
            {
                return NotFound();
            }

            return View(vehicleWarranty);
        }

        // POST: System/VehicleWarranties/Delete/5
        [Route("delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleWarranty = await _context.VehicleWarranties.FindAsync(id);
            if (vehicleWarranty != null)
            {
                _context.VehicleWarranties.Remove(vehicleWarranty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleWarrantyExists(int id)
        {
            return _context.VehicleWarranties.Any(e => e.WarrantyId == id);
        }
    }
}

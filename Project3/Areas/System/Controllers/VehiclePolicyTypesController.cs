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
    [Route("system/vehiclepolicytypes")]
    public class VehiclePolicyTypesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public VehiclePolicyTypesController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }
        [Route("index")]
        [HttpGet]
        // GET: System/VehiclePolicyTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehiclePolicyTypes.ToListAsync());
        }

        // GET: System/VehiclePolicyTypes/Details/5
        [Route("details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiclePolicyType = await _context.VehiclePolicyTypes
                .FirstOrDefaultAsync(m => m.PolicyTypeId == id);
            if (vehiclePolicyType == null)
            {
                return NotFound();
            }

            return View(vehiclePolicyType);
        }
        [Route("create")]
        // GET: System/VehiclePolicyTypes/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: System/VehiclePolicyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyTypeId,PolicyName,PolicyDetails,VehicleRate")] VehiclePolicyType vehiclePolicyType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehiclePolicyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehiclePolicyType);
        }

        [Route("edit/{id}")]
        [HttpGet]
        // GET: System/VehiclePolicyTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiclePolicyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (vehiclePolicyType == null)
            {
                return NotFound();
            }
            return View(vehiclePolicyType);
        }

        // POST: System/VehiclePolicyTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PolicyTypeId,PolicyName,PolicyDetails,VehicleRate")] VehiclePolicyType vehiclePolicyType)
        {
            if (id != vehiclePolicyType.PolicyTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiclePolicyType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiclePolicyTypeExists(vehiclePolicyType.PolicyTypeId))
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
            return View(vehiclePolicyType);
        }
        [Route("delete/{id}")]
        [HttpGet]
        // GET: System/VehiclePolicyTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiclePolicyType = await _context.VehiclePolicyTypes
                .FirstOrDefaultAsync(m => m.PolicyTypeId == id);
            if (vehiclePolicyType == null)
            {
                return NotFound();
            }

            return View(vehiclePolicyType);
        }

        // POST: System/VehiclePolicyTypes/Delete/5
        [Route("delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiclePolicyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (vehiclePolicyType != null)
            {
                _context.VehiclePolicyTypes.Remove(vehiclePolicyType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiclePolicyTypeExists(int id)
        {
            return _context.VehiclePolicyTypes.Any(e => e.PolicyTypeId == id);
        }
    }
}

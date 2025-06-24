using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system/companybilling")]
    public class CompanyBillingPoliciesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public CompanyBillingPoliciesController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }
        [Route("index")]
        // GET: System/CompanyBillingPolicies
        public async Task<IActionResult> Index()
        {
            var vehicleInsuranceManagementContext = _context.CompanyBillingPolicies.Include(c => c.Customer);
            return View(await vehicleInsuranceManagementContext.ToListAsync());
        }
        [Route("details")]
        // GET: System/CompanyBillingPolicies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyBillingPolicy = await _context.CompanyBillingPolicies
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyBillingPolicy == null)
            {
                return NotFound();
            }

            return View(companyBillingPolicy);
        }
        [Route("create")]
        // GET: System/CompanyBillingPolicies/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: System/CompanyBillingPolicies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,CustomerName,PolicyNumber,CustomerAddProve,CustomerPhoneNumber,BillNo,VehicleName,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,Date,Amount,PaymentStatus")] CompanyBillingPolicy companyBillingPolicy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyBillingPolicy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.AspNetUsers, "Id", "Id", companyBillingPolicy.CustomerId);
            return View(companyBillingPolicy);
        }

        // GET: System/CompanyBillingPolicies/Edit/5
        [Route("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyBillingPolicy = await _context.CompanyBillingPolicies.FindAsync(id);
            if (companyBillingPolicy == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.AspNetUsers, "Id", "Id", companyBillingPolicy.CustomerId);
            return View(companyBillingPolicy);
        }

        // POST: System/CompanyBillingPolicies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,CustomerName,PolicyNumber,CustomerAddProve,CustomerPhoneNumber,BillNo,VehicleName,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,Date,Amount,PaymentStatus")] CompanyBillingPolicyViewModel companyBillingPolicy)
        {
            if (id != companyBillingPolicy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var companybill = new CompanyBillingPolicy
                    {
                        CustomerName = companyBillingPolicy.CustomerName,
                        CustomerAddProve = companyBillingPolicy.CustomerAddProve,
                        CustomerId = companyBillingPolicy.CustomerId,
                        CustomerPhoneNumber = companyBillingPolicy.CustomerPhoneNumber,
                        VehicleName = companyBillingPolicy.VehicleName,
                        VehicleModel = companyBillingPolicy.VehicleModel,
                        VehicleBodyNumber = companyBillingPolicy.VehicleBodyNumber,
                        Amount = companyBillingPolicy.Amount,

                    };

                    TempData["SuccessMessage"] = "Create new success !!";
                    _context.Update(companyBillingPolicy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyBillingPolicyExists(companyBillingPolicy.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.AspNetUsers, "Id", "Id", companyBillingPolicy.CustomerId);
            return View(companyBillingPolicy);
        }

        // GET: System/CompanyBillingPolicies/Delete/5
        [Route("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyBillingPolicy = await _context.CompanyBillingPolicies
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyBillingPolicy == null)
            {
                return NotFound();
            }

            return View(companyBillingPolicy);
        }

        // POST: System/CompanyBillingPolicies/Delete/5
        [Route("delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyBillingPolicy = await _context.CompanyBillingPolicies.FindAsync(id);
            if (companyBillingPolicy != null)
            {
                _context.CompanyBillingPolicies.Remove(companyBillingPolicy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyBillingPolicyExists(int id)
        {
            return _context.CompanyBillingPolicies.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using System.Threading.Tasks;

namespace Project3.Areas.System.Controllers
{

    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
[Route("system/companybilling")]
public class CompanyExpensesController : Controller
{
    private readonly VehicleInsuranceManagementContext _context;

    public CompanyExpensesController(VehicleInsuranceManagementContext context)
    {
        _context = context;
    }

    // GET: CompanyExpenses
    public async Task<IActionResult> Index()
    {
        return View(await _context.CompanyExpenses.ToListAsync());
    }

    // GET: CompanyExpenses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companyExpense = await _context.CompanyExpenses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (companyExpense == null)
        {
            return NotFound();
        }

        return View(companyExpense);
    }

    // GET: CompanyExpenses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CompanyExpenses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CompanyExpense companyExpense)
    {
        if (ModelState.IsValid)
        {
            _context.Add(companyExpense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(companyExpense);
    }

    // GET: CompanyExpenses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companyExpense = await _context.CompanyExpenses.FindAsync(id);
        if (companyExpense == null)
        {
            return NotFound();
        }
        return View(companyExpense);
    }

    // POST: CompanyExpenses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CompanyExpense companyExpense)
    {
        if (id != companyExpense.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(companyExpense);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExpenseExists(companyExpense.Id))
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
        return View(companyExpense);
    }

    // GET: CompanyExpenses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companyExpense = await _context.CompanyExpenses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (companyExpense == null)
        {
            return NotFound();
        }

        return View(companyExpense);
    }

    // POST: CompanyExpenses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var companyExpense = await _context.CompanyExpenses.FindAsync(id);
        _context.CompanyExpenses.Remove(companyExpense);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CompanyExpenseExists(int id)
    {
        return _context.CompanyExpenses.Any(e => e.Id == id);
    }
}
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("System")]
    [Route("System/ContactAdmin")]
    public class ContactUsAdminController : Controller
    {
        private static int _emailSuccessCount = 0;

        private readonly VehicleInsuranceManagementContext _context;
        private readonly IMailService _mailService;
        public ContactUsAdminController(VehicleInsuranceManagementContext context, IMailService _MailService)
        {
            _context = context;
            _mailService = _MailService;
        }

        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactUs.ToListAsync());
        }   
        [Route("delete")]
        public IActionResult delete(int id)
        {
            var list = _context.ContactUs.Find(id);
            if (list != null)
            {
                _context.ContactUs.Remove(list);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Index");

            }

        }



        [Route("CreateMail")]
        public IActionResult CreateMail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var list = _context.ContactUs.Find(id);
            if (list == null)
            {

                return RedirectToAction("Index");

            }
            return View(list);
        }
        [Route("CreateMail")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMail(ContactUs mailData)
        {

            if (ModelState.IsValid)
            {
                _mailService.SendMail(mailData);
                mailData.IsReplied = true;
                TempData["EmailSuccess"] = "Send Mail Success";
                _context.Update(mailData);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(Index);



        }

        public IActionResult Success()
        {
            return View();
        }

        private bool ContactUsExists(int id)
        {
            return _context.ContactUs.Any(e => e.Id == id);
        }
    }
}





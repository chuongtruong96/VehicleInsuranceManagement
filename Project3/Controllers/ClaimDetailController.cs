//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using NuGet.Protocol.Resources;
//using Project3.Models;
//using Project3.ModelsView;
//using Project3.ModelsView.Identity;
//using System.Data.Entity;
//using static System.Net.WebRequestMethods;

//namespace Project3.Controllers
//{
//    public class ClaimDetailController : Controller
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly VehicleInsuranceManagementContext _context;


//        public ClaimDetailController(UserManager<ApplicationUser> userManager, VehicleInsuranceManagementContext context)
//        {
//            _userManager = userManager;
//            _context = context;
//        }


//        public async Task<IActionResult> Create()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound("User not found");
//            }

//            // Trước tiên, lấy dữ liệu từ cơ sở dữ liệu một cách bất đồng bộ
//            var billingInfo = await _context.CompanyBillingPolicies
//                .Where(b => b.CustomerId == user.Id)
//                .ToListAsync(); // Lấy toàn bộ dữ liệu trước khi sử dụng Select

//            // Sau khi dữ liệu đã được tải, bạn có thể chuyển đổi thành ViewModel
//            var billingInfoViewModel = billingInfo.Select(b => new ClaimDetailViewModel
//            {
//                PolicyNumber = b.PolicyNumber,
//                Id = b.Id
//            }).ToList();

//            if (!billingInfoViewModel.Any())
//            {
//                return NotFound("Billing information not found");
//            }

//            return View(billingInfoViewModel);
//        }


//        public async Task<IActionResult> Detail(int id)
//        {
//            var claim = await _context.CompanyBillingPolicies.FindAsync(id);
//            if (claim == null)
//            {
//                return NotFound("Claim not found");
//            }

//            // Trả về view với dữ liệu của claim
//            return View(claim);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Detail(ClaimDetailViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var claimDetail = new ClaimDetail
//                {
//                    PolicyNumber = model.PolicyNumber,
//                    CustomerName = model.CustomerName,
//                    PlaceOfAccident = model.PlaceOfAccident,
//                    DateOfAccident = model.DateOfAccident,
//                    InsuredAmount = model.Amount, // Giả sử InsuredAmount là số tiền bảo hiểm trong bảng ClaimDetail
//                    ClaimableAmount = CalculateClaimableAmount(model.Amount), // Tính toán số tiền được bồi thường nếu cần
//                    PolicyStartDate = model.Date,
//                   // PolicyEndDate = model.PolicyEndDate,
//                };

//                // Thêm bản ghi vào bảng ClaimDetail
//                _context.ClaimDetails.Add(claimDetail);
//                await _context.SaveChangesAsync();

//                return RedirectToAction("Create");
//            }

//            // Nếu có lỗi, hiển thị lại form với lỗi
//            return View(model);
//        }

//        // Ví dụ về phương thức để tính toán số tiền được bồi thường (nếu có logic)
//        private decimal? CalculateClaimableAmount(decimal? amount)
//        {
//            // Tính toán số tiền được bồi thường theo logic cụ thể
//            return amount * 0.8m; // Ví dụ: 80% của số tiền bảo hiểm
//        }
//    }
//}






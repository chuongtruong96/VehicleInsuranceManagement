using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project3.ModelsView.Identity;

namespace Project3.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            //check xem user login bằng gì thông qua GetLoginsAsync (bảng AspNetUserLogins)
            var logins = await _userManager.GetLoginsAsync(user);
            var isGoogleLogin = logins.Any(l => l.LoginProvider == "Google");
            var profile = new ProfileViewModel  
            {
                Fullname = user.Fullname,
                Email = user.Email,
                Phone = user.Phone,
                IsGoogleLogin = isGoogleLogin
            };
            return View(profile);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Xác minh mật khẩu hiện tại
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError("CurrentPassword", "The current password is incorrect.");
                return View(model);
            }

            // Thay đổi mật khẩu
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["ChangePasswordSuccess"] = "Your password has been changed successfully.";
                return RedirectToAction("Index", "Profile");
            }

            foreach (var error in result.Errors)
            {
                if (error.Code == "PasswordTooShort")
                {
                    ModelState.AddModelError("PasswordTooShort", error.Description);
                }
                else if (error.Code == "PasswordRequiresLower")
                {
                    ModelState.AddModelError("PasswordRequiresLower", error.Description);
                }
                else
                {
                    // Thêm lỗi vào ModelState để hiển thị cho người dùng.
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var profile = new ProfileViewModel
            {
                Fullname = user.Fullname,

                Phone = user.Phone
            };
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Fullname = model.Fullname;
            user.Phone = model.Phone;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["ProfileSuccessMessage"] = "Your profile has been updated successfully.";
                return RedirectToAction("Index", "Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

    }
}

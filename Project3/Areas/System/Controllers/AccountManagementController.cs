using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView.Admin;
using Project3.ModelsView.Identity;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system/account")]
    public class AccountManagementController : Controller
    {
        VehicleInsuranceManagementContext db = new VehicleInsuranceManagementContext();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountManagementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả người dùng
            var users = await _userManager.Users.ToListAsync();

            // Lấy tất cả vai trò từ AspNetUserRoles và NameRole
            //join 2 bảng AspNetUserRoles và NameRoles lần lượt lấy ra RoleId và Id , RoleId = Id
            //chọn UserId từ bảng AspNetUserRoles và NameRole1 từ bảng NameRole để tạo ra một đối tượng ẩn danh chứa thông tin này
            var userRoles = await (from ur in db.AspNetUserRoles
                                   join nr in db.NameRoles on ur.RoleId equals nr.Id
                                   select new
                                   {
                                       UserId = ur.UserId,
                                       RoleName = nr.NameRole1
                                   }).ToListAsync();

            // Tạo danh sách UserViewModel
            var userViewModels = users.Select(user => new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.Fullname,
                Phone = user.Phone,
                //nếu UserId từ bảng AspNetUserRoles = với Id từ bảng NameRole thì lấy danh sách NameRole ra , trừ Admin 
                Roles = userRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleName).ToList()
            }).Where(u => !u.Roles.Contains("Admin")).ToList();

            return View(userViewModels);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Lấy tất cả các vai trò trừ vai trò "Admin"
            var roles = await db.NameRoles
                .Where(r => r.NameRole1 != "Admin") // Lọc để loại bỏ vai trò "Admin"
                .ToListAsync();

            // Chuyển danh sách vai trò vào ViewBag để hiển thị ở dropdownlist
            ViewBag.Role = roles;

            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountAdminViewModel register)
        {

            if (ModelState.IsValid)
            {

                var roleExists = await db.NameRoles.AnyAsync(r => r.Id == register.RoleId);
                if (!roleExists)
                {
                    ModelState.AddModelError("RoleId", "Confirm password does not match!");
                    ViewBag.Role = await db.NameRoles.ToListAsync();
                    return View(register);
                }
                // Kiểm tra xem email đã tồn tại hay chưa
                var existingUser = await _userManager.FindByEmailAsync(register.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "E-mail is being used!");

                    ViewBag.Role = await db.NameRoles.ToListAsync();
                    return View(register);  // Trả về view đăng ký với lỗi Email đã tồn tại
                }

                var user = new ApplicationUser
                {
                    UserName = register.Email,
                    Email = register.Email,
                    Fullname = register.Fullname,
                    Phone = register.Phone,
                    EmailConfirmed = true   //confirm email trong admin
                };

                // Đăng ký người dùng mới với CreateAsync
                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    // Kiểm tra xem RoleId đã được chọn chưa
                    if (!string.IsNullOrEmpty(register.RoleId))
                    {
                        // Thêm vào bảng AspNetUserRoles
                        var userRole = new AspNetUserRole
                        {
                            UserId = user.Id,
                            RoleId = register.RoleId
                        };

                        db.AspNetUserRoles.Add(userRole);
                        await db.SaveChangesAsync();
                        //Console.WriteLine($"User created: {user.UserName} - {user.Email}");
                        //Console.WriteLine($"Role assigned: {register.RoleId}");
                    }
                    //else
                    //{

                    //    // Nếu không có RoleId được chọn thì gán vai trò mặc định
                    //    await _userManager.AddToRoleAsync(user, "User");
                    //    //Console.WriteLine($"User created: {user.UserName} - {user.Email}");
                    //    //Console.WriteLine("Default role 'User' assigned.");
                    //}
                    TempData["SuccessMessage"] = "Create new success !!";

                    return RedirectToAction(nameof(Index));
                }

                // Xử lý các lỗi trả về từ Identity khi đăng ký không thành công
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
            }

            // Nếu ModelState.IsValid không thành công, debug 
            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Any())
                {
                    foreach (var error in state.Value.Errors)
                    {
                        // Log or debug error messages
                        Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            // Lấy tất cả các vai trò trừ vai trò "Admin"
            var roles = await db.NameRoles
                .Where(r => r.NameRole1 != "Admin") // Lọc để loại bỏ vai trò "Admin"
                .ToListAsync();
            // Chuyển danh sách vai trò vào ViewBag để hiển thị ở dropdownlist
            ViewBag.Role = roles;

            return View(register);
        }


        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Lấy người dùng từ UserManager
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy danh sách tất cả các vai trò từ bảng NameRole
            var allRoles = await db.NameRoles
                .Where(role => role.NameRole1 != "Admin") // Lọc vai trò Admin
                .ToListAsync();

            // Lấy vai trò hiện tại của người dùng từ bảng AspNetUserRoles
            var userRoles = await db.AspNetUserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            // Lấy danh sách vai trò từ bảng NameRole dựa trên RoleId
            var userRoleIds = await db.NameRoles
                .Where(role => userRoles.Contains(role.Id))
                .Select(role => role.Id)
                .ToListAsync();

            // Tạo SelectList và đánh dấu quyền đang có
            ViewBag.Role = new SelectList(allRoles, "Id", "NameRole1", userRoleIds.FirstOrDefault());

            // Load ra view model
            var model = new UpdateAccountAdminViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Fullname = user.Fullname,
                Phone = user.Phone,
                RoleId = userRoleIds.FirstOrDefault() // Sử dụng RoleId cho dropdown
            };

            //// In thông tin để kiểm tra
            //Console.WriteLine($"User ID: {user.Id}");
            //Console.WriteLine($"User Email: {user.Email}");
            //Console.WriteLine($"User Roles: {string.Join(", ", userRoleIds)}");
            //Console.WriteLine($"Selected Role ID: {model.RoleId}");
            //foreach (var role in allRoles)
            //{
            //    Console.WriteLine($"Role ID: {role.Id}, Role Name: {role.NameRole1}");
            //}

            return View(model);
        }





        [HttpPost]
        [Route("update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateAccountAdminViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật thông tin người dùng
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Fullname = model.Fullname;
                user.Phone = model.Phone;
                user.EmailConfirmed = true;
                // Cập nhật mật khẩu nếu có
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                // Cập nhật thông tin người dùng
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    // Xóa các vai trò hiện tại của người dùng từ bảng AspNetUserRoles
                    var currentRoles = await db.AspNetUserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .ToListAsync();
                    db.AspNetUserRoles.RemoveRange(currentRoles);
                    await db.SaveChangesAsync();

                    // Thêm vai trò mới nếu có
                    if (!string.IsNullOrEmpty(model.RoleId))
                    {
                        // Lấy tên vai trò từ bảng NameRole
                        var roleName = await db.NameRoles
                            .Where(r => r.Id == model.RoleId)
                            .Select(r => r.NameRole1)
                            .FirstOrDefaultAsync();

                        if (roleName != null)
                        {
                            // Tạo đối tượng UserRole mới và thêm vào bảng
                            var userRole = new AspNetUserRole
                            {
                                UserId = user.Id,
                                RoleId = model.RoleId
                            };
                            db.AspNetUserRoles.Add(userRole);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Role does not exist.");
                            return View(model);
                        }
                    }
                    TempData["Updatesuccess"] = "Update success !!";

                    return RedirectToAction("Index");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Deletesuccess"] = "Delete success !!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, thêm thông báo lỗi vào ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Index", _userManager.Users.ToList());
        }

    }
}

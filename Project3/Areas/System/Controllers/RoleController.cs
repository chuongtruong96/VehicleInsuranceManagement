using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView.Admin;
namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system/role")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly VehicleInsuranceManagementContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, VehicleInsuranceManagementContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        [Route("index")]
        public IActionResult Index()
        {
            // Lọc ra vai trò "Admin"
            var roles = _context.NameRoles
                .Where(role => role.NameRole1 != "Admin")
                .Where(role => role.NameRole1 != "User")
                .ToList();

            return View(roles);
        }

        [Route("add")]
        public IActionResult add_role()
        {

            return View();
        }

        [Route("add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> add_role(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có ít nhất một quyền được chọn không
                if (model.SelectedPermissions == null || !model.SelectedPermissions.Any())
                {
                    ModelState.AddModelError("SelectedPermissions", "At least one permission must be selected!!");
                    return View(model);
                }

                // Tạo mới NameRole và gán giá trị Id là một GUID mới 
                var nameRole = new NameRole
                {
                    Id = Guid.NewGuid().ToString(), // Gán giá trị GUID mới cho Id
                    NameRole1 = model.NameRole1
                };

                _context.NameRoles.Add(nameRole);
                await _context.SaveChangesAsync(); // Lưu lại để NameRole nhận được Id tự động tăng

                // Thêm vào bảng Role_Permissions
                foreach (var permission in model.SelectedPermissions)
                {
                    // Lấy AspNetRole từ permission (đây có thể là Id hoặc tên quyền)
                    var role = await _roleManager.FindByNameAsync(permission);
                    if (role != null)
                    {
                        var rolePermission = new RolePermission
                        {
                            PermissionsId = role.Id,
                            RoleId = nameRole.Id // Sử dụng Id đã được tự động gán
                        };
                        _context.RolePermissions.Add(rolePermission);
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> update_role(string id)
        {
            var role = await _context.NameRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Lấy danh sách các quyền của vai trò từ bảng Role_Permissions
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionsId)
                .ToListAsync();

            // Lấy danh sách tất cả các quyền có sẵn
            var allPermissions = await _context.AspNetRoles
                 .Where(r => r.Name != "User" && r.Name != "Admin")
                .Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name,
                    Selected = permissions.Contains(r.Id) // Đánh dấu quyền đã được chọn
                })
                .ToListAsync();

            ViewBag.Permissions = allPermissions;

            var model = new CreateRoleViewModel
            {

                NameRole1 = role.NameRole1,
                SelectedPermissions = permissions // Đưa danh sách quyền vào SelectedPermissions
            };

            return View(model);

        }

        [HttpPost]
        [Route("update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> update_role(string id, CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)  // nếu model ko hợp lệ 
            {
                // Trong trường hợp ModelState không hợp lệ, hiển thị lại View với các lựa chọn quyền mà người dùng đã chọn trước đó
                var allPermissions = await _context.AspNetRoles
                    .Where(r => r.Name != "User" && r.Name != "Admin")  // điều kiện ko lấy ra view User và Admin
                                                                        //SelectListItem: Đây là một lớp trong ASP.NET Core dùng để biểu diễn một lựa chọn trong danh sách dropdown hoặc các checkbox.
                                                                        //Trong trường hợp này, mỗi SelectListItem sẽ biểu diễn một vai trò có sẵn, với Value là Id của vai trò và Text là tên của vai trò. 
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id,
                        Text = r.Name,
                        Selected = model.SelectedPermissions.Contains(r.Id) // Đánh dấu quyền đã được chọn
                    })
                    .ToListAsync();
                // tạo một đối tượng viewBag để truyền ra view
                ViewBag.Permissions = allPermissions;
                return View(model);
            }
            // nếu null hoặc ko chọn bất kì quyền nào thì báo lỗi
            if (model.SelectedPermissions == null || !model.SelectedPermissions.Any())
            {
                ModelState.AddModelError("SelectedPermissions", "At least one permission must be selected!!!");
                // tiếp tục load lại quyền đã có sẵn trong trường hợp khi update ng dùng bỏ trống ko chọn lại gì cả 
                var allPermissions = await _context.AspNetRoles
                    .Where(r => r.Name != "User" && r.Name != "Admin")
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id,
                        Text = r.Name,
                        Selected = model.SelectedPermissions.Contains(r.Id) // Đánh dấu quyền đã được chọn
                    })
                    .ToListAsync();

                ViewBag.Permissions = allPermissions;
                return View(model);
            }

            var role = await _context.NameRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Xóa các quyền hiện tại của vai trò từ bảng Role_Permissions
            var existingPermissions = _context.RolePermissions.Where(rp => rp.RoleId == role.Id);
            _context.RolePermissions.RemoveRange(existingPermissions);

            // Thêm các quyền mới được chọn vào bảng Role_Permissions
            foreach (var permissionId in model.SelectedPermissions)
            {
                var rolePermission = new RolePermission
                {
                    PermissionsId = permissionId,
                    RoleId = role.Id
                };
                _context.RolePermissions.Add(rolePermission);
            }
            // Cập nhật tên của vai trò
            role.NameRole1 = model.NameRole1;
            _context.NameRoles.Update(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [Route("delete")]
        public async Task<IActionResult> delete(string id)
        {
            // Tìm vai trò theo ID
            var role = await _context.NameRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            // Xóa tất cả RolePermissions liên quan
            var rolePermissions = _context.RolePermissions.Where(rp => rp.RoleId == id);
            _context.RolePermissions.RemoveRange(rolePermissions);

            // Kiểm tra xem vai trò có liên kết với người dùng không
            var userRoles = _context.AspNetUserRoles.Where(ur => ur.RoleId == id);
            if (userRoles.Any())
            {
                // Nếu có, thêm lỗi vào ModelState           
                TempData["roleerror"] = "This role cannot be deleted because it is being used by a user!!";
                // Trả về view hiện tại với thông báo lỗi
                return RedirectToAction("Index");
            }
            // Xóa vai trò
            _context.NameRoles.Remove(role);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng đến Index
            return RedirectToAction("Index");
        }

    }
}

using IdentityMvc.Areas.Admin.ViewModels;
using IdentityMvc.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    private readonly RoleManager<AppRole> _roleManager;

    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        var userListViewModel = users.Select(x => new UserViewModel
        {
            Id = x.Id,
            UserName = x.UserName,
            Email = x.Email
        }).ToList();
        return View(userListViewModel);
    }

    public IActionResult RoleCreate()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RoleCreate(RoleCreateViewModel roleCreateViewModel)
    {
        AppRole appRole = new();
        appRole.Name = roleCreateViewModel.Name;
        var result = _roleManager.CreateAsync(appRole).Result;
        if (result.Succeeded)
            return RedirectToAction(nameof(Roles));
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return View(roleCreateViewModel);
    }

    public IActionResult Roles()
    {
        return View(_roleManager.Roles.ToList());
    }

    public IActionResult RoleDelete(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role is null)
        {
            //ModelState.AddModelError(String.Empty, "Rol bulunamamıştır.");
            TempData["SuccessMessage"] = "Rol bulunamamıştır.";
            return RedirectToAction(nameof(Roles));
        }

        var result = _roleManager.DeleteAsync(role).Result;
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Rol başarıyla silindi.";
            return RedirectToAction(nameof(Roles));
        }

        TempData["SuccessMessage"] = "Rol silinemedi.";
        //ModelState.AddModelError(String.Empty, "Rol Silinemedi.");
        return RedirectToAction(nameof(Roles));
    }

    public IActionResult RoleUpdate(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role is null)
        {
            TempData["SuccessMessage"] = "Rol bulunamamıştır.";
            return RedirectToAction(nameof(Roles));
        }

        var roleUpdateViewModel = role.Adapt<RoleUpdateViewModel>();
        return View(roleUpdateViewModel); //AppRole u RoleUpdateViewModel e mapledik.
    }

    [HttpPost]
    public IActionResult RoleUpdate(RoleUpdateViewModel roleUpdateViewModel)
    {
        var role = _roleManager.FindByIdAsync(roleUpdateViewModel.Id).Result;
        //AppRole role = model.Adapt<AppRole>(); //model i AppRole e çevirip UpdateAsync e yollayalım.
        if (role is null)
        {
            ModelState.AddModelError(string.Empty, "Güncelleme işlemi başarısız oldu.");
            return View(roleUpdateViewModel);
        }

        role.Name = roleUpdateViewModel.Name;
        var result = _roleManager.UpdateAsync(role).Result;
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Rol başarıyla güncellendi.";
            return RedirectToAction(nameof(Roles));
        }
        // TempData["SuccessMessage"] = "Rol güncellenemedi.";

        // return RedirectToAction(nameof(Roles));
        ModelState.AddModelError(string.Empty, "Rol Güncellenemedi.");
        return View(roleUpdateViewModel);
    }

    public IActionResult RoleAssign(string id)
    {
        TempData["userId"] = id;
        var user = _userManager.FindByIdAsync(id).Result;
        if (user is null)
        {
            TempData["SuccessMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction(nameof(Users));
        }

        ViewBag.userName = user.UserName;
        var roles = _roleManager.Roles; //Tüm roller geldi.
        var userRoles = _userManager.GetRolesAsync(user).Result; //kullanıcıya ait roller geldi.
        //List<string> userRoles=_userManager.GetRolesAsync(user).Result as List<string>;

        List<RoleAssignViewModel> roleAssignViewModels = new();
        foreach (var role in roles)
        {
            var roleAssignViewModel = new RoleAssignViewModel();

            roleAssignViewModel.RoleId = role.Id;
            roleAssignViewModel.RoleName = role.Name;
            // if (userRoles.Contains(role.Name)) {  roleAssignViewModel.Exist = true; }
            // else { roleAssignViewModel.Exist = false; }

            roleAssignViewModel.Exist = userRoles.Contains(role.Name) ? true : false;
            roleAssignViewModels.Add(roleAssignViewModel);
        }

        return View(roleAssignViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
    {
        var userId = TempData["userId"].ToString();
        var user = _userManager.FindByIdAsync(userId).Result;
        // roleAssignViewModels.ForEach(x => 
        // {
        //      var result = x.Exist
        //         ?  _userManager.AddToRoleAsync(user, x.RoleName)
        //         :  _userManager.RemoveFromRoleAsync(user, x.RoleName);
        // });
        foreach (var roleAssign in roleAssignViewModels)
            if (roleAssign.Exist)
                await _userManager.AddToRoleAsync(user, roleAssign.RoleName);
            else
                await _userManager.RemoveFromRoleAsync(user, roleAssign.RoleName);
        return RedirectToAction(nameof(Users));
    }
}
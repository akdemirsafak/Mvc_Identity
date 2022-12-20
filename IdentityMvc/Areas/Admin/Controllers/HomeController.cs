using IdentityMvc.Areas.Admin.ViewModels;
using IdentityMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMvc.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> UserList()
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
}
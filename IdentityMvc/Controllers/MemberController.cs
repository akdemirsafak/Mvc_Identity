using IdentityMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMvc.Controllers;

[Authorize]
public class MemberController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;

    // GET
    public MemberController(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // public async Task<IActionResult> LogOut()
    // {
    //     await _signInManager.SignOutAsync();
    //     return RedirectToAction("Index", "Home");
    // }

    public async Task Logout() //Daha efektif yöntemi
    {
        await _signInManager.SignOutAsync();
    }

    public IActionResult Index()
    {
        return View();
    }
    //Diğer controller'larda sadece üyelerin erişmesini istediğimiz endpointlere [Authorize] attribute ü kullanmalıyız.
}

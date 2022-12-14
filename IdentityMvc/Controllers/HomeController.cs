using IdentityMvc.Models;
using IdentityMvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMvc.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid) return View();

        var identityResult = await _userManager.CreateAsync(new ()
        {
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        }, model.Password!);

        if (identityResult.Succeeded)
        {
            TempData["SuccessMessage"] = "Üyelik işlemi başarıyla tamamlandı.";
            return RedirectToAction(nameof(SignUp));
        }

        foreach (var errors in identityResult.Errors) ModelState.AddModelError(string.Empty, errors.Description);
        return View();
    }

    
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        return View();
    }
    
    // public IActionResult Login(LoginViewModel model)
    // {
    //     return View();
    // }

    
}
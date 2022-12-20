using IdentityMvc.Extensions;
using IdentityMvc.Models;
using IdentityMvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMvc.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

        var identityResult = await _userManager.CreateAsync(new AppUser
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

        ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
        return View();
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Action("Index", "Home");
        //var result = _signInManager.PasswordSignInAsync(model.UserName, model.Password, model); //Eğer username ila giriş yaptırıyorsak
        var hasUser = await _userManager.FindByEmailAsync(model.Email);
        if (hasUser == null)
        {
            ModelState.AddModelError(
                string.Empty, "Email veya şifre yanlış.");
            return View();
        }

        var loginResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);
        //buradaki false kullanıcı bilgilerinin uzun vadede cookie'de tutulması durumudur.
        //Kullanıcı n tane yanlış şifre girişi yaptığında hesabı kitlensin veya kitlenmesin.Default olarak 5 dir.

        if (loginResult.Succeeded) return Redirect(returnUrl);
        if (loginResult.IsLockedOut)
        {
            ModelState.AddModelErrorList(new List<string> { "Hesabınız 3 dakikalığına kitlendi." });
            return View();
        }

        var lockoutCount = await _userManager.GetAccessFailedCountAsync(hasUser);
        ModelState.AddModelErrorList(new List<string> { $"Yanlış giriş sayısı : {lockoutCount}" });
        ModelState.AddModelErrorList(new List<string> { "Email veya şifre yanlış." });
        return View();
    }

    // public IActionResult Login(LoginViewModel model)
    // {
    //     return View();
    // }
}
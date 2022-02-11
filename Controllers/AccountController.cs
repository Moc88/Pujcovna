//chyby by se mìly opravit samy do L10, pokud né, poøešit
//všechny chyby v prùbìhu L6 (7 chyb) jsou dány absencí dvou souborù -  mìly by zmizet do konce L10


using pujcovna.Classes;
using pujcovna.Models;
using pujcovna.Extensions;
using pujcovna.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace pujcovna.Controllers
{
    [Authorize]
    [ExceptionsToMessageFilter]
    public class AccountController : Controller
    {
        //toto následující je asi dependency injection?
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #region Helpers
        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                  ? Redirect(returnUrl)
                  : (IActionResult)RedirectToAction(nameof(HomeController.Index), "Home");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if (await userManager.FindByEmailAsync(model.Email) == null)
                {
                    // vytvoøíme nový objekt typu ApplicationUser (uživatel), pøidáme ho do databáze a pøihlásíme ho
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        this.AddFlashMessage(new FlashMessage("Registrace probìhla úspìšnì", FlashMessageType.Success));

                        return string.IsNullOrEmpty(returnUrl)
                            ? RedirectToAction("Index", "Home")
                            : RedirectToLocal(returnUrl);
                    }

                    AddErrors(result);
                }

                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Email {model.Email} je již zaregistrován" }));
            }

            //nevím jestli je následující øádek na správném místì?
            this.AddFlashMessage(new FlashMessage("Registrace probìhla úspìšnì", FlashMessageType.Success));
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // kontrola na stranì serveru, zda jsou všechny odeslané údaje obsažené ve viewmodelu v poøádku
            if (!ModelState.IsValid)
                return View(model);

            // pokus o pøihlášení uživatele na základì zadaných údajù
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            // pokud byly odeslány neplatné údaje, vrátíme uživatele k pøihlašovacímu formuláøi
            if (result.Succeeded)
            {
                this.AddFlashMessage(new FlashMessage("Pøihlášení probìhlo úspìšnì", FlashMessageType.Success));
                return RedirectToAction("Administration");
            }

            ModelState.AddModelError(string.Empty, "Neplatné pøihlašovací údaje");
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            
            await signInManager.SignOutAsync();
            this.AddFlashMessage(new FlashMessage("Odhlášení probìhlo úspìšnì", FlashMessageType.Success));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User)
                ?? throw new ApplicationException($"Nepodaøilo se naèíst uživatele s ID {userManager.GetUserId(User)}.");

            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User)
                ?? throw new ApplicationException($"Nepodaøilo se naèíst uživatele s ID: {userManager.GetUserId(User)}.");

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);

            this.AddFlashMessage(new FlashMessage("Zmìna hesla probìhlo úspìšnì", FlashMessageType.Success));

            return RedirectToAction("Administration");
        }

        public IActionResult Administration()
        {
            return View();
        }


    }
}
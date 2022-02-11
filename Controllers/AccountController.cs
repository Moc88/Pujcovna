//chyby by se m�ly opravit samy do L10, pokud n�, po�e�it
//v�echny chyby v pr�b�hu L6 (7 chyb) jsou d�ny absenc� dvou soubor� -  m�ly by zmizet do konce L10


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
        //toto n�sleduj�c� je asi dependency injection?
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
                    // vytvo��me nov� objekt typu ApplicationUser (u�ivatel), p�id�me ho do datab�ze a p�ihl�s�me ho
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        this.AddFlashMessage(new FlashMessage("Registrace prob�hla �sp�n�", FlashMessageType.Success));

                        return string.IsNullOrEmpty(returnUrl)
                            ? RedirectToAction("Index", "Home")
                            : RedirectToLocal(returnUrl);
                    }

                    AddErrors(result);
                }

                AddErrors(IdentityResult.Failed(new IdentityError() { Description = $"Email {model.Email} je ji� zaregistrov�n" }));
            }

            //nev�m jestli je n�sleduj�c� ��dek na spr�vn�m m�st�?
            this.AddFlashMessage(new FlashMessage("Registrace prob�hla �sp�n�", FlashMessageType.Success));
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

            // kontrola na stran� serveru, zda jsou v�echny odeslan� �daje obsa�en� ve viewmodelu v po��dku
            if (!ModelState.IsValid)
                return View(model);

            // pokus o p�ihl�en� u�ivatele na z�klad� zadan�ch �daj�
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            // pokud byly odesl�ny neplatn� �daje, vr�t�me u�ivatele k p�ihla�ovac�mu formul��i
            if (result.Succeeded)
            {
                this.AddFlashMessage(new FlashMessage("P�ihl�en� prob�hlo �sp�n�", FlashMessageType.Success));
                return RedirectToAction("Administration");
            }

            ModelState.AddModelError(string.Empty, "Neplatn� p�ihla�ovac� �daje");
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            
            await signInManager.SignOutAsync();
            this.AddFlashMessage(new FlashMessage("Odhl�en� prob�hlo �sp�n�", FlashMessageType.Success));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User)
                ?? throw new ApplicationException($"Nepoda�ilo se na��st u�ivatele s ID {userManager.GetUserId(User)}.");

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
                ?? throw new ApplicationException($"Nepoda�ilo se na��st u�ivatele s ID: {userManager.GetUserId(User)}.");

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);

            this.AddFlashMessage(new FlashMessage("Zm�na hesla prob�hlo �sp�n�", FlashMessageType.Success));

            return RedirectToAction("Administration");
        }

        public IActionResult Administration()
        {
            return View();
        }


    }
}
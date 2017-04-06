﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LexiconLMS.Models;
using System.Collections.Generic;

namespace LexiconLMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        [Authorize(Roles = "Lärare")]
        public ActionResult Index(string söksträng)
        {
            List<AccountIndexViewModel> accountIndexViewModelList = new List<AccountIndexViewModel>();
            
            if(String.IsNullOrWhiteSpace(söksträng))
            {
                var viewModel = GetAllUsersForIndexViewModel();
                return View(viewModel);
            }

            söksträng = söksträng.ToLower();
            var users = db.Users.ToArray();
            foreach (var user in users)
            {
                if(user.FörNamn.ToLower().Contains(söksträng)
                    || user.EfterNamn.ToLower().Contains(söksträng)
                    || user.FullNamn.ToLower().Contains(söksträng)
                    || user.Email.ToLower().Contains(söksträng))
                {
                    var roles = UserManager.GetRoles(user.Id).ToList();
                    var kurser = db.Kurser.Where(k => k.Id == user.KursId);
                    Kurs kurs = null;

                    if (kurser.Count() > 0)
                        kurs = kurser.First();

                    AccountIndexViewModel element = new AccountIndexViewModel
                    {
                        Id = user.Id,
                        Epost = user.Email,
                        FullNamn = user.FörNamn + " " + user.EfterNamn,
                        ÄrLärare = roles.Contains("Lärare") ? true : false,
                        Kursnamn = (kurs != null) ? kurs.Namn : "",
                    };
                    accountIndexViewModelList.Add(element);
                }
            }

            return View(accountIndexViewModelList);
        }

        private List<AccountIndexViewModel> GetAllUsersForIndexViewModel()
        {
            var users = db.Users.ToList();
            var accountIndexViewModelList = new List<AccountIndexViewModel>();

            foreach (var user in users)
            {
                var roles = UserManager.GetRoles(user.Id).ToList();
                var kurser = db.Kurser.Where(k => k.Id == user.KursId);
                Kurs kurs = null;

                if (kurser.Count() > 0)
                    kurs = kurser.First();

                AccountIndexViewModel element = new AccountIndexViewModel
                {
                    Id = user.Id,
                    Epost = user.Email,
                    FullNamn = user.FörNamn + " " + user.EfterNamn,
                    ÄrLärare = roles.Contains("Lärare") ? true : false,
                    Kursnamn = (kurs != null) ? kurs.Namn : "",
                };

                accountIndexViewModelList.Add(element);
            }

            return accountIndexViewModelList;
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Lärare")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, KursId = model.KursId, FörNamn = model.FirstName, EfterNamn = model.LastName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.ÄrLärare)
                    {
                        var foundUser = UserManager.FindByEmail(model.Email);
                        UserManager.AddToRole(foundUser.Id, "Lärare");
                    }

                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //Skicka med alla users till index.
                    TempData["Händelse"] = $"Lyckat! Skapat användare {user.FullNamn}.";
                    TempData["Status"] = "Lyckat";
                  
                    var users = GetAllUsersForIndexViewModel();
                    return View("Index", users);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //GET, Edit
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var användare = UserManager.FindById(id);
            if (användare == null)
                return HttpNotFound();

            bool ärLärare = false;
            var roller = UserManager.GetRoles(användare.Id);
            if (roller.Count() > 0)
            {
                if(roller.First().CompareTo("Lärare") == 0)
                    ärLärare = true;
            }

            var dagensDatum = DateTime.Today;
            var allaKurser = db.Kurser.ToArray();
            List<Kurs> kurser = new List<Kurs>();
            foreach (var kurs in allaKurser)
            {
                var moduler = kurs.Moduler.ToArray();
                var startDatum = kurs.StartDatum;

                if (moduler.Count() > 0)
                {
                    var slutDatum = moduler[moduler.Count() - 1].SlutDatum;
                    if (dagensDatum.CompareTo(slutDatum) < 0) kurser.Add(kurs);
                }
            }

            new SelectList(db.Kurser.ToList(), "Id", "Namn");
            var viewModel = new AccountEditViewModel
            {
                Id = användare.Id,
                Epost = användare.Email,
                Förnamn = användare.FörNamn,
                Efternamn = användare.EfterNamn,
                ÄrLärare = ärLärare,
                KursId = användare.KursId,
                Kurser = new SelectList(kurser, "Id","Namn"),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit([Bind(Include = "Id, Password, Epost, Förnamn, Efternamn, KursId, ÄrLärare")] AccountEditViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var användare = UserManager.FindById(viewModel.Id);
                användare.UserName = viewModel.Epost;
                användare.Email = viewModel.Epost;
                användare.FörNamn = viewModel.Förnamn;
                användare.EfterNamn = viewModel.Efternamn;
                användare.KursId = viewModel.KursId;

                if(!String.IsNullOrWhiteSpace(viewModel.Password))
                {
                    var resetToken = UserManager.GeneratePasswordResetToken(användare.Id);
                    UserManager.ResetPassword(användare.Id, resetToken, viewModel.Password);
                }

                UserManager.Update(användare);

                if (UserManager.IsInRole(användare.Id,"Lärare"))
                {
                    //Turn lärare into elev
                    if(!viewModel.ÄrLärare)
                        UserManager.RemoveFromRole(användare.Id, "Lärare");
                }
                else
                {
                    //Turn elev into lärare
                    if(viewModel.ÄrLärare)
                        UserManager.AddToRole(användare.Id, "Lärare");
                }

                TempData["Händelse"] = $"Lyckat! Uppdaterat användare {viewModel.Förnamn} {viewModel.Efternamn}.";
                TempData["Status"] = "Lyckat";
                return RedirectToAction("Index");
            }

            viewModel.Kurser = new SelectList(db.Kurser.ToList(), "Id", "Namn");
            return View(viewModel);
        }

        [Authorize(Roles = "Lärare")]
        public ActionResult Details(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var användare = db.Users.Find(id);
            var kurserQuery = db.Kurser.Where(k => k.Id == användare.KursId);
            Kurs kurs = null;

            if(användare == null)
                return HttpNotFound();

            if (kurserQuery.Count() > 0)
                kurs = kurserQuery.First();

            var roles = UserManager.GetRoles(användare.Id);
            string role = "";
            if (roles.Count() > 0)
                role = roles.First();

            string kursnamn;
            if (kurs == null)
                kursnamn = "Deltar ej";
            else
                kursnamn = kurs.Namn;

            var accountDetailViewModel = new AccountDetailViewModel
            {
                Id = användare.Id,
                Epost = användare.Email,
                FullNamn = användare.FörNamn + " " + användare.EfterNamn,
                Roll = (role.CompareTo("Lärare") == 0) ? "Lärare" : "Elev",
                Kursnamn = kursnamn,
            };
            return View(accountDetailViewModel);
        }

        [Authorize(Roles = "Lärare")]
        public ActionResult Delete(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);
            if(user == null)
                return HttpNotFound();

            if(User.Identity.GetUserId().CompareTo(user.Id) == 0)
            {
                TempData["Händelse"] = "Ojdå! Ni kan inte ta bort er själva!";
                TempData["Status"] = "Misslyckat";
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lärare")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = UserManager.FindById(id);

            TempData["Händelse"] = $"Lyckat! Tagit bort användare {user.FullNamn}.";
            TempData["Status"] = "Lyckat";

            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index","Skol");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Misslyckat! Felaktigt inloggningsförsök.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
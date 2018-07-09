using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using cms.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net.Mail;

namespace cms.Controllers {
    public class AccountController : Controller {
        cmsEntities1 db = new cmsEntities1();

        ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager {
            get {
                if(_signInManager == null) {
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                return _signInManager;
            }
        }

        ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager {
            get {
                if(_userManager == null) {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _userManager;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login() {
            return View();
        }



        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public ActionResult Login(LoginModel model, string returnUrl) {

        //    if (ModelState.IsValid)
        //    {
        //        var exist = db.Users.Where(c => c.Email == model.UserName && c.Password == model.Password).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            return RedirectToAction("Index", "Home");

        //        }
        //    }


        //return View(model);


        //}





        public ActionResult Login(LoginModel model, string returnUrl)
        {
			var user = db.Users.Where(c => c.Email == model.UserName && c.Password == model.Password).FirstOrDefault();
			if (user != null)
			{
				var result = PasswordSignIn(model.UserName, model.Password, user.Password, user.Id.ToString(), user.Email, (bool)model.RememberMe);
				switch (result)
				{
					case SignInStatus.Success:
						Session["UserName"] = user.UserName;
						Session["LoggedTime"] = DateTime.Now.ToString("HH:mm:ss");
						return RedirectToAction("Index", "Home");

				}
			}


			ViewBag.ErrorMessage ="Username or Password Incorrect.";
            return View(model);
        }

		private string GetUserRolesDto(string userName)
		{
			var model = (from ur in db.UserRoles.Where(c => c.UserId.ToString() == userName)
						 from roles in db.Roles.Where(c => c.Id.ToString() == ur.RoleId)
						 from user in db.Users.Where(c => c.Id.ToString() == ur.UserId)
						 select new UserRolesDto
						 {
							 ObjId = ur.ObjId,
							 RoleName = roles.Name,
							 RoleId = ur.RoleId,
							 UserId = user.Id.ToString()
						 });
var results = model.FirstOrDefault();
			return results.RoleName;
		}

		public SignInStatus PasswordSignIn(string userName, string password, string DBPassword, string DBId, string DBEmail, bool rememberMe)
        {


            var usertextPassword = DBPassword;
            // Match Password
            if (usertextPassword != password)
            {
                return SignInStatus.Failure;
            }

            if (usertextPassword == password)
            {
				//Get User Role
				string userRole = GetUserRolesDto(DBId);

				if (userRole != null)
				{
					FormsAuthentication.SetAuthCookie(DBEmail, false);
					var authTicket = new FormsAuthenticationTicket(1, DBEmail, DateTime.Now, DateTime.Now.AddMinutes(20), false, userRole);
					string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
					var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					HttpContext.Response.Cookies.Add(authCookie);
				}


				return SignInStatus.Success;
                
            }

            return SignInStatus.Failure;
        }
        //
        // GET: /Account/LogOff

        public ActionResult LogOff() {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model) {
            if(ModelState.IsValid) {
                // Attempt to register the user
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return Redirect("/");
                }
                ViewBag.ErrorMessage = result.Errors.First();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword() {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model) {
            if(ModelState.IsValid) {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ViewBag.ErrorMessage = result.Errors.First();
                return View(model);
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess() {
            return View();
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
        //// POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("Code", "Invalid code.");
        //            return View(model);
        //    }
        //}

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
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
				var user = db.Users.Where(c => c.Email == model.Email).FirstOrDefault();
				if (user.Email == null)
				{
					ModelState.AddModelError("Email", "The user either does not exist or is not confirmed");
                    return View(model);
                }

				try
				{
					//Create a token of a type guid
					string UserToken = Guid.NewGuid().ToString();

					if (user != null)
					{
						//Get email for that user
						string EmailId = user.Email;

						//create url with above token
						var resetLink = "<a href='" + Url.Action("ResetUserPassword", "Account", new { userId = user.Id, tk = UserToken }, "http") + "'>Reset Password</a>";

						//send mail
						string subject = " Password Reset Token";
						string body = "Dear " + user.UserName + " <br/>" + " You recently requested to reset your Password for Palbroker <br/>"
						+ "Please find the Password Reset Token " + resetLink; //edit it

						//Pass Email Address to ResetUserPassword form
						ViewBag.PassEmail = user.Email;
						try
						{
							SendEMail(EmailId, subject, body);

							//   TempData["Message"] = "Mail Sent.";
							return PartialView("EmailSuccess");
						}
						catch (Exception ex)
						{
							ViewBag.ErrorMessage = "Error occured while sending email." + ex.Message;
						}
					}
					else
					{
						ViewBag.ErrorMessage = "Error occured while sending email. User does not exit!";
					}

				}
				catch (Exception e)
				{
					ViewBag.ErrorMessage = e.Message;
				}
//				return PartialView("ForgotPasswordView");

				return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

		[AllowAnonymous]
		public ActionResult ResetUserPassword()
		{

			return PartialView("ResetPassword");

		}

		private void SendEMail(string emailid, string subject, string body)
		{
			System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
			client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
			client.EnableSsl = false;
			client.Host = "smtp.gmail.com";
			client.Port = 587;


			System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("b.mutheiwana@gmail.com", "Windows8");
			client.UseDefaultCredentials = false;
			client.Credentials = credentials;

			System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
			msg.From = new MailAddress("b.mutheiwana@gmail.com");
			msg.To.Add(new MailAddress(emailid));

			msg.Subject = subject;
			msg.IsBodyHtml = true;
			msg.Body = body;

			client.Send(msg);
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
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "No user found");
                return View(model);
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
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

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
                    return View("Login");
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
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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
            return Redirect("/");
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
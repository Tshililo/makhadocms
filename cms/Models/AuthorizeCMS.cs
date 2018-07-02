using cms.Controllers;
using cms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace cms
{

	public class AuthorizeCMS : System.Web.Mvc.AuthorizeAttribute
	{
		public string RedirectUrl = "~/Account/NotAuthorized";


		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			// Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
			// return true;
			//filterContext.Result = new RedirectResult("~/Shared/NotAuthorized");
			// return Unauthorized();

			if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				//if not logged, it will work as normal Authorize and redirect to the Login
				base.HandleUnauthorizedRequest(filterContext);

			}
			else
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "NotAuthorized", area = "" }));
			}
		}


		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var isAuthorized = base.AuthorizeCore(httpContext);

			//Redirect("~/Account/NotAuthorized");

			//return false;

			//IIS lost the user session, however user still logged in via cookie
			if (isAuthorized )
			{
				var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

				//Wrong cookie
				if (HttpContext.Current.User.Identity.Name != ticket.Name)
				{
					httpContext.Response.Redirect(RedirectUrl);
					return false;
				}
				//Check for "remember me" checkbox
				if (!ticket.IsPersistent)
				{
					return false;

				}
				var userData = string.Empty;
				try
				{
					userData = ApplicationModel.Decrypt(ticket.UserData, "{3F4E0AA1-2D03-4210-942D-9C1F30F75E2C}");
				}
				catch (Exception ex)
				{
					//invalid cookie data, tampering?
					httpContext.Response.Redirect(RedirectUrl);
					return false;
				}

				if (userData.Length <= 4 || !userData.Substring(0, 4).IsNumeric())
				{
					httpContext.Response.Redirect(RedirectUrl);
					return false;
				}

				var season = userData.Substring(0, 4);
				var password = userData.Substring(4);

				//var x = new AuthenticationDomainService();
				//x.ValidateUser1(ticket.Name, password);

				User user = null;
				user = new User()
				{
					Name = HttpContext.Current.User.Identity.Name
				};

			//	user = S.Security.Default.GetAuthenticatedUser(user);

				// var locale = "en-ZA";
				//PalBrokerDomainService.StoreUserMetadata(user, locale, season, "NOT_SILVERLIGHT");

			}

			if (!isAuthorized)
			{
				//return RedirectToAction("NotAuthorized", "Account", new { area = string.Empty });

				return false;

			}

			return true;
		}




	}

}
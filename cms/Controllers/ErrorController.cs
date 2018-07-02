using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cms;

namespace cms.Controllers
{
	// [Authorize]
	public class ErrorController : Controller
	{
		// GET: Error
		public ActionResult Index()
		{
			Response.StatusCode = 500;
			return View("Error");
		}

		//
		// GET: /Error/PageNotFound
		public ActionResult PageNotFound()
		{
			Response.StatusCode = 404;

			return View("Account/PageNotFound");
		}

		public ActionResult NotAuthorized()
		{
			Response.StatusCode = 401;
			return View("Account/NotAuthorized");
		}

	}


}
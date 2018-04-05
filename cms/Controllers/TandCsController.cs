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
    public class TandCsController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
      

}
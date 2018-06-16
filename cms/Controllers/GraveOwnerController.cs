using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cms;
using cms.Models;

namespace cms.Controllers
{
  //  [Authorize]
    public class GraveOwnerController : Controller
    {
        private cmsEntities1 db = new cmsEntities1();

        // GET: Cemeteries
        public ActionResult Index()
        {
            return View();
        }


    }
}

using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class DropBoxController : Controller
    {
		cmsEntities1 db = new cmsEntities1();
		// GET: DropBox
		public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial(string headerObjId)
        {
            var  RootFolder = @"~\Content\DropBox";
            return PartialView("_FileManagerPartial", RootFolder);
        }

		public ActionResult ApplicationsGridViewPartial()
		{
			var model = db.Applications;

			// DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
			return PartialView("GridViewPartialView", model.ToList());
		}

		public FileStreamResult FileManagerPartialDownload()
        {
            //  return FileManagerExtension.DownloadFiles("DropBox", DropBoxControllerDropBoxSettings.Model);
            return null;
        }
    }

}
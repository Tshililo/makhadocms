using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class DropBoxController : BaseController
    {
 
		// GET: DropBox
		public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileManagerPartial(string headerObjId)
        {
			string RootFolder;
			if (headerObjId == null)
			{
				RootFolder = @"C:\DropBox\";
				return PartialView("_FileManagerPartial", RootFolder);
			}
			var model = db.Applications.Where(c => c.ObjId.ToString() == headerObjId).FirstOrDefault();
			//RootFolder = @"~\Content\" + model.IdNo;

			RootFolder = @"C:\DropBox\" + model.IdNo;
			// Determine whether the directory exists.
			if (Directory.Exists(RootFolder))
			{
				ViewBag.RootFolder = RootFolder;
				return PartialView("_FileManagerPartial", RootFolder);
			}
			Directory.CreateDirectory(RootFolder);
			ViewBag.RootFolder = RootFolder;
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
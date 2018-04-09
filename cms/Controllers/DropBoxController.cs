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
        // GET: DropBox
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            var  RootFolder = @"~\Content\DropBox";
            return PartialView("_FileManagerPartial", RootFolder);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            //  return FileManagerExtension.DownloadFiles("DropBox", DropBoxControllerDropBoxSettings.Model);
            return null;
        }
    }
    //public class DropBoxControllerDropBoxSettings
    //{
    //    public const string RootFolder = @"~\Content\DropBox";

    //    public static string Model { get { return RootFolder; } }
    //}

}
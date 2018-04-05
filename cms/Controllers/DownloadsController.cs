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
    public class DownloadsController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            return PartialView("_FileManagerPartial", DownloadsControllerDropBoxSettings.Model);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            //  return FileManagerExtension.DownloadFiles("DropBox", DropBoxControllerDropBoxSettings.Model);
            return null;
        }
    }
    public class DownloadsControllerDropBoxSettings
    {
        public const string RootFolder = @"~\Content\Downloads";

        public static string Model { get { return RootFolder; } }
    }

}
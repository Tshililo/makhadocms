using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cms.Models;
using cms;
using DevExpress.XtraReports.UI;
using DevExpress.Web.Mvc;
using System.Data;
using System.IO;

namespace cms.Controllers
{
    public class HomeController : Controller
    {
        CMSEntityModel db = new CMSEntityModel();
        public ActionResult Index()
        {
            return View();
        }


        #region Applications

        private List<Mortuary> GetMortuary()
        {
            return db.Mortuaries.ToList();
        }

        public PartialViewResult FileUploadPopUp()
        {
            FileUpload data = new FileUpload();

            return PartialView("EditFileUploadPopUp", data);
        }

        public PartialViewResult ExportCSV(string headerObjId, string ApplicantObjId)
        {
           var FilteredQuery = db.Applications.ToList();

            var filename = "";

            return null;
        }

        public PartialViewResult ImportApplication()
        {
            Application data = new Application();

            return PartialView("ImportApplication", data);
        }


        public PartialViewResult SaveFileUploadPopUp()
        {
            FileUpload data = new FileUpload();

            return PartialView("EditFileUploadPopUp", data);
        }
        public ActionResult ApplicationsUpdateEntryToForm(Guid ObjId, Application model)
        {
            ViewData["Mortuaries"] = GetMortuary();
            var ApplicationsInfo = db.Applications.Where(s => s.ObjId == ObjId).FirstOrDefault();

            if (ApplicationsInfo == null)
            {
                model.ObjId = ObjId;
                model.IdNo = "0000000000000";
                return PartialView("CreateApplicationsEditPartial", model);
            }

            if (ApplicationsInfo != null)
            {
                return PartialView("CreateApplicationsEditPartial", ApplicationsInfo);
            }

            return null;

        }

        public ActionResult ApplicationsGridViewPartial()
        {
            var BurialRecords = db.Applications.ToList().OrderBy(r => r.DateOfBurial).ThenBy(r => r.DeathAge);
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", BurialRecords);
        }

        public ActionResult ApplicationsEdit(Application item)
        {
            var model = db.Applications;
            var exists = model.Where(c => c.ObjId == item.ObjId).SingleOrDefault();

            if (exists == null)
            {
                model.Add(item);
                db.SaveChanges();
            }
            if (exists != null)
            {

                this.UpdateModel(exists);
                // model.Attach(userRole);
                db.SaveChanges();

            }
            var BurialRecords = db.Applications.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", BurialRecords);

        }


        [HttpPost, ValidateInput(false)]
        public ActionResult ApplicationsDelete(Guid ObjId)
        {

            var model = db.Applications;
            var model2 = db.DualApplications;
            if (ObjId != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ObjId == ObjId);
                    if (item != null)
                    {
                        model.Remove(item);

                    }
                    var item2 = model2.Where(it => it.HeaderApplicationId == ObjId).ToList();
                    foreach (var record in item2)
                    {
                        if (item != null)
                        {
                            model2.Remove(record);

                        }
                    }

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var BurialRecords = db.Applications.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", BurialRecords);
        }


        #endregion
        #region PrintReport


        [HttpGet]
        public ActionResult ReportViewPartial(string reportParams)
        {

            var report = GetReport(reportParams);

            return PartialView("ReportViewPartial", report);
        }

        XtraReport GetReport(string passed_params)
        {
            List<string> p = passed_params.Split(',').ToList();
            var rep = new AppReportXrMvc();
            rep.ObjId.Value = p[0];
            rep.Attention.Value = p[1];
            rep.ToCompany.Value = p[2];
            return rep;
        }
        #endregion


        #region GraveOwners
        [ValidateInput(false)]
        public ActionResult GraveOwnerPartial(string headerObjId)
        {
            var model = GetGraveOwnerDto(headerObjId);
            return PartialView("GraveOwnerGridViewPartial", model.ToList());
        }

        public ActionResult LinkToGraveLanding(string headerObjId)
        {
            return PartialView("LinkToGraveLanding");
        }

        public ActionResult LinkToGraveAdd(string ApplicantObjId, string GraveId)
        {
            Guid AppObjId = Guid.Parse(ApplicantObjId);
            Guid graveId = Guid.Parse(GraveId);

            var model = db.GraveOwners;
            var exists = model.Where(c => c.ApplicationId == AppObjId && c.GraveId == graveId).SingleOrDefault();

            if (exists == null)
            {
                GraveOwner Item = new GraveOwner();
                Item.ApplicationId = AppObjId;
                Item.GraveId = graveId;
                Item.ObjId = Guid.NewGuid();

                model.Add(Item);
                db.SaveChanges();
            }
            if (exists != null)
            {

                this.UpdateModel(exists);
                // model.Attach(userRole);
                db.SaveChanges();

            }
            var BurialRecords = db.Applications.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", BurialRecords);

        }

        private List<GraveOwnerDto> GetGraveOwnerDto(string headerObjId)
        {
            var model = (from go in db.GraveOwners.Where(c => c.ApplicationId.ToString() == headerObjId)
                         from gr in db.Graves.Where(c => c.ObjId == go.GraveId)
                         from app in db.Applications.Where(c => c.ObjId == go.ApplicationId)
                         select new GraveOwnerDto
                         {
                             ObjId = go.ObjId,
                             ApplicationId = app.ObjId,
                             GraveId = gr.ObjId,
                             GraveName = gr.Name,
                             CemeteryId = gr.CemeteryId,
                             ReceiptNo = app.ReceiptNo,
                             GrafNumber = app.GrafNumber,
                             CareTaker = app.CareTaker,
                             PurchaseOfGrave = app.PurchaseOfGrave,
                             ReservationOfGrave = app.ReservationOfGrave,
                             OpenCloseGrave = app.OpenCloseGrave,
                             WideningOfGrave = app.WideningOfGrave,
                             UseOfANiche = app.WideningOfGrave,
                             BurialOfPauper = app.BurialOfPauper,
                             Amount = app.Amount,
                             AmountPaidDate = app.AmountPaidDate

                         });

            return model.ToList();
        }


        public ActionResult GraveOwnerDelete(GraveOwnerDto item, string headerObjId)
        {
            var model = db.GraveOwners;

            if (item.ObjId != null)
            {
                try
                {
                    var GraveOwner = db.GraveOwners.Where(c => c.ObjId == item.ObjId).SingleOrDefault();
                    if (GraveOwner != null)
                        model.Remove(GraveOwner);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            var modelReturn = GetGraveOwnerDto(headerObjId);
            return PartialView("GraveOwnerGridViewPartial", modelReturn);
        }
        #endregion

        #region DualApplication

        public ActionResult DualApplicationLanding()
        {
            return PartialView("DualApplicationLanding");
        }

        public ActionResult DualApplicationPartial(string headerObjId)
        {
            var model = GetDualApplicationDto(headerObjId);
            return PartialView("DualApplicationPartialView", model.ToList());
        }

        public ActionResult DualApplicationsUpdateEntryToForm(Guid ObjId, DualApplication model)
        {
            var DualApplicationsInfo = db.DualApplications.Where(s => s.ObjId == ObjId).FirstOrDefault();

            if (DualApplicationsInfo == null)
            {
                model.ObjId = ObjId;
                return PartialView("DualCreateApplicationsEditPartial", model);
            }

            if (DualApplicationsInfo != null)
            {
                return PartialView("DualCreateApplicationsEditPartial", DualApplicationsInfo);
            }

            return null;

        }

        public ActionResult AddDualApplication(string headerObjId, DualApplication Item)
        {
            Guid AppObjId = Guid.Parse(headerObjId);

            var model = db.DualApplications;
            var exists = model.Where(c => c.ObjId == Item.ObjId).SingleOrDefault();

            if (exists == null)
            {
                Item.HeaderApplicationId = AppObjId;
                model.Add(Item);
                db.SaveChanges();
            }
            if (exists != null)
            {

                this.UpdateModel(exists);
                // model.Attach(userRole);
                db.SaveChanges();

            }
            var Dto = GetDualApplicationDto(headerObjId);
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("DualApplicationPartialView", Dto);

        }

        private List<DualApplication> GetDualApplicationDto(string headerObjId)
        {
            var model = db.DualApplications.Where(c => c.HeaderApplicationId.ToString() == headerObjId);

            return model.ToList();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDualApplication(DualApplication item, string headerObjId)
        {
            var model = db.DualApplications;

            if (item.ObjId != null)
            {
                try
                {
                    var DualApplication = db.DualApplications.Where(c => c.ObjId == item.ObjId).SingleOrDefault();
                    if (DualApplication != null)
                        model.Remove(DualApplication);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            var modelReturn = GetDualApplicationDto(headerObjId);
            return PartialView("DualApplicationPartialView", modelReturn);
        }
        #endregion

        #region Graves
        public ActionResult GravesGridViewPartial()
        {
            var GravesRecords = db.Graves.OrderBy(c => c.Name).ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GravesGridViewPartialView", GravesRecords);
        }
        #endregion

        #region Attachment Upload

        public ActionResult UploadControlUpload()
        {

            UploadControlExtension.GetUploadedFiles("UploadControl", FileLoadingControllerUploadControlSettings.UploadValidationSettings, FileLoadingControllerUploadControlSettings.FileUploadComplete);

            return null;
        }
    }
    public class FileLoadingControllerUploadControlSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".csv", ".xls" },
            MaxFileSize = 4000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
             

            }
        }
    }

    #endregion



}




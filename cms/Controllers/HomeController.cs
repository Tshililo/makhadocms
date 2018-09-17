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
using System.Text;
using System.Web.UI.WebControls;


namespace cms.Controllers
{

	[Authorize]
	public class HomeController : BaseController
    {
     
	//	[Authorize(Roles = "Admin")]
		public ActionResult Index()
        {
            return View();
        }

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
            return PartialView("ImportApplication");
        }

        public string SaveRecordsApplication(byte[] fileToUpload, string fileName)
        {

            // create if does not exist
            var dir = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\" + "log";
            System.IO.Directory.CreateDirectory(dir);

            var filenameErrorlog = dir + "\\LogImportErrors" + DateTime.Now.ToFileTime() + ".txt";
            var sw = new System.IO.StreamWriter(filenameErrorlog, true);

            try
            {
                string localDirectoryToCopyFiles = "C:/DropBox/ImportApplications";  // this is the actual folder we will store the files

                sw.WriteLine(DateTime.Now.ToString() + " " + "SaveAttachmentToServerCreateBurial." + localDirectoryToCopyFiles);

                // CREATE ORG FOLDER IF IT DOES NOT EXIST
                if (!Directory.Exists(@localDirectoryToCopyFiles))
                    Directory.CreateDirectory(@localDirectoryToCopyFiles);

                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = "temp" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv";

                var filedesc = @localDirectoryToCopyFiles + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + " " + fileName;


                sw.WriteLine(DateTime.Now.ToString() + " " + "filedesc." + filedesc);
                sw.WriteLine(DateTime.Now.ToString() + " " + "fileName." + fileName);


                FileInfo file = new FileInfo(filedesc); // we are in the org folder on the server
                try
                {
                    FileStream outStream = file.OpenWrite();

                    outStream.Write(fileToUpload, 0, fileToUpload.Length);
                    outStream.Flush();
                    outStream.Close();

                    DataSet dt = ReadCsvIntoDataSet(file.FullName);

                    foreach (DataTable table in dt.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            #region GetValues
                            var Address = row["Address"].ToString();
                            var IdNo = row["IdNo"].ToString();
                            var DeedName = row["Deed Name"].ToString();
                        
                            var Religion = row["Religion"].ToString();
                            var DateOfBirth = row["DateOfBirth"].ToString();
                            var PlaceOfIssue = row["PlaceOfIssue"].ToString();
                            var AgeGroup = row["AgeGroup"].ToString();
                            var DeedGender = row["DeedGender"].ToString();
                            var DeathAge = row["DeathAge"].ToString();
                            var Buried = row["Buried"].ToString();
                            var PlaceOfBurial = row["Place Of Burial"].ToString();
                            var DateOfBurial = row["Date Of Burial"].ToString();
                            var UsualResidence = row["Usual Residence"].ToString();
                            var CapturedDate = row["Captured Date"].ToString();
                            var Capturer = row["Capturer"].ToString();
                            var Amount = row["Amount"].ToString();
                            var AmountPaidDate = row["Amount Paid Date"].ToString();
                            #endregion

                            #region DoValidations
                            var IdNoisnull = string.IsNullOrWhiteSpace(IdNo) ? null : IdNo;
                            if (IdNoisnull == null)
                            {
                                return "ID No cannot be Empty.";
                            }

                            var Religionisnull = string.IsNullOrWhiteSpace(Religion) ? null : Religion;
                            if (Religionisnull == null)
                            {
                                return "Religion cannot be Empty.";
                            }

                            var AgeGroupisnull = string.IsNullOrWhiteSpace(AgeGroup) ? null : AgeGroup;
                            if (AgeGroupisnull == null)
                            {
                                return "Age Group cannot be Empty.";
                            }

                            var DeedNameisnull = string.IsNullOrWhiteSpace(DeedName) ? null : DeedName;
                            if (DeedNameisnull == null)
                            {
                                return "Deed Name cannot be Empty.";
                            }

                            bool Isburied = false;

                            var Buriedisnull = string.IsNullOrWhiteSpace(Buried) ? null : Buried;
                            if (Buriedisnull != null)
                            {
                                Isburied = Buriedisnull == "yes" ? true : false;
                            }

                            decimal? amountpaid = 0;

                            var Amountisnull = string.IsNullOrWhiteSpace(Amount) ? null : Amount;
                            if (Amountisnull != null)
                            {
                                amountpaid = decimal.Parse(Amount);
                            } 
                            #endregion

                            DateTime? DateofBirth = null;
                            DateTime? Dateofburial = null;
                            DateTime? DateofPayment = null;
                            var modelRepo = db.Applications;
                            var exists = modelRepo.Where(c => c.IdNo == IdNo).SingleOrDefault();
                            Application existsToSave = new Application();

                            #region AssignValues
                            existsToSave.IdNo = IdNo;
                            existsToSave.DeedName = DeedName;
                            existsToSave.DateOfBirth = DateOfBirth != null ? DateTime.Parse(DateOfBirth) : DateofBirth;
                            existsToSave.DateOfBurial = DateOfBurial != null ? DateTime.Parse(DateOfBurial) : Dateofburial;
                            existsToSave.PlaceOfIssue = PlaceOfIssue;
                            existsToSave.PlaceOfBurial = PlaceOfBurial;
                            existsToSave.ReligionId = Religion;
                            existsToSave.AgeGroupId = AgeGroup;
                            existsToSave.AgeGroup = AgeGroup;
                            //exists.ReceiptNo = rece
                            // exists.GrafNumber = item.GrafNumber;
                            existsToSave.Address = Address;
                            existsToSave.Burial_Status = Isburied;
                            existsToSave.UsualResidence = UsualResidence;
                            existsToSave.DeathAge = DeathAge;
                            //exists.Mortuary = item.Mortuary;
                            existsToSave.DeedGender = DeedGender;
                            //exists.CareTaker = care
                            // exists.PurchaseOfGrave = pur
                            existsToSave.Amount = amountpaid;
                            existsToSave.AmountPaidDate = AmountPaidDate != null ? DateTime.Parse(AmountPaidDate) : DateofPayment;
                            #endregion

                            if (exists == null)
                            {
                                modelRepo.Add(existsToSave);
                                db.SaveChanges();
                                return "Successfuly Added";
                            }

                            else
                            {
                                return "Record Already Exists";
                            }
                           
                        }
                    }
                }
                catch (Exception e)
                {
                    e.GetBaseException().Message.ToString();
                }
            }
            catch (Exception e)
            {
              e.GetBaseException().Message.ToString();
            }
            return null;
        }


        public PartialViewResult SaveFileUploadPopUp()
        {
            FileUpload data = new FileUpload();

            return PartialView("EditFileUploadPopUp", data);
        }

		#region BurialRecordApplication
		public ActionResult ApplicationsUpdateEntryToForm(Guid ObjId)
		{
			ViewData["Mortuaries"] = GetMortuary();

            var Applications = db.Applications;

            var Mortuaries = db.Mortuaries;

            ViewData["Mortuaries"] = Mortuaries.ToList();


           var mymodel = ReadApplication();

            if (mymodel != null)
            {
                var ApplicationsInfo = mymodel.Where(s => s.ObjId == ObjId).FirstOrDefault();
                ApplicationsDTO model = new ApplicationsDTO();
                if (ApplicationsInfo == null)
                {

                    model.ObjId = ObjId;
                    return PartialView("CreateApplicationsEditPartial", model);
                }

                if (ApplicationsInfo != null)
                {
                    return PartialView("CreateApplicationsEditPartial", ApplicationsInfo);
                }

            }


            return null;

		}



        public ActionResult ApplicationsGridViewPartial()
        {
            var query = ReadApplication();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", query);
        }

        private List<ApplicationsDTO> ReadApplication()
        {
            var model = db.Applications;
            var Mortuaries = db.Mortuaries;

            var query =  from ap in model
                   from mo in Mortuaries.Where(c => c.ObjId == ap.MortuaryId).DefaultIfEmpty()
                   select new ApplicationsDTO
                   {
                       IdNo = ap.IdNo,
                       ObjId = ap.ObjId,
                       DeedName = ap.DeedName,
                       Address = ap.Address,
                       DateOfBirth = ap.DateOfBirth,
                       DateOfBurial = ap.DateOfBurial,
                       PlaceOfIssue = ap.PlaceOfIssue,
                       AgeGroup = ap.AgeGroup,
                       MortuaryName = mo.Name,
                       MortuaryId = mo.ObjId,
                       ReligionId = ap.ReligionId,
                       DeedGender = ap.DeedGender,
                       DeathAge = ap.DeathAge,
                       CapturedDate = ap.CapturedDate,
                       Burial_Status = ap.Burial_Status,
                       UsualResidence = ap.UsualResidence,
                       ReceiptNo = ap.ReceiptNo,
                       CareTaker = ap.CareTaker,
                       Amount = ap.Amount,
                       AmountPaidDate = ap.AmountPaidDate
                   };

            return query.ToList();
        }

        public ActionResult ApplicationsEdit(ApplicationsDTO item)
		{
			var modelRepo = db.Applications;
			var exists = modelRepo.Where(c => c.ObjId == item.ObjId).SingleOrDefault();
			Application Tosave = new Application();
			if (exists == null)
			{
                CopyProperties(item, Tosave);
				modelRepo.Add(Tosave);
				db.SaveChanges();
			}
			if (exists != null)
			{
                CopyProperties(item, exists);
				modelRepo.Attach(exists);
				db.SaveChanges();
			}

            var BurialRecords = ReadApplication();
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
			var BurialRecords =  ReadApplication();
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
            rep.FromDate.Value = p[2];
            rep.ToDate.Value = p[4];
            rep.ShowDual.Value = p[6];
            return rep;
        }
        #endregion

        #region DropBox
        public ActionResult FileManagerPartial(string reportParams)
        {
			string RootFolder;
			if (reportParams == null)
			{
				RootFolder = @"C:\DropBox\";
				return PartialView("_FileManagerPartial", RootFolder);
			}

			if (reportParams.Contains("C:"))
			{
				return PartialView("_FileManagerPartial", reportParams);
			}
			var model = db.Applications.Where(c => c.ObjId.ToString() == reportParams).FirstOrDefault();
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

			//var RootFolder = @"~\Content\DropBox";
			//return PartialView("_FileManagerPartial", RootFolder);
		}



        public FileStreamResult FileManagerPartialDownload()
        {
            return null;
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
            var GraveOwners = db.GraveOwners.ToList();
            var Graves = db.Graves.ToList();
            var Cemeteries = db.Cemeteries.ToList();

            var model = (from go in GraveOwners.Where(c => c.ApplicationId.ToString() == headerObjId)
                         from gr in Graves.Where(c => c.ObjId == go.GraveId)
                         from cm in Cemeteries.Where(c => c.ObjId == gr.CemeteryId)
                         from app in db.Applications.Where(c => c.ObjId == go.ApplicationId)
                         select new GraveOwnerDto
                         {
                             ObjId = go.ObjId,
                             ApplicationId = app.ObjId,
                             GraveId = gr.ObjId,
                             GraveName = gr.Name,
                             CemeteryName = cm.Name,
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

                try
                {
                    var ToUpload = new cms.Controllers.HomeController().SaveRecordsApplication(e.UploadedFile.FileBytes, e.UploadedFile.FileName);
                    e.UploadedFile.IsValid = false;
                    e.ErrorText = ToUpload;
                }
                catch (Exception ex)
                {
                    e.UploadedFile.IsValid = false;
                    e.ErrorText = "Custom Error Text " + ex.Message;
                }

            }

        }
    }

    #endregion



}




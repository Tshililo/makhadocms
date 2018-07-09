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
	public class HomeController : Controller
    {
        cmsEntities1 db = new cmsEntities1();

	//	[Authorize(Roles = "Admin")]
		public ActionResult Index()
        {
            return View();
        }

        private List<Mortuary> GetMortuary()
        {
            return db.Mortuaries.ToList();
        }

		private DataSet ReadCsvIntoDataSet(string fileFullPath)
		{
			System.Data.DataSet dSet = new System.Data.DataSet("CSV File");
			try
			{
				dSet = GetData(fileFullPath);
				return dSet;

			}
			catch (Exception e)
			{
			//	"Error Importing CSV File: Error: " + e.InnerException.Message;
			}
			return null;

		}

		private static DataSet GetData(string fileName)
		{
			string strLine;

			string[] strArray;
			char[] charArray = new char[] { ',' };
			DataSet ds = new DataSet();
			DataTable dt = ds.Tables.Add("TheData");
			FileStream aFile = new FileStream(fileName, FileMode.Open);
			StreamReader sr = new StreamReader(aFile);

			strLine = sr.ReadLine();

			strArray = strLine.Split(charArray);

			for (int x = 0; x <= strArray.GetUpperBound(0); x++)
			{
				dt.Columns.Add(strArray[x].Trim());
			}

			strLine = sr.ReadLine();
			while (strLine != null)
			{
				strArray = strLine.Split(charArray);
				System.Data.DataRow dr = dt.NewRow();
				for (int i = 0; i <= strArray.GetUpperBound(0); i++)
				{
					dr[i] = strArray[i].Trim();
				}
				dt.Rows.Add(dr);
				strLine = sr.ReadLine();
			}
			sr.Close();
			return ds;
		}

		public string SaveAttachmentToServerCreateBurial(byte[] fileToUpload, string fileName)
		{
			// create if does not exist
			var dir = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\" + "log";
			System.IO.Directory.CreateDirectory(dir);

			var filenameErrorlog = dir + "\\BurialImportlogErrors" + DateTime.Now.ToFileTime() + ".txt";
			var sw = new System.IO.StreamWriter(filenameErrorlog, true);

			try
			{
				string localDirectoryToCopyFiles = "C:/ImportCSV";  // this is the actual folder we will store the files

				sw.WriteLine(DateTime.Now.ToString() + " " + "SaveAttachmentToServerCreatePriceBurial." + localDirectoryToCopyFiles);

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
					var counterAdd = 0;
					var counterSkip = 0;

					foreach (DataTable table in dt.Tables)
					{
						foreach (DataRow row in table.Rows)
						{

							var Idno = row["Id No."].ToString();

							//get week code row
							var mymodel = db.Applications;
							var ApplicationsInfo = mymodel.Where(s => s.IdNo == Idno).FirstOrDefault();
							if (ApplicationsInfo == null)
							{
								// if any week found to be null, report to the user and skip processing
								sw.WriteLine(DateTime.Now.ToString() + " " + "One or More of the Idno do not exists. Please create it and Attempt Import Again");
								return "One or More of the Idno do not exists. Please create it and Attempt Import Again";

							}
						}
					}

			

					foreach (DataTable table in dt.Tables)
					{
						foreach (DataRow row in table.Rows)
						{
							try
							{

								var IdNo = row["Id No."].ToString();
								var DeedName = row["Deed Name"].ToString();
								var DateOfBirth = row["Date Of Birth"].ToString();
								var DateOfBurial = row["Date Of Burial"].ToString();
								var PlaceOfIssue = row["Place Of Issue"].ToString();
								var PlaceOfBurial = row["Place Of Burial"].ToString();
								var ReligionId = row["Religion"].ToString();
								var AgeGroupId = row["Age Group"].ToString();
								var AgeGroup = row["Age Group"].ToString();
								var ReceiptNo = row["Receipt No"].ToString();
								var GrafNumber = row["GrafNumber"].ToString();
								var Burial_Status = row["Buried"].ToString();
								var Address = row["Address"].ToString();

								var UsualResidence = row["Usual Residence"].ToString();
								var DeathAge = row["Death Age"].ToString();
								var Mortuary = row["Mortuary"].ToString();
								var DeedGender = row["Deed Gender"].ToString();
								var CareTaker = row["Id No."].ToString();
								var Amount = row["Amount"].ToString();
								var AmountPaidDate = row["Amount Paid Date"].ToString();

								Application exists = new Application();
								//this.UpdateModel(item);
								exists.IdNo = IdNo;
								exists.DeedName = DeedName;
								exists.DateOfBirth = DateTime.Parse(DateOfBirth);
								exists.DateOfBurial = DateTime.Parse(DateOfBurial);
								exists.PlaceOfIssue = PlaceOfIssue;
								exists.PlaceOfBurial = PlaceOfBurial;
								exists.ReligionId = ReligionId;
								exists.AgeGroupId = AgeGroupId;
								exists.AgeGroup = AgeGroup;
								exists.ReceiptNo = ReceiptNo;
								exists.GrafNumber = GrafNumber;
								exists.Address = Address;
								exists.Burial_Status = Burial_Status == "Yes" ? true : false;
								exists.UsualResidence = UsualResidence;
								exists.DeathAge = DeathAge;
								exists.Mortuary = Mortuary;
								exists.DeedGender = DeedGender;
								exists.CareTaker = CareTaker;
								exists.Amount = decimal.Parse(Amount);
								exists.AmountPaidDate = DateTime.Parse(AmountPaidDate); 
								var modelRepo = db.Applications;
								modelRepo.Attach(exists);
								db.SaveChanges();

							}
							catch (Exception e)
							{
								sw.WriteLine(DateTime.Now.ToString() + " " + "Error Getting Values from Csv File - Check Column Names:" + e.StackTrace.ToString());
								sw.WriteLine(DateTime.Now.ToString() + " " + "Error Getting Values from Csv File - Check Column Names:" + e.Message);
								if (e.InnerException != null)
								{
									sw.WriteLine(DateTime.Now.ToString() + " " + "Error Getting Values from Csv File - Check Column Names:" + e.InnerException);
								}
								sw.Close();

								return "Error Getting Values from Csv File - Check Column Names: " + e.StackTrace.ToString();
							}
						}
						sw.Close();
						return "PriceMatrix Created: " + counterAdd;
					}
				}
				catch (Exception e)
				{
					//AddException(() => true, "Error copying File to In Folder. Attempt File Uploaded Again. Error: " + e.InnerException.Message.ToString(), throwNow: true);
					sw.WriteLine(DateTime.Now.ToString() + " " + "Error copying File to In Folder. Attempt File Uploaded Again. Error:  " + e.StackTrace.ToString());
					sw.WriteLine(DateTime.Now.ToString() + " " + "Error copying File to In Folder. Attempt File Uploaded Again. Error:  " + e.Message);
					if (e.InnerException != null)
					{
						sw.WriteLine(DateTime.Now.ToString() + " " + "Error copying File to In Folder. Attempt File Uploaded Again. Error:" + e.InnerException);
					}
					sw.Close();
					return "Error copying File to In Folder. Attempt File Uploaded Again. Error: ";
				}
			}
			catch (Exception e)
			{

				sw.WriteLine(DateTime.Now.ToString() + " " + "excep " + e.StackTrace.ToString());
				sw.WriteLine(DateTime.Now.ToString() + " " + "excep:  " + e.Message);
				if (e.InnerException != null)
				{
					sw.WriteLine(DateTime.Now.ToString() + " " + "excep:" + e.InnerException);
				}
				sw.Close();
			}
			return "";
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

		#region BurialRecordApplication
		public ActionResult ApplicationsUpdateEntryToForm(Guid ObjId, Application model)
		{
			ViewData["Mortuaries"] = GetMortuary();

			var mymodel = db.Applications;
			var ApplicationsInfo = mymodel.Where(s => s.ObjId == ObjId).FirstOrDefault();

			if (ApplicationsInfo == null)
			{
				model.ObjId = ObjId;
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
			var model = db.Applications;

			// DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
			return PartialView("GridViewPartialView", model.ToList());
		}

		public ActionResult ApplicationsEdit(Application item)
		{
			var modelRepo = db.Applications;
			var exists = modelRepo.Where(c => c.ObjId == item.ObjId).SingleOrDefault();
			Application Tosave = new Application();
			if (exists == null)
			{
				modelRepo.Add(item);
				db.SaveChanges();
			}
			if (exists != null)
			{

				//this.UpdateModel(item);
				exists.IdNo = item.IdNo;
				exists.DeedName = item.DeedName;
				exists.DateOfBirth = item.DateOfBirth;
				exists.DateOfBurial = item.DateOfBurial;
				exists.PlaceOfIssue = item.PlaceOfIssue;
				exists.PlaceOfBurial = item.PlaceOfBurial;
				exists.ReligionId = item.ReligionId;
				exists.AgeGroupId = item.AgeGroupId;
				exists.AgeGroup = item.AgeGroup;
				exists.ReceiptNo = item.ReceiptNo;
				exists.GrafNumber = item.GrafNumber;
				exists.Address = item.Address;
				exists.Burial_Status = item.Burial_Status;
				exists.UsualResidence = item.UsualResidence;
				exists.DeathAge = item.DeathAge;
				exists.Mortuary = item.Mortuary;
				exists.DeedGender = item.DeedGender;
				exists.CareTaker = item.CareTaker;
				exists.PurchaseOfGrave = item.PurchaseOfGrave;
				exists.ReservationOfGrave = item.ReservationOfGrave;
				exists.OpenCloseGrave = item.OpenCloseGrave;
				exists.WideningOfGrave = item.WideningOfGrave;
				exists.UseOfANiche = item.WideningOfGrave;
				exists.BurialOfPauper = item.BurialOfPauper;
				exists.Amount = item.Amount;
				exists.AmountPaidDate = item.AmountPaidDate;
				modelRepo.Attach(exists);
				db.SaveChanges();
			}
	
			// DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
			return PartialView("GridViewPartialView", modelRepo.ToList());

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
            rep.FromDate.Value = p[2];
            rep.ToDate.Value = p[4];
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

				new HomeController().SaveAttachmentToServerCreateBurial(e.UploadedFile.FileBytes, e.UploadedFile.FileName);

			}
        }
    }

    #endregion



}




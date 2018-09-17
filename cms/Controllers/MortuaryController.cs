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
  //  [Authorize]
    public class MortuaryController : BaseController
    {

        // GET: Cemeteries
        public ActionResult Index()
        {
            return View();
        }

        #region Mortuary
        public ActionResult MortuaryUpdateEntryToForm(Guid ObjId)
        {
            var MortuaryInfo = db.Mortuaries.Where(s => s.ObjId == ObjId).FirstOrDefault();

            Mortuary model = new Mortuary();
            if (MortuaryInfo == null)
            {
                model.ObjId = ObjId;
                return PartialView("CreateMortuaryEditPartial", model);
            }

            if (MortuaryInfo != null)
            {

                return PartialView("CreateMortuaryEditPartial", MortuaryInfo);
            }

            return null;

        }

        public ActionResult MortuaryGridViewPartial()
        {
            var MortuaryRecords = db.Mortuaries.OrderBy(c => c.Name).ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", MortuaryRecords);
        }

        public ActionResult MortuaryEdit(Mortuary item)
        {
            var model = db.Mortuaries;
            var exists = model.Where(c => c.ObjId == item.ObjId).SingleOrDefault();

            if (exists == null)
            {
                model.Add(item);
                db.SaveChanges();
            }
            if (exists != null)
            {
                CopyProperties(item, exists);
                this.UpdateModel(exists);
                // model.Attach(userRole);
                db.SaveChanges();
            }
            var MortuaryRecords = db.Mortuaries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", MortuaryRecords);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MortuaryDelete(Guid ObjId)
        {
            var model = db.Mortuaries;
            if (ObjId != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ObjId == ObjId);
                    if (item != null)
                    {
                        model.Remove(item);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var MortuaryRecords = db.Mortuaries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", MortuaryRecords);
        }
        #endregion
    }
}

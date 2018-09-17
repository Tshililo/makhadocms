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
    public class CemeteriesController : BaseController
    {


        // GET: Cemeteries
        public ActionResult Index()
        {
            return View();
        }

        #region Cemetery
        public ActionResult CemeteryUpdateEntryToForm(Guid ObjId, Cemetery model)
        {
            var CemeteriesInfo = db.Cemeteries.Where(s => s.ObjId == ObjId).FirstOrDefault();

            if (CemeteriesInfo == null)
            {
                model.ObjId = ObjId;
                return PartialView("CreateCemeteryEditPartial", model);
            }

            if (CemeteriesInfo != null)
            {
                return PartialView("CreateCemeteryEditPartial", CemeteriesInfo);
            }

            return null;

        }

        public ActionResult CemeteryGridViewPartial()
        {
            var CemeteriesRecords = db.Cemeteries.OrderBy(c => c.Name).ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", CemeteriesRecords);
        }

        public ActionResult CemeteryEdit(Cemetery item)
        {
            var model = db.Cemeteries;
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
            var CemeteriesRecords = db.Cemeteries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", CemeteriesRecords);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CemeteryDelete(Guid ObjId)
        {
            var model = db.Cemeteries;
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
            var CemeteriesRecords = db.Cemeteries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", CemeteriesRecords);
        }
        #endregion
    }
}

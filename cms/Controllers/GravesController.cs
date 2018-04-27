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
    public class GravesController : Controller
    {
        private CMSEntityModel db = new CMSEntityModel();

        // GET: Cemeteries
        public ActionResult Index()
        {
            return View();
        }

        #region Graves

        private List<Cemetery> GetCemeteries()
        {
            return db.Cemeteries.ToList();
        }

        public ActionResult GravesUpdateEntryToForm(Guid ObjId)
        {
            var GravesInfo = db.Graves.Where(s => s.ObjId == ObjId).FirstOrDefault();

            ViewData["GetCemeteries"] = GetCemeteries();

            Grave model = new Grave();

            if (GravesInfo == null)
            {
              
                return PartialView("CreateGravesEditPartial", model);
            }

            if (GravesInfo != null)
            {
                return PartialView("CreateGravesEditPartial", GravesInfo);
            }

            return null;

        }

        public ActionResult GravesGridViewPartial()
        {
            var GravesRecords = db.Graves.OrderBy(c => c.Name).ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", GravesRecords);
        }

        public ActionResult GravesEdit(Grave item)
        {
            var model = db.Graves;
            var exists = model.Where(c => c.Name == item.Name).SingleOrDefault();

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
            var GravesRecords = db.Graves.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", GravesRecords);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GravesDelete(Guid ObjId)
        {
            var model = db.Graves;
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
            var GravesRecords = db.Graves.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", GravesRecords);
        }
        #endregion


    }
}

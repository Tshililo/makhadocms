using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cms;
using cms.Models;

namespace cms.Controllers
{
   // [Authorize]
    public class GravesController : BaseController
    {

        // GET: Cemeteries
        public ActionResult Index()
        {
            return View();
        }

        #region Graves

        private List<CemeteryOwnerDTO> GetCemeteries()
        {
            //return db.Cemeteries.ToList();
            var Cemeteries = db.Cemeteries.ToList();
            var Graves = db.Graves.ToList();

            var query = from Gr in Graves
                        from ap in Cemeteries.Where(c => c.ObjId == Gr.CemeteryId).DefaultIfEmpty()
                        select new CemeteryOwnerDTO
                        {
                            ObjId = Gr.ObjId,
                            Name = Gr.Name,
                            Longitude = Gr.Longitude,
                            Latitude = Gr.Latitude,
                            Status = Gr.Status,
                            CemeteryName = ap.Name
                        };

            var model = query.ToList();

            return model;
        }

        public ActionResult GravesUpdateEntryToForm(Guid ObjId)
        {
            var GravesInfo = GetCemeteries().Where(s => s.ObjId == ObjId).FirstOrDefault();

            var CemeteryInfo = db.Cemeteries.ToList();

            ViewData["GetCemeteries"] = CemeteryInfo;

            CemeteryOwnerDTO model = new CemeteryOwnerDTO();

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
            var GravesRecords = GetCemeteries();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", GravesRecords);
        }

        public ActionResult GravesEdit(CemeteryOwnerDTO item)
        {
            var model = db.Graves;
            var exists = model.Where(c => c.Name == item.Name).SingleOrDefault();

            Grave newItem = new Grave();
            if (exists == null)
            {
                CopyProperties(item, newItem);
                model.Add(newItem);
                db.SaveChanges();
            }
            if (exists != null)
            {
                CopyProperties(item, exists);
                this.UpdateModel(exists);
                // model.Attach(userRole);
                db.SaveChanges();

            }
            var GravesRecords = GetCemeteries();
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
            var GravesRecords = GetCemeteries();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", GravesRecords);
        }
        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class CemeteryController : Controller
    {
        GeoPortalEntities db = new GeoPortalEntities();
        // GET: Cemetery
        public ActionResult Index()
        {
            return View();
        }
        private List<SpaceFeature> GetSpaceFeatures()
        {
            return db.SpaceFeatures.ToList();
        }
        public ActionResult CemeteryUpdateEntryToForm(Guid ObjId)
        {
            var cemeteryInfo = db.Cemeteries.Where(s => s.ObjId == ObjId).FirstOrDefault();
            Cemetery model = new Cemetery();

            ViewData["SpaceFeatures"] = GetSpaceFeatures();

            if (cemeteryInfo == null)
            {

              
                return PartialView("CreateCemeteryEditPartial", model);
            }

            if (cemeteryInfo != null)
            {

                return PartialView("CreateCemeteryEditPartial", cemeteryInfo);
            }

            return null;

        }

        public ActionResult CemeteryGridViewPartial()
        {
            var CemeteryRecords = db.Cemeteries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", CemeteryRecords);
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
            var BurialRecords = db.Cemeteries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", BurialRecords);

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
            var BurialRecords = db.Cemeteries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", BurialRecords);
        }

        [HttpPost]
        public ActionResult QuickUpdate(Guid CemeteryObjId, Guid OwnershipObjId)
        {
            var model = db.CemeteryOwnerships;
            if (ModelState.IsValid)
            {
                try
                {

                    var Exists = db.CemeteryOwnerships
                        .Where(c => c.CemeteryId == CemeteryObjId && c.OwnershipId == OwnershipObjId).Any();

                    if (Exists == false)
                    {
                        var CO = new CemeteryOwnership()
                        {
                            ObjId = Guid.NewGuid(),
                            OwnershipId = OwnershipObjId,
                            CemeteryId = CemeteryObjId
                        }; ;


                        model.Add(CO);
                        db.SaveChanges();
                    }
                }

       
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please add correct all errors.";
            }

            var CemeteryRecords = db.Cemeteries.ToList();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", CemeteryRecords);
        }
    }
}
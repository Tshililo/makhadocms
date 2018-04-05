using cms.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class CemeteryOwnershipController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        GeoPortalEntities db = new GeoPortalEntities();

        private List<CemeteryOwnershipDto> GetCemeteryOwnership()
        {
            var Cemeteryownership = db.CemeteryOwnerships;
            var Ownershiprecords = db.OwnershipRecords;
            var Cemeteries = db.Cemeteries;

            var model = (from repo in Cemeteryownership
                         from or in Ownershiprecords.Where(c => c.ObjId == repo.OwnershipId)
                         from ce in Cemeteries.Where(c => c.ObjId == repo.CemeteryId)
                         select new CemeteryOwnershipDto
                         {
                             ObjId = repo.ObjId,
                             OwnerName = or.OwnerName,
                             OwnerAddress = or.OwnerAddress,
                             Contact = or.Phone,
                             DeedName = or.DeedName,
                             Cemetery_Name = ce.Cemetery_Name,
                             Status = ce.Status,
                             Latitude = ce.Latitude,
                             Longitude = ce.Longitude
                         });

            return model.ToList();
        }

        [ValidateInput(false)]
        public ActionResult CemeteryOwnershipGridViewPartial()
        {
            var model = GetCemeteryOwnership();
            return PartialView("CemeteryOwnershipGridViewPartial", model.ToList());
        }
    }
}
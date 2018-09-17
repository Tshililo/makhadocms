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
  //  [Authorize]
    public class SiteVisitAuditController : BaseController
    {
        // GET: Cemeteries
        public ActionResult Index()
        {
            return View();
        }

        #region SiteVisitAudit
        public ActionResult SiteVisitAuditUpdateEntryToForm(Guid ObjId)
        {
            var SiteVisitAuditInfo = db.SiteVisitAudits.Where(s => s.ObjId == ObjId).FirstOrDefault();

            OnSiteVisitDto model = new OnSiteVisitDto();
            if (SiteVisitAuditInfo == null)
            {
                model.ObjId = ObjId;
                return PartialView("CreateSiteVisitAuditEditPartial", model);
            }

            if (SiteVisitAuditInfo != null)
            {
                CopyProperties(SiteVisitAuditInfo, model);
                return PartialView("CreateSiteVisitAuditEditPartial", model);
            }

            return null;

        }

        public ActionResult SiteVisitAuditGridViewPartial()
        {
          var query = Read();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", query);
        }

        private List<OnSiteVisitDto> Read()
        {
            var SiteVisitAuditInfo = db.SiteVisitAudits.OrderBy(c => c.DateOfVisit).ToList();

            var query = from ap in SiteVisitAuditInfo
                        select new OnSiteVisitDto
                        {
                            ObjId = ap.ObjId,
                            DateOfVisit = ap.DateOfVisit,
                            Purpose = ap.Purpose,
                            Found = ap.GraveFound == true ? "Yes" : "No",
                            GraveFound = ap.GraveFound,
                            Officer = ap.Officer
                        };
            return query.ToList();
        }

        public ActionResult SiteVisitAuditEdit(OnSiteVisitDto item)
        {
            var model = db.SiteVisitAudits;
            var exists = model.Where(c => c.ObjId == item.ObjId).SingleOrDefault();

            if (item.GraveFound == null)
            {
                item.GraveFound = false;
            }
            if (exists == null)
            {
                SiteVisitAudit ToSave = new SiteVisitAudit();
                CopyProperties(item, ToSave);
                model.Add(ToSave);
                db.SaveChanges();
            }
            if (exists != null)
            {
                CopyProperties(item, exists);
                this.UpdateModel(exists);
                // model.Attach(userRole);
                db.SaveChanges();
            }
            var query = Read();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", query);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SiteVisitAuditDelete(Guid ObjId)
        {
            var model = db.SiteVisitAudits;
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
            var query = Read();
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView", query);
        }
        #endregion
    }
}

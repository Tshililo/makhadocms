using System;
using System.Linq;
using System.Web.Mvc;

namespace cms.Controllers
{
    public class ReportHeadersController : BaseController
    {
        // GET: DropBox
        public ActionResult Index()
        {
            return View();
        }

      

        [ValidateInput(false)]
        public ActionResult ReportHeadersGridViewPartial()
        {
            var model = db.ReportHeaders;
            return PartialView("_ReportHeaderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ReportHeadersGridViewPartialAddNew(ReportHeader item)
        {
            var model = db.ReportHeaders;
            if (ModelState.IsValid)
            {
                try
                {
                    var currentRolesToAddExists = db.ReportHeaders.Where(c => c.ObjId == item.ObjId).Any();
                    if (currentRolesToAddExists == false)
                    {
                        item.ObjId = Guid.NewGuid();
                        model.Add(item);
                        db.SaveChanges();
                    }
                    else
                        ViewData["EditError"] = "Report Headers Already Exist";

                }

                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ReportHeaderGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ReportHeadersGridViewPartialUpdate(ReportHeader item)
        {
            var model = db.ReportHeaders;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ObjId == item.ObjId);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ReportHeaderGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ReportHeadersGridViewPartialDelete(System.String ObjId)
        {
            var model = db.ReportHeaders;
            if (ObjId != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ObjId.ToString() == ObjId);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_ReportHeaderGridViewPartial", model.ToList());
        }
    }


}
using DevExpress.Web.Mvc;
using cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Controllers
{
   // [Authorize]
    public class RolesController : BaseController
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult RolesGridViewPartial()
        {
            var model = db.Roles;
            return PartialView("_RolesGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RolesGridViewPartialAddNew(Role item)
        {
            var model = db.Roles;
            if (ModelState.IsValid)
            {
                try
                {
                    var currentRolesToAddExists = db.Roles.Where(c => c.Id == item.Id).Any();
                    if (currentRolesToAddExists == false)
                    {
                        item.Id = Guid.NewGuid();
                        model.Add(item);
                        db.SaveChanges();
                    }
                    else
                        ViewData["EditError"] = "Role Already Exist";

                }

                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_RolesGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult RolesGridViewPartialUpdate(Role item)
        {
            var model = db.Roles;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
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
            return PartialView("_RolesGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult RolesGridViewPartialDelete(System.String Id)
        {
            var model = db.Roles;
            if (Id != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Id.ToString() == Id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_RolesGridViewPartial", model.ToList());
        }
    }
}
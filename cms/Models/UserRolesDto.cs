using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{ 
    public class UserRolesDto
    {
        public List<SelectListItem> UserRoles { get; set; }
        public Guid ObjId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }

        public string RoleId { get; set; }

        public string UserId { get; set; }


    }
}
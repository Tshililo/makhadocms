using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{ 
    public class OnSiteVisitDto
    {
        public System.Guid ObjId { get; set; }
        public Nullable<System.DateTime> DateOfVisit { get; set; }
        public string Purpose { get; set; }

        public string Found { get; set; }
        public string Officer { get; set; }
        public Nullable<bool> GraveFound { get; set; }


    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{ 
    public class CemeteryOwnerDTO
    {
        public System.Guid ObjId { get; set; }
        public string Name { get; set; }
        public string CemeteryName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<System.Guid> CemeteryId { get; set; }
        public string Status { get; set; }


    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{ 
    public class CemeteryOwnershipDto
    {
        public List<SelectListItem> CemeteryOwnerships { get; set; }
        public Guid ObjId { get; set; }
        public Guid OwnershipId { get; set; }
        public Guid CemeteryId { get; set; }

        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string Contact { get; set; }
        public string DeedName { get; set; }

        public string Cemetery_Name { get; set; }
        public string Status { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }


    }
}
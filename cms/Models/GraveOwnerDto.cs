using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{ 
    public class GraveOwnerDto
    {
        public List<SelectListItem> GraveOwners { get; set; }
        public Guid ObjId { get; set; }
        public Guid GraveId { get; set; }
        public Guid ApplicationId { get; set; }
        public string GraveName { get; set; }
        public Guid? CemeteryId { get; set; }
        public string ReceiptNo { get; set; }

        public string CemeteryName { get; set; }
        
        public string CareTaker { get; set; }
        public string GrafNumber { get; set; }

        public Nullable<decimal> PurchaseOfGrave { get; set; }
        public Nullable<decimal> ReservationOfGrave { get; set; }
        public Nullable<decimal> OpenCloseGrave { get; set; }
        public Nullable<decimal> WideningOfGrave { get; set; }
        public Nullable<decimal> UseOfANiche { get; set; }
        public Nullable<decimal> BurialOfPauper { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> AmountPaidDate { get; set; }

    }
}
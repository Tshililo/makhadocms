using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{
	public class ApplicationsDTO
	{
		public System.Guid ObjId { get; set; }
		public string IdNo { get; set; }
		public string DeedName { get; set; }
		public Nullable<System.DateTime> DateOfBirth { get; set; }
		public Nullable<System.DateTime> DateOfBurial { get; set; }
		public string PlaceOfIssue { get; set; }
		public string AgeGroup { get; set; }
		public string MortuaryName { get; set; }		
		public Nullable<decimal> PurchaseOfGrave { get; set; }
		public Nullable<decimal> ReservationOfGrave { get; set; }
		public Nullable<decimal> OpenCloseGrave { get; set; }
		public Nullable<decimal> WideningOfGrave { get; set; }
		public Nullable<decimal> UseOfANiche { get; set; }
		public Nullable<decimal> BurialOfPauper { get; set; }
		public Nullable<decimal> Amount { get; set; }
		public Nullable<System.DateTime> AmountPaidDate { get; set; }
		public string ReceiptNo { get; set; }
		public string PlaceOfBurial { get; set; }
		public string CareTaker { get; set; }
		public string GrafNumber { get; set; }
		public string ReligionId { get; set; }
		public string AgeGroupId { get; set; }
		public Nullable<System.Guid> Mortuary { get; set; }
		public string DeedGender { get; set; }
		public string DeathAge { get; set; }
		public string CauseOfDeath { get; set; }
		public string Address { get; set; }
		public string UsualResidence { get; set; }
		public Nullable<System.DateTime> CapturedDate { get; set; }
		public Nullable<System.DateTime> PurchaseCapturedDate { get; set; }
		public Nullable<bool> Burial_Status { get; set; }
        public Guid? MortuaryId { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{
    public class SummaryDto : ReportBaseDto
    {
        public SummaryDto()
        {
            Lines = new List<SummaryLineDto>();
        }

        public class SummaryLineDto
        {
            public string IdNo { get; set; }
            public string DeedName { get; set; }
            public Nullable<System.DateTime> DateOfBirth { get; set; }
            public Nullable<System.DateTime> DateOfBurial { get; set; }
            public string PlaceOfIssue { get; set; }
            public string AgeGroup { get; set; }
            public Nullable<decimal> Amount { get; set; }
            public Nullable<System.DateTime> AmountPaidDate { get; set; }
            public string ReceiptNo { get; set; }
            public string PlaceOfBurial { get; set; }
            public string CareTaker { get; set; }
            public string GrafNumber { get; set; }
            public string ReligionId { get; set; }
            public string AgeGroupId { get; set; }
            public string Mortuary { get; set; }
            public string DeedGender { get; set; }
            public string DeathAge { get; set; }
            public string CauseOfDeath { get; set; }
            public string Address { get; set; }
            public Nullable<System.DateTime> DateOfDeath { get; set; }
            public string UsualResidence { get; set; }
            public Nullable<System.DateTime> CapturedDate { get; set; }
            public Nullable<System.DateTime> PurchaseCapturedDate { get; set; }
            public string toCompany { get; internal set; }
            public string forAttention { get; internal set; }
        }

        public List<SummaryLineDto> Lines
        {
            get; set;

        }

    }

    public class ReportBaseDto
    {
        public string EmailAddress { get; set; }
        public string FaxNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string VatRegistrationNumber { get; set; }
        public string CompanyName { get; set; }
        public string PhysicalAddress1 { get; set; }
  
    }
}
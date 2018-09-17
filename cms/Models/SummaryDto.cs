using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
            public Nullable<System.DateTime> DateOfBirth { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime? DateOfBurial { get; set; }
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
            public string toCompany { get;  set; }
            public string forAttention { get;  set; }
            public string MortuaryName { get;  set; }
            public bool? Burial_Status { get;  set; }
            public string duIdNo { get;  set; }
            public string duDeedName { get;  set; }
            public string duAgeGroup { get;  set; }
            public string duDeathAge { get;  set; }
            public decimal? duAmount { get;  set; }
            public bool? duBurial_Status { get; set; }
        }

        public List<SummaryLineDto> Lines
        {
            get; set;

        }

    }

    public class ReportBaseDto
    {
        public string OrganisationName { get; set; }
        public string Address { get; set; }
        public string TeNo { get; set; }
        public string Vat { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ToCompany { get; set; }
        public string ForAttention { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? dateTo { get; set; }

    }
}
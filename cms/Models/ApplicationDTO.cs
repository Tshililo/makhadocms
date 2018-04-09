﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{ 
    public class ApplicationDTO
    {
        public List<SelectListItem> UserRoles { get; set; }
        public System.Guid ObjId { get; set; }

        [Required(ErrorMessage = "ID no is required.")]
        [MaxLength(13, ErrorMessage = "ID no cannot be longer than 40 characters.")]
        public string IdNo { get; set; }
        public string DeedName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DateOfBurial { get; set; }
        public string PlaceOfIssue { get; set; }
        public string AgeGroup { get; set; }
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
        public string Mortuary { get; set; }
        public string DeedGender { get; set; }
        public string DeathAge { get; set; }
        public string CauseOfDeath { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        public string UsualResidence { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> CapturedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> PurchaseCapturedDate { get; set; }
        public Nullable<bool> Burial_Status { get; set; }
        public string MortuaryName { get; internal set; }


    }
}
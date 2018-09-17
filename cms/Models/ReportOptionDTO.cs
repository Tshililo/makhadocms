using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Models
{
    public class ReportOtion
    {
        public string To { get; set; }

        public string Attention { get; set; }

        public bool ShowDual { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
    }
}
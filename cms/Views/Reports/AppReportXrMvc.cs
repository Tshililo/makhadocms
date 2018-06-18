﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace cms
{

    public partial class AppReportXrMvc : DevExpress.XtraReports.UI.XtraReport
    {
		cmsEntities1 db = new cmsEntities1();
        public AppReportXrMvc()
		{
            InitializeComponent();
            this.ReportPrintOptions.DetailCount = 10;
        }

		private void AppReportXrMvc_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			Guid objId = Guid.Parse(this.ObjId.Value.ToString());
            string ForAttention = Attention.Value.ToString();

            string dateFrom = FromDate.Value.ToString();         
            string dateTo = ToDate.Value.ToString();

            DateTime? searchfrom = DateTime.Parse(dateFrom);
            DateTime? searchto = DateTime.Parse(dateTo);

            var orgSrc = GetReportDto(ForAttention, searchfrom, searchto);
			this.srcOrganization.DataSource = orgSrc;
		}

        private List<Models.SummaryDto.SummaryLineDto> GetApplicationsDto(string forAttention, DateTime? dateFrom, DateTime? dateTo)
        {
            var model = (from ur in db.Applications
                         select new Models.SummaryDto.SummaryLineDto
                         {
                             IdNo = ur.IdNo,
                             DeedName = ur.DeedName,
                             DateOfBirth =ur.DateOfBirth,
                             forAttention = forAttention,
                             DateOfBurial = ur.DateOfBurial,
                             PlaceOfIssue = ur.PlaceOfIssue,
                             AgeGroup = ur.AgeGroup,
                             MortuaryName = ur.Mortuary,
                             ReligionId = ur.ReligionId,
                             DeedGender = ur.DeedGender,
                             DeathAge = ur.DeathAge,
                             Burial_Status = ur.Burial_Status,
                             Amount = ur.Amount
                         }).Where(c => c.DateOfBurial >= dateFrom && c.DateOfBurial <= dateTo).OrderBy(c => c.DateOfBurial);

            return model.ToList();
        }

        public Models.SummaryDto GetReportDto(string forAttention, DateTime? dateFrom, DateTime? dateTo)
        {
            var ReportHead = db.ReportHeaders.FirstOrDefault();
            var dto = new Models.SummaryDto();
            dto.ForAttention = forAttention;
            dto.DateFrom = dateFrom;
            dto.dateTo = dateTo;
            dto.OrganisationName = ReportHead.OrganisationName;
            dto.Fax = ReportHead.Fax;
            dto.TeNo = ReportHead.TeNo;
            dto.Vat = ReportHead.Vat;
            dto.Address = ReportHead.Address;
            var grouping =  GetApplicationsDto(forAttention, dateFrom, dateTo);
            dto.Lines.AddRange(grouping);



            return dto;
        }

        }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace cms
{

    public partial class AppReportXrMvc : DevExpress.XtraReports.UI.XtraReport
    {
        CMSEntityModel db = new CMSEntityModel();
        public AppReportXrMvc()
		{
            InitializeComponent();
            this.ReportPrintOptions.DetailCount = 10;
        }

		private void AppReportXrMvc_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			Guid objId = Guid.Parse(this.ObjId.Value.ToString());
            string toCompay = ToCompany.Value.ToString();
            string ForAttention = Attention.Value.ToString();

            var orgSrc = GetReportDto(toCompay, ForAttention);
			this.srcOrganization.DataSource = orgSrc;
		}

        private List<Models.SummaryDto.SummaryLineDto> GetApplicationsDto(string toCompany, string forAttention)
        {
            var model = (from ur in db.Applications
                         select new Models.SummaryDto.SummaryLineDto
                         {
                             IdNo = ur.IdNo,
                             DeedName = ur.DeedName,
                             DateOfBirth =ur.DateOfBirth,
                             toCompany = toCompany,
                             forAttention = forAttention
                         });

            return model.ToList();
        }

        public Models.SummaryDto GetReportDto(string toCompany, string forAttention)
        {
            var dto = new Models.SummaryDto();

            var grouping =  GetApplicationsDto(toCompany, forAttention);

            dto.Lines.AddRange(grouping);

            return dto;
        }

        }
}
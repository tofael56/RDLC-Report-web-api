using Microsoft.AspNetCore.Mvc;
using RdlcWebApi.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace RdlcWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportDownloadConroller : ControllerBase
    {
        private IReportService _reportService;
        public ReportDownloadConroller(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("{reportName}/{reportType}")]
        public IActionResult Get(string reportName,string reportType)
        {
            //var stream = new FileStream(@"path\to\file", FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
            var reportbytestream = _reportService.GenerateReportAsync(reportName, reportType);
            return File(reportbytestream,MediaTypeNames.Application.Octet, getReportName(reportName, reportType));
        }

        private  string getReportName(string reportName, string reportType)
        {
            string outputFileExtension = reportName + ".pdf";
            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    outputFileExtension = reportName + ".pdf";
                    break;
                case "XLS":
                    outputFileExtension = reportName + ".xls";
                    break;
                case "WORD":
                    outputFileExtension = reportName + ".doc";
                    break;
            }
            return outputFileExtension;
        }
    }
}

using AspNetCore.Reporting;
using RdlcWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RdlcWebApi.Service
{
    public interface IReportService
    {
        byte[] GenerateReportAsync(string reportName, string repotType);
    }
    public class ReportService : IReportService
    {
        public byte[] GenerateReportAsync(string reportName, string repotType)
        {
            string fileDirectory = Assembly.GetExecutingAssembly().Location.Replace("RdlcWebApi.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}ReportFile\\{1}.rdlc", fileDirectory, reportName);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");
            LocalReport report = new LocalReport(rdlcFilePath);

            //data

            List<ProductModel> productList = new List<ProductModel>();
            var producta1 = new ProductModel { productcode = "001", ProductName = "Test1", Color = "Red",   Size = "4" };
            var producta2 = new ProductModel { productcode = "002", ProductName = "Test2", Color = "Green", Size = "5" };
            var producta3 = new ProductModel { productcode = "003", ProductName = "Test3", Color = "Black", Size = "3" };
            var producta4 = new ProductModel { productcode = "004", ProductName = "Test4", Color = "White", Size = "5" };
            var producta5 = new ProductModel { productcode = "005", ProductName = "Test5", Color = "Lemon", Size = "0" };
            
            productList.Add(producta1);
            productList.Add(producta2);
            productList.Add(producta3);
            productList.Add(producta4);
            productList.Add(producta5);

            report.AddDataSource("DsProductList", productList);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var result = report.Execute(GetRenderType(repotType),1,parameters);

            return result.MainStream;
        }
        private RenderType GetRenderType(string reportType)
        {
            var rendertype = RenderType.Pdf;
            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    rendertype = RenderType.Pdf;
                    break;
                case "XLS":
                    rendertype = RenderType.Excel;
                    break;
                case "WORD":
                    rendertype = RenderType.Word;
                    break;
            }
            return rendertype;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.Authentication;
using PagedList;
using AD_Team10.ViewModels;
using Microsoft.ML;
using System.Web.Helpers;
using System.IO;
using System.Diagnostics;
using System.Web.Routing;
using System.Runtime.Serialization.Json;
using Microsoft.ML.Data;

namespace AD_Team10.Controllers.Store
{
    [CustomAuthorize(Roles = "CLERK")]
    public class ClerkController : Controller
    {
        static readonly int PAGE_SIZE = 8;
        private DBContext db = new DBContext();
        public ActionResult Index()
        {
            return View("~/Views/Store/Clerk/Index.cshtml");
        }

        public ActionResult Inventory(int? page, string stockLevel, string sort, string searchStr = "")
        {
            var items = db.Items.ToList();
            items = items.Where(i => i.Description.ToLower().Contains(searchStr.ToLower())).ToList();
            if (stockLevel == null) stockLevel = "default";
            if (sort == null) sort = "default";
            if (stockLevel == "low") items = items.Where(i => i.UnitsInStock < i.ReorderLevel).ToList();
            else if(stockLevel == "enough") items = items.Where(i => i.UnitsInStock >= i.ReorderLevel).ToList();
            else { items = items.ToList(); }
            switch (sort)
            {
                case "stockAsc":
                    items = items.OrderBy(i => i.UnitsInStock).ToList();
                    break;
                case "stockDesc":
                    items = items.OrderByDescending(i => i.UnitsInStock).ToList();
                    break;
                case "reorderAsc":
                    items = items.OrderBy(i => i.ReorderLevel).ToList();
                    break;
                case "reorderDesc":
                    items = items.OrderByDescending(i => i.ReorderLevel).ToList();
                    break;
                default:
                    items = items.ToList();
                    break;
            }
            ViewBag.searchStr = searchStr;
            ViewBag.stockLevel = stockLevel;
            ViewBag.sort = sort;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/Inventory.cshtml", items.ToPagedList(pageNumber, PAGE_SIZE));
        }

        public ActionResult AdjustmentVoucher(string status, int? page)
        {
            var vouchers = db.AdjustmentVouchers.OrderBy(v => v.AdjustmentVoucherID).ToList();
            if(status!=null) vouchers = vouchers.Where(v => v.Status.ToString() == status).ToList();
            ViewBag.status = status;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/AdjustmentVoucher.cshtml", vouchers.ToPagedList(pageNumber, PAGE_SIZE));
        }

        public ActionResult VoucherDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentVoucher voucher= db.AdjustmentVouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Store/Clerk/VoucherDetails.cshtml", voucher);
        }


        public ActionResult CreateVoucher()
        {
            var item = db.Items;
            ViewBag.itemList = item.ToList();

            return View("~/Views/Store/Clerk/CreateVoucher.cshtml");
        }

        [HttpPost]
        public JsonResult CreateVoucher(List<AdjustmentVoucherDetail> newVoucherDetails)
        {
            if (newVoucherDetails == null)
            {
                return Json(0);
            }
            else
            {
                AdjustmentVoucher newVoucher = new AdjustmentVoucher { AdjustmentDate = DateTime.Now };
                db.AdjustmentVouchers.Add(newVoucher);
                db.SaveChanges();
                int voucherID = newVoucher.AdjustmentVoucherID;
                newVoucherDetails = CheckDuplicateInVoucher(newVoucherDetails);
                foreach (AdjustmentVoucherDetail newVoucherDetail in newVoucherDetails)
                {
                    newVoucherDetail.VoucherID = voucherID;
                    db.AdjustmentVoucherDetails.Add(newVoucherDetail);
                }
                int record = db.SaveChanges();
                return Json(record);
            }
        }

        public List<AdjustmentVoucherDetail> CheckDuplicateInVoucher(List<AdjustmentVoucherDetail> voucherDetails)
        {
            var results = voucherDetails
                 .GroupBy(p => p.ItemID)
                 .Select(g => new AdjustmentVoucherDetail { ItemID = g.Key, Quantity = g.Sum(i => i.Quantity),
                     Reason = string.Concat(g.Select(r => r.Reason + ": " + r.Quantity.ToString() + Environment.NewLine))
                 })
                 .ToList();
            return results;
        }
        
        public ActionResult RequisitionReport()
        {
            var departments = db.Departments.ToList();
            var items = db.Items.ToList();
            ViewBag.departments = departments;
            ViewBag.items = items;
            return View("~/Views/Store/Clerk/RequisitionReport.cshtml");
        }

        [HttpPost]
        public ActionResult RequisitionReport(Report report)
        {
            TempData["report"] = report;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            Report report = TempData["report"] as Report;
            ViewBag.report = report;

            return PartialView("~/Views/Store/Clerk/_Report.cshtml");
    }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public ActionResult ShowChart(string report, string type)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Report));
            var re = (Report)ser.ReadObject(GenerateStreamFromString(report));
            string[][] data = GetData(re);
            string[] x = data[0];
            string[] y = data[1];
            var chart = new Chart(width: 600, height: 400)
            .AddTitle("Chart Title")
            .AddSeries(
                chartType: type,
                xValue: x,
                yValues: y)
            .GetBytes("png");
            return File(chart, "image/bytes");
        }
 
        public string[][] GetData(Report report)
        {
            DateTime startDate = Convert.ToDateTime(report.StartDate);
            DateTime endDate = Convert.ToDateTime(report.EndDate);
            var data = db.RequisitionDetails
                 .Where(r => r.Requisition.RequisitionDate >= startDate &&
                             r.Requisition.RequisitionDate <= endDate &&
                             report.ItemList.Contains(r.ItemID) && report.DepartmentList.Contains(r.Requisition.Employee.Department.DepartmentID))
                 .GroupBy(r => r.Requisition.RequisitionDate)
                 .Select(r => new { r.Key, Quantity = r.Sum(b => b.Quantity) });
            TempData["data"] = data.ToList();
            string[] x = data.Select(r => r.Key.ToString()).ToArray();
            string[] y = data.Select(r => r.Quantity.ToString()).ToArray();
            string[][] result = new string[][] { x, y };
            Debug.WriteLine(x.Length);
            Debug.WriteLine(y.Length);
            return result;
        }

        public void Forecast()
        {
            var data = db.RequisitionDetails
                 .GroupBy(r => r.Requisition.RequisitionDate)
                 .Select(r => new { r.Key, Quantity = r.Sum(b => b.Quantity) }).ToList();
            MLContext mlContext = new MLContext();
            ModelInput[] input = new ModelInput[data.Count() - 4];
            for(int i=0; i<data.Count() - 4; i++)
            {
                input[i] = new ModelInput { HistoricalQuantity = 
                                                new float[] {data[i].Quantity, data[i+1].Quantity,
                                                data[i+2].Quantity, data[i+3].Quantity},
                                            Quantity = data[i+4].Quantity};
            }
            IDataView inputData = mlContext.Data.LoadFromEnumerable(input);

            IEstimator<ITransformer> dataPrepEstimator =
                            mlContext.Transforms.Concatenate("Features", "HistoricalQuantity")
                            .Append(mlContext.Transforms.NormalizeMinMax("Features"));

            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(inputData);

            IDataView transformedData = dataPrepTransformer.Transform(inputData);

            var sdcaEstimator = mlContext.Regression.Trainers.Sdca();

            var trainedModel = sdcaEstimator.Fit(transformedData);

            PredictionEngine<ModelInput, ModelOutput> predictionEngine = 
                mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);

            ModelInput modelInput = new ModelInput
            {
                HistoricalQuantity = new float[] {
                    data[data.Count()].Quantity,
                    data[data.Count()-1].Quantity,
                    data[data.Count()-2].Quantity,
                    data[data.Count()-3].Quantity}
            };
            ModelOutput output = predictionEngine.Predict(modelInput);
            Debug.WriteLine(output);
        }

        
        
        
    }
}
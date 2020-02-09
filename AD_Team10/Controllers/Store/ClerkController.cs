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
using System.IO;
using System.Runtime.Serialization.Json;
using MathNet.Numerics;
using System.Data.Entity.SqlServer;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using Chart = System.Web.UI.DataVisualization.Charting.Chart;
using System.Data;
using ClosedXML.Excel;

namespace AD_Team10.Controllers.Store
{
    [CustomAuthorize(Roles = "CLERK")]
    public class ClerkController : Controller
    {
        static readonly int PAGE_SIZE = 8;
        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            ViewBag.requisitionList = db.Requisitions.OrderByDescending(r => r.RequisitionDate).Take(PAGE_SIZE).ToList();
            ViewBag.orderList = db.PurchaseOrders.OrderByDescending(p => p.OrderDate).Take(PAGE_SIZE).ToList();
            ViewBag.voucherList = db.AdjustmentVouchers.OrderByDescending(v => v.AdjustmentDate).Take(PAGE_SIZE).ToList();
            ViewBag.stock = db.Items.OrderBy(i => i.UnitsInStock / i.ReorderLevel).Take(PAGE_SIZE).ToList();
            return View("~/Views/Store/Clerk/Index.cshtml");
        }

        public ActionResult Inventory(int? page, string stockLevel = "", string sort = "", string searchStr = "")
        {
            var items = db.Items.ToList();
            items = items.Where(i => i.Description.ToLower().Contains(searchStr.ToLower())).ToList();
            if (stockLevel == "low") items = items.Where(i => i.UnitsInStock < i.ReorderLevel).ToList();
            else if (stockLevel == "enough") items = items.Where(i => i.UnitsInStock >= i.ReorderLevel).ToList();
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

        public ActionResult AdjustmentVoucher(int? page, string status = "", string clerk = "")
        {
            ViewBag.statusList = new List<string> { VoucherStatus.Pending.ToString(),
                    VoucherStatus.Rejected.ToString(), VoucherStatus.Supervisor_Approved.ToString(), VoucherStatus.Manager_Approved.ToString()};
            ViewBag.clerkList = db.AdjustmentVouchers.Select(a => a.Clerk).Distinct().ToList();
            var vouchers = db.AdjustmentVouchers.OrderBy(v => v.AdjustmentVoucherID).ToList();
            if (status != "") vouchers = vouchers.Where(v => v.Status.ToString() == status).ToList();
            if (clerk != "") vouchers = vouchers.Where(v => v.StoreEmployeeID == Convert.ToUInt32(clerk)).ToList();
            ViewBag.status = status;
            ViewBag.clerk = clerk;
            int pageNumber = (page ?? 1);
            return View("~/Views/Store/Clerk/AdjustmentVoucher.cshtml", vouchers.ToPagedList(pageNumber, PAGE_SIZE));
        }

        public ActionResult VoucherDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentVoucher voucher = db.AdjustmentVouchers.Find(id);
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
                CustomPrincipal currenUser = (CustomPrincipal)System.Web.HttpContext.Current.User;
                AdjustmentVoucher newVoucher = new AdjustmentVoucher
                {
                    AdjustmentDate = DateTime.Now,
                    Status = VoucherStatus.Pending,
                    StoreEmployeeID = currenUser.UserID
                };
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
                return Json(voucherID);
            }
        }

        public List<AdjustmentVoucherDetail> CheckDuplicateInVoucher(List<AdjustmentVoucherDetail> voucherDetails)
        {
            var results = voucherDetails
                 .GroupBy(p => new { p.ItemID, p.Reason })
                 .Select(g => new AdjustmentVoucherDetail
                 {
                     ItemID = g.Key.ItemID,
                     Quantity = g.Sum(p => p.Quantity),
                     Reason = g.Key.Reason
                 })
                 .ToList();
            return results;
        }

        public ActionResult RequisitionReport()
        {
            var departments = db.Departments.ToList();
            var categories = db.Categories.ToList();
            ViewBag.departments = departments;
            ViewBag.categories = categories;
            return View("~/Views/Store/Clerk/RequisitionReport.cshtml");
        }

        public ActionResult OrderReport()
        {
            var categories = db.Categories.ToList();
            ViewBag.categories = categories;
            return View("~/Views/Store/Clerk/OrderReport.cshtml");
        }

        [HttpPost]
        public ActionResult RequisitionReport(RequisitionReport reqReport)
        {
            TempData["reqReport"] = reqReport;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderReport(OrderReport orderReport)
        {
            TempData["orderReport"] = orderReport;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowReqReport()
        {
            RequisitionReport reqReport = TempData["reqReport"] as RequisitionReport;
            if (reqReport.CategoryList == null)
                reqReport.CategoryList = db.Categories.Select(c => c.CategoryId).ToList();

            if (reqReport.DepartmentList == null)
                reqReport.DepartmentList = db.Departments.Select(d => d.DepartmentID).ToList();
            GetRequisitionData(reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt);
            DataTable dt = GetTable(categories, out int[] catSize, out string tablePeriod, timeDt, reqReport.StartDate, reqReport.EndDate);
            

            ViewBag.report = reqReport;
            ViewBag.catSize = catSize;
            ViewBag.tableName = "Department requisition quantity by category from " + tablePeriod;
            return PartialView("~/Views/Store/Clerk/_Report.cshtml", dt);
        }

        public ActionResult ShowOrderReport()
        {
            OrderReport orderReport = TempData["orderReport"] as OrderReport;
            if (orderReport.CategoryList == null)
                orderReport.CategoryList = db.Categories.Select(c => c.CategoryId).ToList();
            GetOrderData(orderReport, out string[] categories, out ReportByTimeSeries[] timeDt);
            DataTable dt = GetTable(categories, out int[] catSize, out string tablePeriod, timeDt, orderReport.StartDate, orderReport.EndDate);
            ViewBag.report = orderReport;
            ViewBag.catSize = catSize;
            ViewBag.tableName = "Purchase order quantity by category from " + tablePeriod;
            return PartialView("~/Views/Store/Clerk/_Report.cshtml", dt);
        }

        public DataTable GetTable(string[] categories, out int[] catSize, out string tablePeriod,
                                    ReportByTimeSeries[] timeDt, string start, string end)
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);
            catSize = new int[categories.Length];
            string[][] months = new string[categories.Length][];
            double[][] quantity = new double[categories.Length][];
            List<Dictionary<string, double[]>> itemDataByMonth = new List<Dictionary<string, double[]>>();

            for (int i = 0; i < categories.Length; i++)
            {
                GetTimeRange(timeDt, categories[i], startDate, endDate, out string[] xValues, out double[] yValues,
                    out Dictionary<string, double[]> dic);
                months[i] = xValues;
                quantity[i] = yValues;
                itemDataByMonth.Add(dic);
                catSize[i] = dic.Count();
            }             
            tablePeriod = months[0][0] + " - " + months[0][months[0].Length - 1];
            DataTable dt = new DataTable("MyTable");
            dt.Columns.Add(new DataColumn("Category", typeof(string)));
            dt.Columns.Add(new DataColumn("Item", typeof(string)));
            for(int i=0; i<months[0].Length; i++)
            {
                dt.Columns.Add(new DataColumn(months[0][i], typeof(double)));
            }
            for (int i = 0; i < itemDataByMonth.Count(); i++)
            {
                for(int j=0; j<itemDataByMonth[i].Count(); j++)
                {
                    DataRow row = dt.NewRow();
                    row["Category"] = categories[i];
                    row["Item"] = itemDataByMonth[i].ElementAt(j).Key;
                    for(int k=0; k<months[0].Length; k++)
                    {
                        row[months[0][k]] = itemDataByMonth[i].ElementAt(j).Value[k];
                    }
                    dt.Rows.Add(row); 
                }
            }
            return dt;
        }

        public void ExportReqDataTable(string report)
        {
            RequisitionReport reqReport = GetReqReport(report);
            GetRequisitionData(reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt);
            DataTable dt = GetTable(categories, out int[] catSize, out string tablePeriod, timeDt, reqReport.StartDate, reqReport.EndDate);
            string fileName = "Requisition " + tablePeriod.Replace('/', '-');
            ExportData(dt, fileName, catSize, categories);
        }

        public void ExportOrderDataTable(string report)
        {
            OrderReport orderReport = GetOrderReport(report);
            GetOrderData(orderReport, out string[] categories, out ReportByTimeSeries[] timeDt);
            DataTable dt = GetTable(categories, out int[] catSize, out string tablePeriod, timeDt, orderReport.StartDate, orderReport.EndDate);
            DateTime endDate = Convert.ToDateTime(orderReport.EndDate);
            string fileName = "PurchaseOrder " + tablePeriod.Replace('/', '-');
            ExportData(dt, fileName, catSize, categories);
        }

        public void ExportData(DataTable dt, string fileName, int[] catSize, string[] categories)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            XLWorkbook wbook = new XLWorkbook();
            var ws = wbook.Worksheets.Add("Sheet 1");
            var tableWithData = ws.Cell(1, 1).InsertTable(dt.AsEnumerable(), false);
            int startMerge = 2;
            
            for(int i=0; i<catSize.Length; i++)
            {
                int endMerge = startMerge + catSize[i] - 1;
                ws.Cell("A" + startMerge).Value = categories[i];
                ws.Range("A" + startMerge + ":A" + endMerge).Merge();
                startMerge = (endMerge + 1);
            }
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("content-disposition", "attachment;filename=\"" + fileName + ".xlsx\"");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wbook.SaveAs(memoryStream);
                memoryStream.WriteTo(response.OutputStream);
                memoryStream.Close();
            }
            response.End();
        }


        public ActionResult ShowRequisitionChart(string report, string type, string groupBy)
        {
            RequisitionReport reqReport = GetReqReport(report);

            SeriesChartType chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), type);
            GetRequisitionData(reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                    out string[] departments, out ReportByCategory[] catData);

            Chart chart = new Chart
            {
                Width = 1000,
                Height = 500
            };
            chart.ChartAreas.Add("0");
            chart.ApplyPaletteColors();

            if (groupBy.Equals("cat"))
            {
                chart.Titles.Add("Total quantity by category");
                ChartByCat(departments, catData, chart, chartType);
            }

            if (groupBy.Equals("time"))
            {
                chart.Titles.Add("Total quantity of all departments by month");
                ChartByTimeSeries(timeDt, categories, chart,
                                Convert.ToDateTime(reqReport.StartDate), Convert.ToDateTime(reqReport.EndDate), chartType);
            }

            Legend leg = new Legend();
            chart.Legends.Add(leg);
            var imgStream = new MemoryStream();
            chart.SaveImage(imgStream, ChartImageFormat.Png);
            imgStream.Seek(0, SeekOrigin.Begin);

            return File(imgStream, "image/png");
        }

        public ActionResult ShowOrderChart(string report, string type, string groupBy)
        {
            OrderReport orderReport = GetOrderReport(report);
            SeriesChartType chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), type);
            GetOrderData(orderReport, out string[] categories, out ReportByTimeSeries[] timeDt);

            Chart chart = new Chart
            {
                Width = 1000,
                Height = 500
            };
            chart.ChartAreas.Add("0");
            chart.ApplyPaletteColors();

            if (groupBy.Equals("time"))
            {
                chart.Titles.Add("Total order quantity of all categories by month");
                ChartByTimeSeries(timeDt, categories, chart,
                                Convert.ToDateTime(orderReport.StartDate), Convert.ToDateTime(orderReport.EndDate), chartType);
            }

            Legend leg = new Legend();
            chart.Legends.Add(leg);
            var imgStream = new MemoryStream();
            chart.SaveImage(imgStream, ChartImageFormat.Png);
            imgStream.Seek(0, SeekOrigin.Begin);
            return File(imgStream, "image/png");
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

        public OrderReport GetOrderReport(string report)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(OrderReport));
            var orderReport = (OrderReport)ser.ReadObject(GenerateStreamFromString(report));
            return orderReport;
        }

        public RequisitionReport GetReqReport(string report)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RequisitionReport));
            var reqReport = (RequisitionReport)ser.ReadObject(GenerateStreamFromString(report));
            return reqReport;
        }
        static readonly int MAX_VARIABLE = 5;
        static readonly int PREDICT_SIZE = 3;
        public void GetRequisitionData(RequisitionReport reqReport, out string[] categories, out ReportByTimeSeries[] timeDt,
                                    out string[] departments, out ReportByCategory[] catDt)
        {
            DateTime startDate = Convert.ToDateTime(reqReport.StartDate);
            DateTime endDate = Convert.ToDateTime(reqReport.EndDate);
            var data = db.RequisitionDetails
                 .Where(r => r.Requisition.RequisitionDate >= startDate &&
                             r.Requisition.RequisitionDate < endDate &&
                             reqReport.CategoryList.Contains(r.Item.CategoryID) && reqReport.DepartmentList.Contains(r.Requisition.Employee.Department.DepartmentID));
            var timeSeriesData = data
                 .GroupBy(r => new ReportByTimeSeriesKey
                 {
                     Month = SqlFunctions.DatePart("month", r.Requisition.RequisitionDate),
                     Year = SqlFunctions.DateName("year", r.Requisition.RequisitionDate),
                     Category = r.Item.Category.CategoryName
                 })
                 .Select(r => new ReportByTimeSeries { Key = r.Key, Quantity = r.Sum(b => b.Quantity) }).ToList();
            var catData = data
                .GroupBy(r => new ReportByCategoryKey
                {
                    Department = r.Requisition.Employee.Department.DepartmentName,
                    Category = r.Item.Category.CategoryName
                })
                .Select(r => new ReportByCategory { Key = r.Key, Quantity = r.Sum(b => b.Quantity) });

            categories = timeSeriesData.Select(r => r.Key.Category.ToString()).Distinct().ToArray();
            timeDt = timeSeriesData.ToArray();
            departments = catData.Select(r => r.Key.Department.ToString()).Distinct().ToArray();
            catDt = catData.ToArray();
        }

        public void GetOrderData(OrderReport orderReport, out string[] categories, out ReportByTimeSeries[] data)
        {
            DateTime startDate = Convert.ToDateTime(orderReport.StartDate);
            DateTime endDate = Convert.ToDateTime(orderReport.EndDate);
            var query = db.PurchaseOrderDetails
                 .Where(r => r.PurchaseOrder.OrderDate >= startDate &&
                             r.PurchaseOrder.OrderDate < endDate &&
                             orderReport.CategoryList.Contains(r.Item.CategoryID))
                 .GroupBy(r => new ReportByTimeSeriesKey
                 {
                     Month = SqlFunctions.DatePart("month", r.PurchaseOrder.OrderDate),
                     Year = SqlFunctions.DateName("year", r.PurchaseOrder.OrderDate),
                     Category = r.Item.Category.CategoryName
                 })
                 .Select(r => new ReportByTimeSeries { Key = r.Key, Quantity = r.Sum(b => b.Quantity) }).ToList();

            data = query.ToArray();
            categories = query.Select(r => r.Key.Category.ToString()).Distinct().ToArray();
        }

        public double[] Predict(double[] data)
        {
            int totalVariable = 0;
            if (data.Count() > 2)
            {
                if ((data.Count() - MAX_VARIABLE) > MAX_VARIABLE) totalVariable = MAX_VARIABLE;
                else
                {
                    if (data.Count() % 2 == 0) totalVariable = data.Count() / 2 - 1;
                    else totalVariable = (int)Math.Floor((double)data.Count() / 2);
                }
            }

            if (totalVariable == 0) return null;

            double[][] xValues = new double[data.Count() - totalVariable][];
            double[] yValues = new double[data.Count() - totalVariable];
            for (int i = 0; i < data.Count() - totalVariable; i++)
            {
                xValues[i] = new double[totalVariable];
                int numZero = 0;
                for (int j = 0; j < totalVariable; j++)
                {
                    xValues[i][j] = data[i + j];
                    if (data[i + j] == 0) numZero++;
                }
                yValues[i] = data[i + totalVariable];
                if (yValues[i] == 0) numZero++;
                if (numZero > 1)
                {
                    xValues[i] = xValues[i].Select(r => r + 1).ToArray();
                    yValues[i] = yValues[i] + 1;
                }             
            }
            double[] c = new double[totalVariable + 1];
            try
            {
                c = Fit.MultiDim(
                xValues,
                yValues,
                intercept: true);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            double[][] predictedInput = new double[PREDICT_SIZE][];
            double[] predictedOutput = new double[PREDICT_SIZE];
            predictedInput[0] = new double[totalVariable];
            predictedOutput[0] = c[0];
            for (int i = 0; i < totalVariable; i++)
            {
                predictedInput[0][i] = data[data.Count() - 1 - i];
                predictedOutput[0] += c[i + 1] * predictedInput[0][i];
            }
            if (predictedOutput[0] < 0) predictedOutput[0] = 0;
            for (int i = 1; i < PREDICT_SIZE; i++)
            {
                predictedInput[i] = new double[totalVariable];
                predictedOutput[i] = c[0];
                for (int j = 1; j < totalVariable; j++)
                {
                    predictedInput[i][j - 1] = predictedInput[i - 1][j];
                    predictedOutput[i] += c[j] * predictedInput[i][j - 1];
                }
                predictedInput[i][totalVariable - 1] = predictedOutput[i - 1];
                predictedOutput[i] += c[totalVariable] * predictedInput[i][totalVariable - 1];
                if (predictedOutput[i] < 0) predictedOutput[i] = 0;
            }
            return predictedOutput;
        }

        public void ChartByTimeSeries(ReportByTimeSeries[] timeDt, string[] categories, Chart chart,
                                DateTime startDate, DateTime endDate, SeriesChartType chartType)
        {
            for (int i = 0; i < categories.Length; i++)
            {
                GetTimeRange(timeDt, categories[i], startDate, endDate, out string[] xValues, out double[] yValues,
                    out Dictionary<string, double[]> dic);
                chart.Series.Add(new Series(categories[i]));
                //chart.Series[categories[i]].IsValueShownAsLabel = true;
                chart.Series[categories[i]].ChartType = chartType;
                chart.Series[categories[i]].Points.DataBindXY(xValues, yValues);
                for (int k = 1; k <= PREDICT_SIZE; k++)
                {
                    chart.Series[categories[i]].Points[yValues.Length - k].BorderDashStyle = ChartDashStyle.Dash;
                }
            }
        }

        public void GetTimeRange(ReportByTimeSeries[] timeDt, string category, DateTime startDate, DateTime endDate,
            out string[] xValues, out double[] yValues, out Dictionary<string, double[]> dic)
        {
            var query = timeDt.Where(r => r.Key.Category == category)
                          .OrderBy(r => r.Key.Year).ThenBy(r => r.Key.Month);
            string[] x = query.Select(r => String.Format("{0:00}/{1}", r.Key.Month, r.Key.Year)).ToArray();
            double[] y = query.Select(r => r.Quantity).ToArray();
            int diff = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
            DateTime[] dateRange = new DateTime[diff + PREDICT_SIZE];
            dateRange[0] = startDate;
            for (int j = 1; j < dateRange.Length; j++)
            {
                dateRange[j] = dateRange[j - 1].AddMonths(1);
            }

            xValues = dateRange.Select(d => d >= endDate ? string.Format("{0:00}/{1} (predicted)", d.Month, d.Year)
                                                         : string.Format("{0:00}/{1}", d.Month, d.Year))
                                                         .ToArray();
            yValues = new double[xValues.Length];
            for (int j = 0; j < x.Length; j++)
            {
                int keyIndex = Array.FindIndex(xValues, e => e.Equals(x[j]));
                yValues[keyIndex] = y[j];
            }
            var item = db.Items.Where(t => t.Category.CategoryName.Equals(category))
                               .Select(t => t.Description).ToList();

            dic = new Dictionary<string, double[]>();
            for (int j = 0; j < item.Count(); j++)
            {

                double[] itemQuantity = new double[xValues.Length];

                for (int k = 0; k < (itemQuantity.Length); k++)
                {
                    string[] s = xValues[k].Replace("(predicted)", "").Split('/');
                    int? month = Convert.ToInt32(s[0]);
                    int? year = Convert.ToInt32(s[1]);
                    string itemName = item[j];
                    var query2 = db.RequisitionDetails.Where(r =>
                               SqlFunctions.DatePart("month", r.Requisition.RequisitionDate) == month
                               && SqlFunctions.DatePart("year", r.Requisition.RequisitionDate) == year
                               && r.Item.Description.Equals(itemName)).ToList();
                    double qty = 0;
                    if (query2 != null) qty = query2.Select(r => r.Quantity).Sum();
                    itemQuantity[k] = qty;
                }

                double[] data = itemQuantity.Take(itemQuantity.Length - PREDICT_SIZE).ToArray();
                double[] predictedQuantity = Predict(data);
                for (int l = PREDICT_SIZE; l >= 1; l--)
                {

                    itemQuantity[itemQuantity.Length - l] = predictedQuantity[l - 1];
                }
                dic.Add(item[j], itemQuantity);
            }

            for (int i = PREDICT_SIZE; i >= 1; i--)
            {
                double sum = 0;
                for(int j=0; j < dic.Count(); j++)
                {
                    sum += dic.ElementAt(j).Value[yValues.Length - i];
                }
                yValues[yValues.Length - i] = sum;
            }
        }

        public void ChartByCat(string[] departments, ReportByCategory[] catData, Chart chart, SeriesChartType chartType)
        {
            for (int i = 0; i < departments.Length; i++)
            {
                var dt = catData.Where(r => r.Key.Department.Equals(departments[i]));
                string[] xValues = dt.Select(r => r.Key.Category).Distinct().ToArray();
                double[] yValues = dt.GroupBy(r => r.Key.Category)
                    .Select(r => r.Sum(b => b.Quantity)).ToArray();
                chart.Series.Add(new Series(departments[i]));
                //chart.Series[categories[i]].IsValueShownAsLabel = true;
                chart.Series[departments[i]].ChartType = chartType;
                chart.Series[departments[i]].Points.DataBindXY(xValues, yValues);
            }
        }


    }
}
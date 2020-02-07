using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Net;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;

namespace AD_Team10.Controllers.Store
{
    [CustomAuthorize(Roles = "MANAGER")]
    public class ManagerController : Controller
    {
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            var vouchers = db.AdjustmentVouchers.Where(v => v.Status == VoucherStatus.Supervisor_Approved).ToList();
            return View("~/Views/Store/Manager/Index.cshtml", vouchers);
        }

        public void ApproveVoucher (int voucherId )
        {
            AdjustmentVoucher adjustmentVoucher = FindVoucherById(voucherId);
            if(adjustmentVoucher != null)
            {
                adjustmentVoucher.Status = VoucherStatus.Manager_Approved;
                db.SaveChanges();
            }
        }

        public void RejectVoucher(int voucherId)
        {
            AdjustmentVoucher adjustmentVoucher = FindVoucherById(voucherId);
            if(adjustmentVoucher != null)
            {
                adjustmentVoucher.Status = VoucherStatus.Rejected;
                db.SaveChanges();
            }
        }

        public AdjustmentVoucher FindVoucherById(int voucherId)
        {
            return db.AdjustmentVouchers.SingleOrDefault(v => v.AdjustmentVoucherID == voucherId);
        }

        public ActionResult Partial()
        {
            var vouchers = db.AdjustmentVouchers.Where(v => v.Status == VoucherStatus.Supervisor_Approved).ToList();
            return PartialView("~/Views/Store/_AdjustmentVoucher.cshtml", vouchers);
        }

        public ActionResult Details(int voucherId)
        {
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVouchers.Find(voucherId);
            if (adjustmentVoucher == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Store/Manager/VoucherDetails.cshtml", adjustmentVoucher);
        }

=======
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.Controllers.Store
{
    [Authorize(Roles = "MANAGER")]
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Store/Manager/Index.cshtml");
        }
>>>>>>> 10578a09f5d6cdf27db9a235fc7f8c6c6b60fd2b
    }
}
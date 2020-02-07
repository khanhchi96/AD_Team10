using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AD_Team10.DAL;
using AD_Team10.Models;

namespace AD_Team10.Controllers
{
    public class AdjustmentVouchersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: AdjustmentVouchers
        public ActionResult Index()
        {
            var adjustmentVouchers = db.AdjustmentVouchers.Include(a => a.Clerk);
            return View(adjustmentVouchers.ToList());
        }

        // GET: AdjustmentVouchers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVouchers.Find(id);
            if (adjustmentVoucher == null)
            {
                return HttpNotFound();
            }
            return View(adjustmentVoucher);
        }

        // GET: AdjustmentVouchers/Create
        public ActionResult Create()
        {
            ViewBag.StoreEmployeeID = new SelectList(db.StoreEmployees, "StoreEmployeeID", "FirstName");
            return View();
        }

        // POST: AdjustmentVouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdjustmentVoucherID,AdjustmentDate,Status,StoreEmployeeID,Remark")] AdjustmentVoucher adjustmentVoucher)
        {
            if (ModelState.IsValid)
            {
                db.AdjustmentVouchers.Add(adjustmentVoucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoreEmployeeID = new SelectList(db.StoreEmployees, "StoreEmployeeID", "FirstName", adjustmentVoucher.StoreEmployeeID);
            return View(adjustmentVoucher);
        }

        // GET: AdjustmentVouchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVouchers.Find(id);
            if (adjustmentVoucher == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreEmployeeID = new SelectList(db.StoreEmployees, "StoreEmployeeID", "FirstName", adjustmentVoucher.StoreEmployeeID);
            return View(adjustmentVoucher);
        }

        // POST: AdjustmentVouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdjustmentVoucherID,AdjustmentDate,Status,StoreEmployeeID,Remark")] AdjustmentVoucher adjustmentVoucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adjustmentVoucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreEmployeeID = new SelectList(db.StoreEmployees, "StoreEmployeeID", "FirstName", adjustmentVoucher.StoreEmployeeID);
            return View(adjustmentVoucher);
        }

        // GET: AdjustmentVouchers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVouchers.Find(id);
            if (adjustmentVoucher == null)
            {
                return HttpNotFound();
            }
            return View(adjustmentVoucher);
        }

        // POST: AdjustmentVouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVouchers.Find(id);
            db.AdjustmentVouchers.Remove(adjustmentVoucher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

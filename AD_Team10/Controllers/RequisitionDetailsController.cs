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
    public class RequisitionDetailsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: RequisitionDetails
        public ActionResult Index()
        {
            var requisitionDetails = db.RequisitionDetails.Include(r => r.Item).Include(r => r.Requisition);
            return View(requisitionDetails.ToList());
        }

        // GET: RequisitionDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequisitionDetail requisitionDetail = db.RequisitionDetails.Find(id);
            if (requisitionDetail == null)
            {
                return HttpNotFound();
            }
            return View(requisitionDetail);
        }

        // GET: RequisitionDetails/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "Description");
            ViewBag.RequisitionID = new SelectList(db.Requisitions, "RequisitionID", "Remark");
            return View();
        }

        // POST: RequisitionDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequisitionID,ItemID,Quantity,QuantityReceived")] RequisitionDetail requisitionDetail)
        {
            if (ModelState.IsValid)
            {
                db.RequisitionDetails.Add(requisitionDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "Description", requisitionDetail.ItemID);
            ViewBag.RequisitionID = new SelectList(db.Requisitions, "RequisitionID", "Remark", requisitionDetail.RequisitionID);
            return View(requisitionDetail);
        }

        // GET: RequisitionDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequisitionDetail requisitionDetail = db.RequisitionDetails.Find(id);
            if (requisitionDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "Description", requisitionDetail.ItemID);
            ViewBag.RequisitionID = new SelectList(db.Requisitions, "RequisitionID", "Remark", requisitionDetail.RequisitionID);
            return View(requisitionDetail);
        }

        // POST: RequisitionDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequisitionID,ItemID,Quantity,QuantityReceived")] RequisitionDetail requisitionDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requisitionDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "Description", requisitionDetail.ItemID);
            ViewBag.RequisitionID = new SelectList(db.Requisitions, "RequisitionID", "Remark", requisitionDetail.RequisitionID);
            return View(requisitionDetail);
        }

        // GET: RequisitionDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequisitionDetail requisitionDetail = db.RequisitionDetails.Find(id);
            if (requisitionDetail == null)
            {
                return HttpNotFound();
            }
            return View(requisitionDetail);
        }

        // POST: RequisitionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequisitionDetail requisitionDetail = db.RequisitionDetails.Find(id);
            db.RequisitionDetails.Remove(requisitionDetail);
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

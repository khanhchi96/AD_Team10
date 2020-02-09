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
    [Authorize(Roles = "CLERK")]
    public class CategoriesController : Controller
    {
      
        private DBContext db = new DBContext();

        // GET: Categories
        public ActionResult Index()
        {
            var ItemCategories = db.Items.Include(d => d.Category);
            return View(ItemCategories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item ItemCategory = db.Items.Find(id);
            if (ItemCategory == null)
            {
                return HttpNotFound();
            }
            return View(ItemCategory);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,Description,UnitOfMeasure,UnitsInStock,ReorderLevel,ReorderQuantity,CategoryID")] Models.Item ItemCategory)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(ItemCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName", ItemCategory.CategoryID);
            return View(ItemCategory);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item ItemCategory = db.Items.Find(id);
            if (ItemCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName", ItemCategory.CategoryID);
            return View(ItemCategory);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,Description,UnitOfMeasure,UnitsInStock,ReorderLevel,ReorderQuantity,CategoryName")] Models.Item ItemCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ItemCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryId", "CategoryName", ItemCategory.CategoryID);
            return View(ItemCategory);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item ItemCategory = db.Items.Find(id);
            if (ItemCategory == null)
            {
                return HttpNotFound();
            }
            return View(ItemCategory);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item ItemCategory = db.Items.Find(id);
            db.Items.Remove(ItemCategory);
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

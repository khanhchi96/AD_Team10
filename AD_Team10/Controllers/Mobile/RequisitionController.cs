using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AD_Team10.DAL;
using AD_Team10.Models;

namespace AD_Team10.Controllers.Mobile
{
    public class RequisitionController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Requisition
        public IQueryable<Requisition> GetRequisitions()
        {
            return db.Requisitions;
        }

        // GET: api/Requisition/5
        [ResponseType(typeof(Requisition))]
        public IHttpActionResult GetRequisition(int id)
        {
            Requisition requisition = db.Requisitions.Find(id);
            if (requisition == null)
            {
                return NotFound();
            }

            return Ok(requisition);
        }

        // PUT: api/Requisition/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRequisition(int id, Requisition requisition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != requisition.RequisitionID)
            {
                return BadRequest();
            }

            db.Entry(requisition).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequisitionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Requisition
        [ResponseType(typeof(Requisition))]
        public IHttpActionResult PostRequisition(Requisition requisition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Requisitions.Add(requisition);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = requisition.RequisitionID }, requisition);
        }

        // DELETE: api/Requisition/5
        [ResponseType(typeof(Requisition))]
        public IHttpActionResult DeleteRequisition(int id)
        {
            Requisition requisition = db.Requisitions.Find(id);
            if (requisition == null)
            {
                return NotFound();
            }

            db.Requisitions.Remove(requisition);
            db.SaveChanges();

            return Ok(requisition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RequisitionExists(int id)
        {
            return db.Requisitions.Count(e => e.RequisitionID == id) > 0;
        }
    }
}
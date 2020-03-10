using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AD_Team10.Authentication;
using AD_Team10.DAL;
using AD_Team10.Models;

//Author: Feng Li Ying
namespace AD_Team10.Service
{
    public class EmployeeService
    {
        DBContext db = new DBContext();
        
        public void UpdateAndSave(int requisitionId, FormCollection form)
        {
            var rds = (from rqDetail in db.RequisitionDetails
                       where rqDetail.RequisitionID == requisitionId
                       select rqDetail).ToList();

            foreach (var rd in rds)
            {
                db.RequisitionDetails.Remove(rd);
                db.SaveChanges();
            }

            var Items = form.GetValues("Item");
            var Quantities = form.GetValues("Quantity");

            for (int i = 0; i < Items.Count(); i++)
            {
                RequisitionDetail newReqDetail = new RequisitionDetail
                {
                    RequisitionID = requisitionId,
                    ItemID = Int32.Parse(Items[i]),
                    Quantity = Int32.Parse(Quantities[i]),
                    QuantityReceived = 0
                };


                db.RequisitionDetails.Add(newReqDetail);
                db.SaveChanges();

            }
        }

        public void CreateRequisition(Requisition requisition, FormCollection form)
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int departmentId = db.DeptUsers.SingleOrDefault(d => d.DeptUserID == user.UserID).DeptEmployee.DepartmentID;
            Requisition newReq = new Requisition
            {
                RequisitionDate = DateTime.Today,
                Status = Status.Pending,
                EmployeeID = user.UserID,
            };
            //newReq.RetrievalListID = retrievalListId;

            db.Requisitions.Add(newReq);

            var ItemDescriptions = form.GetValues("ItemDescription");
            var ItemQuantitys = form.GetValues("ItemQuantity");

            for (int i = 0; i < ItemDescriptions.Count(); i++)
            {
                var temp = ItemDescriptions[i];

                int itemID = (from item in db.Items
                              where item.Description == temp
                              select item.ItemID).FirstOrDefault();


                RequisitionDetail newReqDetail = new RequisitionDetail
                {
                    RequisitionID = newReq.RequisitionID,
                    ItemID = itemID,
                    Quantity = Int32.Parse(ItemQuantitys[i]),
                    QuantityReceived = 0
                };

                db.RequisitionDetails.Add(newReqDetail);
                
            }
            db.SaveChanges();

        }

        public void DeleteRequisition(int requisitionId)
        {
            var Requisition = db.Requisitions.Find(requisitionId);
            db.Requisitions.Remove(Requisition);
            db.SaveChanges();
        }

        public List<Requisition> GetRequisitions()
        {
            int employeeId = GetEmployeeID();
            var requisitions = db.Requisitions.Where(r => r.EmployeeID == employeeId).OrderByDescending(r => r.RequisitionDate).ToList();
            return requisitions;
        }

        public int GetEmployeeID()
        {
            CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int employeeId = user.UserID;
            return employeeId;
        }
    }
}
using AD_Team10.Authentication;
using AD_Team10.CustomMobileModels;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AD_Team10.Controllers.Mobile
{
    public class MobileController : ApiController
    {
        private DBContext db = new DBContext();

        [HttpPost]
        public IHttpActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                CustomMembership customMembership = new CustomMembership
                {
                    UserType = loginView.UserType
                };
                CustomRole customRole = new CustomRole
                {
                    UserType = loginView.UserType
                };

                if (customMembership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)customMembership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        if ((loginView.UserType.Equals("Store") && user.Role != "CLERK") ||
                            (loginView.UserType.Equals("Department") && user.Role == "EMPLOYEE"))
                            return Unauthorized();
                        return Ok(user);
                    }
                }
            }
            return NotFound();
        }

        public IQueryable<CustomPurchaseOrder> GetPurchaseOrders()
        {
            var orders = db.PurchaseOrders.OrderBy(p => p.OrderDate)
                        .Select(p => new CustomPurchaseOrder
                        {
                            OrderID = p.PurchaseOrderID,
                            OrderDate = p.OrderDate.ToString(),
                            SupplierName = p.Supplier.SupplierName,
                            Status = p.OrderStatus.ToString()
                        });
            if (orders.Count() <= 20) return orders;
            else return orders.Take(20);
        }


        public IHttpActionResult GetPurchaseOrderDetail(int id)
        {
            List<CustomItem> orderDetails = db.PurchaseOrderDetails
                                                     .Where(p => p.OrderID == id)
                                                     .Select(p => new CustomItem
                                                     {
                                                         ItemID = p.ItemID,
                                                         Description = p.Item.Description,
                                                         Quantity = p.Quantity,
                                                         QuantityReceived = p.QuantityReceived
                                                     }).ToList();
            if (orderDetails == null)
            {
                return NotFound();
            }

            return Ok(orderDetails);
        }

        public IHttpActionResult GetRetrievalList()
        {
           int id = Convert.ToInt32(db.RetrievalLists.OrderByDescending(r => r.StartDate).Take(1).Select(r => r.RetrievalListID));
            List<CustomItem> retrieval = db.RetrievalListDetails.Where(r => r.RetrievalListID == id)
                         .Select(r => new CustomItem
                         {
                             ItemID = r.ItemID,
                             Description = r.Item.Description,
                             Quantity = r.Quantity,
                             QuantityReceived = r.QuantityReceived
                         }).ToList();
            if (retrieval == null) return NotFound();
            else return Ok(retrieval);
        }

        [HttpPut]        
        public IHttpActionResult UpdatePurchaseOrder(CustomPurchaseOrder purchaseOrder)
        {
            CustomItem[] customItems = purchaseOrder.CustomItems;
            int orderId = purchaseOrder.OrderID;
            for (int i = 0; i < customItems.Count(); i++)
            {
                CustomItem customItem = customItems[i];
                PurchaseOrderDetail purchaseOrderDetail = db.PurchaseOrderDetails.SingleOrDefault(
                    r => r.ItemID == customItem.ItemID && r.OrderID == orderId);
                if (purchaseOrderDetail == null) return NotFound();
                else
                {
                    purchaseOrderDetail.QuantityReceived = customItem.QuantityReceived;
                    db.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult UpdateDisbursementList(CustomDisbursementList disbursementList)
        {
            int retrievalListId = disbursementList.RetrievalListID;
            int departmentId = disbursementList.DepartmentID;
            CustomItem[] customItems = disbursementList.CustomItems;
            for(int i=0; i< customItems.Count(); i++)
            {
                CustomItem customItem = customItems[i];
                RetrievalListDetail retrievalListDetail = db.RetrievalListDetails.SingleOrDefault(
                    r => r.DepartmentID == departmentId && r.ItemID == customItem.ItemID && r.RetrievalListID == retrievalListId);
                if (retrievalListDetail == null) return NotFound();
                else
                {
                    retrievalListDetail.QuantityReceived = customItem.QuantityReceived;
                    db.SaveChanges();
                }
            }
            return Ok();
        }

        public IHttpActionResult GetDisbursementList(int departmentId)
        {
            int id = Convert.ToInt32(db.RetrievalLists.OrderByDescending(r => r.StartDate).Take(1).Select(r => r.RetrievalListID));
            List<CustomItem> disbursement = db.RetrievalListDetails.Where(r => r.RetrievalListID == id && r.DepartmentID == departmentId)
                          .Select(r => new CustomItem
                          {
                              ItemID = r.ItemID,
                              Description = r.Item.Description,
                              Quantity = r.Quantity,
                              QuantityReceived = r.QuantityReceived
                          }).ToList();
            if (disbursement == null) return NotFound();
            else return Ok(disbursement);
        }

        public IHttpActionResult GetDepartmentList()
        {
            int id = Convert.ToInt32(db.RetrievalLists.OrderByDescending(r => r.StartDate).Take(1).Select(r => r.RetrievalListID));
            List<CustomDepartment> departments = db.RetrievalListDetails.Where(r => r.RetrievalListID == id)
                          .Select(r => new CustomDepartment
                          {
                              DepartmentID = r.DepartmentID,
                              DepartmentName = r.Department.DepartmentName,
                              CollectionPointName = r.Department.CollectionPoint.CollectionPointName
                          }).ToList();
            for(int i=0; i<departments.Count(); i++)
            {
                int depID = departments[i].DepartmentID;
                DepUser rep = db.DepUsers.Where(d => d.Role == DepartmentRole.REPRESENTATIVE
                          && d.DepEmployee.DepartmentID == depID).First();
                departments[i].RepName = rep.DepEmployee.ToString();
                departments[i].RepEmail = rep.DepEmployee.Email;
                departments[i].RepPhone = rep.DepEmployee.Phone;
            }

            return Ok(departments);
        }
        

    }
}

using AD_Team10.Authentication;
using AD_Team10.CustomMobileModels;
using AD_Team10.DAL;
using AD_Team10.Models;
using AD_Team10.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//Author: Phung Khanh Chi
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

        public IHttpActionResult GetPurchaseOrders()
        {
            List<CustomPurchaseOrder> orders = db.PurchaseOrders.OrderByDescending(p => p.OrderDate)
                        .Select(p => new CustomPurchaseOrder
                        {
                            OrderID = p.PurchaseOrderID,
                            OrderDate = p.OrderDate.ToString(),
                            SupplierName = p.Supplier.SupplierName,
                            Status = p.OrderStatus.ToString()
                        }).ToList();


            if (orders.Count() <= 20) return Ok(orders);
            else return Ok(orders.Take(20));
        }


        [HttpPut]   
        public IHttpActionResult UpdatePurchaseOrder([FromBody] CustomPurchaseOrder purchaseOrder)
        {
            CustomItem[] customItems = purchaseOrder.CustomItems;
            Debug.WriteLine(purchaseOrder.OrderID);
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

            PurchaseOrder order = db.PurchaseOrders.SingleOrDefault(p => p.PurchaseOrderID == purchaseOrder.OrderID);
            List<PurchaseOrderDetail> orderDetails = order.PurchaseOrderDetails.ToList();
            bool isCompleted = true;
            bool isIncomplete = false;
            for (int i = 0; i < orderDetails.Count(); i++)
            {
                if (orderDetails[i].QuantityReceived > 0) isIncomplete = true;
                if (orderDetails[i].QuantityReceived < orderDetails[i].Quantity) isCompleted = false;
            }

            if (isCompleted) order.OrderStatus = OrderStatus.Completed;
            else if (isIncomplete) order.OrderStatus = OrderStatus.Incompleted;
            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult GetPurchaseOrderDetail(int id)
        {
            PurchaseOrder order = db.PurchaseOrders.SingleOrDefault(p => p.PurchaseOrderID == id);
            CustomPurchaseOrder customPurchaseOrder = new CustomPurchaseOrder
            {
                OrderID = order.PurchaseOrderID,
                OrderDate = order.OrderDate.ToString("dd/MM/yyyy"),
                SupplierName = order.Supplier.SupplierName,
                Status = order.OrderStatus.ToString(),
                CustomItems = GetItemsForOrder(id)
            };
            return Ok(customPurchaseOrder);
        }


        public CustomItem[] GetItemsForOrder(int id)
        {
            CustomItem[] items = db.PurchaseOrderDetails
                                                     .Where(p => p.OrderID == id)
                                                     .Select(p => new CustomItem
                                                     {
                                                         ItemID = p.ItemID,
                                                         Description = p.Item.Description,
                                                         Quantity = p.Quantity,
                                                         QuantityReceived = p.QuantityReceived
                                                     }).ToArray();
            return items;
        }
        public IHttpActionResult GetRetrievalList()
        {
            RetrievalList retrievalList = FindCurrentRetrievalList();
            if(retrievalList == null)
            {
                return NotFound();
            }
            else
            {
                int id = retrievalList.RetrievalListID;
                List<CustomItem> retrieval = db.RetrievalListDetails.Where(r => r.RetrievalListID == id).GroupBy(r => new { r.ItemID, r.Item.Description })
                         .Select(r => new CustomItem
                         {
                             ItemID = r.Key.ItemID,
                             Description = r.Key.Description,
                             Quantity = r.Sum(i => i.Quantity),
                             QuantityReceived = r.Sum(i => i.QuantityReceived),
                             QuantityOffered = r.Sum(i => i.QuantityOffered)
                         }).ToList();

                if (retrieval == null) return NotFound();
                else return Ok(retrieval);
            } 
        }

        public RetrievalList FindCurrentRetrievalList(){
            DateTime today = DateTime.Today;
            RetrievalList retrievalList = db.RetrievalLists.SingleOrDefault(r => r.StartDate <= today && r.EndDate >= today);
            return retrievalList;
        }
        public IHttpActionResult GetRetrievalListByItem(int id)
        {
            RetrievalList retrievalList = FindCurrentRetrievalList();
            if (retrievalList == null) return NotFound();
            int retrievalListId = retrievalList.RetrievalListID;
            List<CustomRetrievalListDetail> details = db.RetrievalListDetails.Where(r => r.RetrievalListID == retrievalListId
                                                && r.ItemID == id)
                                                .Select(r => new CustomRetrievalListDetail {
                                                    DepartmentID = r.DepartmentID,
                                                    DepartmentName = r.Department.DepartmentName,
                                                    Quantity = r.Quantity,
                                                    QuantityOffered = r.QuantityOffered
                                                }).ToList();
            if (details == null) return NotFound();
            else
            {
                return Ok(details);
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateRetrievalListByItem(CustomRetrievalList customRetrievalList)
        {
            RetrievalList retrievalList = FindCurrentRetrievalList();
            if (retrievalList == null) return NotFound();
            int retrievalListId = retrievalList.RetrievalListID;
            CustomRetrievalListDetail[] retrievalListDetails = customRetrievalList.RetrievalListDetails;
            int itemId = customRetrievalList.ItemID;
            for(int i=0; i<retrievalListDetails.Length; i++)
            {
                CustomRetrievalListDetail customDetail = retrievalListDetails[i];
                RetrievalListDetail detail = db.RetrievalListDetails.SingleOrDefault(
                                             r => r.RetrievalListID == retrievalListId &&
                                             r.ItemID == itemId &&
                                             r.DepartmentID == customDetail.DepartmentID);
                detail.QuantityOffered = customDetail.QuantityOffered ;
              
            }
            db.SaveChanges();
            return Ok();
        }

             
        

        [HttpPut]
        public IHttpActionResult UpdateDisbursementList(CustomDisbursementList disbursementList)
        {
            int retrievalListId = FindCurrentRetrievalList().RetrievalListID - 1;
            int departmentId = disbursementList.DepartmentID;
            CustomItem[] customItems = disbursementList.CustomItems;
            for(int i=0; i< customItems.Count(); i++)
            {
                CustomItem customItem = customItems[i];
                RetrievalListDetail retrievalListDetail = db.RetrievalListDetails.SingleOrDefault(
                    r => r.DepartmentID == departmentId && r.ItemID == customItem.ItemID && r.RetrievalListID == retrievalListId);
                    retrievalListDetail.QuantityReceived = customItem.QuantityReceived;
            }
            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult GetDisbursementList(int id)
        {
            int retrievalListId = FindCurrentRetrievalList().RetrievalListID - 1;
            List<CustomItem> disbursement = db.RetrievalListDetails.Where(r => r.RetrievalListID == retrievalListId &&
                                        r.DepartmentID == id)
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
            int id = FindCurrentRetrievalList().RetrievalListID - 1;
            List<CustomDepartment> departments = db.RetrievalListDetails.Where(r => r.RetrievalListID == id)
                          .Select(r => new CustomDepartment {
                              DepartmentID = r.DepartmentID,
                              DepartmentName = r.Department.DepartmentName,
                              CollectionPoint = r.Department.CollectionPoint
                          }).Distinct().ToList();
            for(int i=0; i<departments.Count(); i++)
            {
                int depID = departments[i].DepartmentID;
                DeptUser rep = db.DeptUsers.SingleOrDefault(d => d.Role == DepartmentRole.REPRESENTATIVE
                          && d.DeptEmployee.DepartmentID == depID);
                departments[i].Representative = new CustomDeptEmployee
                {
                    DeptEmployeeName = rep.DeptEmployee.ToString(),
                    Email = rep.DeptEmployee.Email,
                    Phone = rep.DeptEmployee.Phone
                };
            }

            return Ok(departments);
        }

        public IHttpActionResult GetDepartmentIdFromUserId(int id)
        {
            int departmentId = db.DeptEmployees.SingleOrDefault(d => d.DeptEmployeeID == id).DepartmentID;
            if (departmentId == 0) return NotFound();
            else return Ok(departmentId);
        }

        public IHttpActionResult GetPendingRequisitions(int id)
        {
            List<CustomRequisition> requisitions = db.Requisitions.Where(r => r.Employee.DepartmentID == id && r.Status == Status.Pending).AsEnumerable()
                                            .Select(r => new CustomRequisition
                                            {
                                                RequisitionID = r.RequisitionID,
                                                RequisitionDate = r.RequisitionDate.ToString("dd/MM/yyyy"),
                                                EmployeeName = r.Employee.ToString()
                                            }).ToList();
            if (requisitions == null) return NotFound();
            else return Ok(requisitions);
        }


        public IHttpActionResult GetRequisitionDetail(int id)
        {
            string employeeName = db.Requisitions.SingleOrDefault(r => r.RequisitionID == id).Employee.ToString();
            CustomItem[] items = db.RequisitionDetails.Where(r => r.RequisitionID == id)
                                        .Select(r => new CustomItem
                                        {
                                            ItemID = r.ItemID,
                                            Description = r.Item.Description,
                                            Quantity = r.Quantity
                                        }).ToArray();
            CustomRequisition customRequisition = new CustomRequisition
            {
                RequisitionID = id,
                EmployeeName = employeeName,
                CustomItems = items
            };
            return Ok(customRequisition);
        }
        
        [HttpPut]
        public IHttpActionResult ApproveRequisition(int id)
        {
            Requisition requisition = db.Requisitions.SingleOrDefault(r => r.RequisitionID == id);
            requisition.Status = Status.Approved;
            int deptId = requisition.Employee.DepartmentID;
            List<RequisitionDetail> details = requisition.RequisitionDetails.ToList();
            int retrievalListId = FindCurrentRetrievalList().RetrievalListID;
            db.RequisitionRetrievals.Add(new RequisitionRetrieval { RequisitionID = requisition.RequisitionID, RetrievalListID = retrievalListId });
            db.SaveChanges();
            for(int i=0; i<details.Count(); i++)
            {
                RequisitionDetail detail = details[i];
                RetrievalListDetail retrievalListDetail = db.RetrievalListDetails.SingleOrDefault(
                                                r => r.RetrievalListID == retrievalListId &&
                                                r.DepartmentID == deptId && r.ItemID == detail.ItemID
                                                );
                if (retrievalListDetail == null)
                {
                    retrievalListDetail = new RetrievalListDetail
                    {
                        RetrievalListID = retrievalListId,
                        DepartmentID = deptId,
                        ItemID = detail.ItemID,
                        Quantity = detail.Quantity
                    };
                    db.RetrievalListDetails.Add(retrievalListDetail);
                }
                else retrievalListDetail.Quantity += detail.Quantity;
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult RejectRequisition(int id)
        {
            Requisition requisition = db.Requisitions.SingleOrDefault(r => r.RequisitionID == id);
            if (requisition == null) return NotFound();
            requisition.Status = Status.Rejected;
            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult UpdateRequisition(CustomRequisition customRequisition)
        {
            int requisitionId = customRequisition.RequisitionID;
            int retrievalListId = FindCurrentRetrievalList().RetrievalListID;
            CustomItem[] items = customRequisition.CustomItems;
            for(int i=0; i<items.Length; i++)
            {
                CustomItem item = items[i];
                RequisitionDetail detail = db.RequisitionDetails.SingleOrDefault(
                    r => r.RequisitionID == requisitionId && r.ItemID == item.ItemID);
                detail.QuantityReceived = item.QuantityReceived;
                if(item.QuantityReceived < detail.Quantity)
                {
                    int departmentId = detail.Requisition.Employee.DepartmentID;
                    RetrievalListDetail retrievalListDetail = db.RetrievalListDetails.SingleOrDefault(
                        r => r.RetrievalListID == retrievalListId &&
                        r.DepartmentID == departmentId && r.ItemID == item.ItemID);
                    if(retrievalListDetail == null)
                    {
                        retrievalListDetail = new RetrievalListDetail
                        {
                            RetrievalListID = retrievalListId,
                            DepartmentID = departmentId,
                            ItemID = item.ItemID,
                            Quantity = detail.Quantity - item.QuantityReceived
                        };
                        db.RetrievalListDetails.Add(retrievalListDetail);
                    }
                    else retrievalListDetail.Quantity += (detail.Quantity - item.QuantityReceived);
                    RequisitionRetrieval requisitionRetrieval = db.RequisitionRetrievals.SingleOrDefault(r => r.RequisitionID == requisitionId);
                    requisitionRetrieval.RetrievalListID = retrievalListId;
                }
            }
            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult GetCurrentRep(int id)
        { 
            DeptUser rep = db.DeptUsers.SingleOrDefault(d => d.DeptEmployee.DepartmentID == id && d.Role == DepartmentRole.REPRESENTATIVE);
            CustomDeptEmployee employee = new CustomDeptEmployee
            {
                DepartmentID = id,
                DeptEmployeeName = rep.DeptEmployee.ToString(),
                Email = rep.DeptEmployee.Email,
                Phone = rep.DeptEmployee.Phone          
            };
            return Ok(employee);
        }

        public IHttpActionResult GetEmployeeList(int id)
        { 
            List<CustomDeptEmployee> employees = db.DeptUsers.Where(d => d.DeptEmployee.DepartmentID == id
                                                    && d.Role == DepartmentRole.EMPLOYEE).AsEnumerable()
                                                    .Select(d => new CustomDeptEmployee
                                                    {
                                                        DepartmentID = id,
                                                        DeptEmployeeID = d.DeptEmployeeID,
                                                        DeptEmployeeName = d.DeptEmployee.ToString(),
                                                        Designation = d.DeptEmployee.Designation,
                                                        Email = d.DeptEmployee.Email,
                                                        Phone = d.DeptEmployee.Phone
                                                    }).ToList();
            if (employees == null) return NotFound();
            else return Ok(employees);
        }

        public IHttpActionResult GetCollectionPoint(int id)
        {
            Models.Department department = db.Departments.SingleOrDefault(d => d.DepartmentID == id);
            if (department == null || department.CollectionPoint == null) return NotFound();
            else return Ok(department.CollectionPoint);
        }

        public IHttpActionResult GetCollectionPointList(int id)
        {
            Models.Department department = db.Departments.SingleOrDefault(d => d.DepartmentID == id);
            List<CollectionPoint> collectionPoints = db.CollectionPoints.Where(c => c.CollectionPointID != department.CollectionPointID)
                                .ToList();
            return Ok(collectionPoints);
        }

        [HttpPut]
        public IHttpActionResult ChangeCollectionPoint(CustomDepartment customDepartment)
        {
            Models.Department dept = db.Departments.SingleOrDefault(d => d.DepartmentID == customDepartment.DepartmentID);
            if (dept == null) return NotFound();
            dept.CollectionPointID = customDepartment.CollectionPoint.CollectionPointID;
            db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult AssignRepresentative(CustomDeptEmployee customDeptEmployee)
        {
            DeptUser newRep = db.DeptUsers.SingleOrDefault(d => d.DeptEmployeeID == customDeptEmployee.DeptEmployeeID);
            DeptUser currentRep = db.DeptUsers.SingleOrDefault(d => d.Role == DepartmentRole.REPRESENTATIVE && 
                            d.DeptEmployee.DepartmentID == customDeptEmployee.DepartmentID);
            currentRep.Role = DepartmentRole.EMPLOYEE;
            newRep.Role = DepartmentRole.REPRESENTATIVE;
            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult GetDisbursementListForDept(int id)
        {
            int retrievalListId = FindCurrentRetrievalList().RetrievalListID - 1;
            RetrievalList retrievalList = db.RetrievalLists.SingleOrDefault(r => r.RetrievalListID == retrievalListId);
            List<CustomItem> disbursement = db.RetrievalListDetails.Where(r => r.RetrievalListID == retrievalListId &&
                                        r.DepartmentID == id)
                          .Select(r => new CustomItem
                          {
                              ItemID = r.ItemID,
                              Description = r.Item.Description,
                              Quantity = r.Quantity,
                              QuantityReceived = r.QuantityReceived
                          }).ToList();

            return Ok(disbursement);
        }

        public IHttpActionResult GetDisbursementDetailByItem(int departmentId, int itemId)
        {
            int retrievalListId = FindCurrentRetrievalList().RetrievalListID - 1;
            List<int> requisitionIDs = db.RequisitionRetrievals.Where(r => r.RetrievalListID == retrievalListId)
                                                .Select(r => r.Requisition.RequisitionID).ToList();
            List<CustomRequisition> details = db.RequisitionDetails.Where(r => r.ItemID == itemId &&
                                        requisitionIDs.Contains(r.RequisitionID) &&
                                        r.Requisition.Employee.DepartmentID == departmentId &&
                                        (r.Requisition.Status == Status.Incomplete || r.Requisition.Status == Status.Approved)).ToList()
                                        .Select(r => new CustomRequisition
                                        {
                                            RequisitionID = r.RequisitionID,
                                            RequisitionDate = r.Requisition.RequisitionDate.ToString("dd/MM/yyyy"),
                                            EmployeeName = r.Requisition.Employee.ToString(),
                                            CustomItems = new CustomItem[]
                                            {
                                                new CustomItem
                                                {
                                                    ItemID = r.ItemID,
                                                    Description = r.Item.Description,
                                                    Quantity = r.Quantity,
                                                    QuantityReceived = r.QuantityReceived
                                                }
                                            }
                                        }).ToList();

            if (details == null) return NotFound();
            else return Ok(details); 
        }

        [HttpPut]
        public IHttpActionResult UpdateDisbursementDetailByItem(CustomRequisition[] requisitions)
        {
           for(int i=0; i<requisitions.Length; i++)
            {
                CustomRequisition requisition = requisitions[i];
                CustomItem item = requisition.CustomItems[0];
                RequisitionDetail detail = db.RequisitionDetails.SingleOrDefault(r => r.RequisitionID == requisition.RequisitionID &&
                                                    r.ItemID == item.ItemID);
                detail.QuantityReceived = item.QuantityReceived;
                db.SaveChanges();
            }
            return Ok();
        }
    }
}

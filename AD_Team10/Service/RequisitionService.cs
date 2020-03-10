using AD_Team10.DAL;
using AD_Team10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


//Author: Wang Wang Wang, Phung Khanh Chi
namespace AD_Team10.Service
{
    public class RequisitionService
    {
        public static string employeeName = "EmployeeName";
        public static string requisitionNo = "REQUISITION NO.";
        public static string itemNo = "Item No.";
        public static string quantityRequsted = "QuantityRequested";
        public static string quantityRemaining = "QuantityRemaining";
        public static string update = "Update";
        public static string save = "Save";
        public static string retrievalList = "RETRIEVAL LIST";
        public static string generateDisbursement = "Generate Disbursement List";
        public static string totalQuantity = "Total Quantity";
        public static string breakDownByDep = "Breakdown by Department";
        public static string needed = "Needed";
        public static string retrieved = "Retrieved";
        DBContext db = new DBContext();

        public List<Requisition> GetRequisitionByStatus(Status status)
        {
            List<Requisition> requisitions = db.Requisitions.Where(x => x.Status == status).ToList();
            return requisitions;
        }

        public Requisition Find(int? id)
        {
            Requisition requisition = db.Requisitions.Find(id);
            return requisition;
        }

        public List<RequisitionDetail> GetPendingRequisitionDetails()
        {
            return db.RequisitionDetails.Include(i => i.Requisition).Include(x => x.Item).Where(x => x.Requisition.Status == Status.Approved).ToList();
        }

        public List<RequisitionDetail> GetApprovedRequisitionDetail()
        {
            return db.RequisitionDetails.Include(i => i.Requisition).Include(x => x.Item).Where(x => x.Requisition.Status == Status.Approved).ToList();
        }

        public List<Requisition> GetPendingRequisitions()
        {
            return db.Requisitions.Include(x => x.Employee).Where(x => x.Status == Status.Approved || x.Status == Status.Incomplete).ToList();
        }

        public void SequencedRetrievalList(List<Item> items, RetrievalList retrievalList)
        {
            List<RetrievalListDetail> newRetrievalListDetails = new List<RetrievalListDetail>();
            foreach (var item in items)
            {
                List<RetrievalListDetail> retrievalListDetails = retrievalList.RetrievalListDetails.Where(x => x.ItemID == item.ItemID).ToList();
                newRetrievalListDetails.AddRange(retrievalListDetails);
            }
            for (int i = 0; i < retrievalList.RetrievalListDetails.Count; i++)
            {
                retrievalList.RetrievalListDetails[i] = newRetrievalListDetails[i];
            }
        }

        public Dictionary<Requisition, int> GetRequisitionByItem(Item item, List<RequisitionDetail> pendingRequisitionDetails)
        {
            List<Requisition> requisitions = new List<Requisition>();
            Dictionary<Requisition, int> requisitionQuantity = new Dictionary<Requisition, int>();
            List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Where(x => x.ItemID == item.ItemID).ToList();
            foreach(var reqDetail in requisitionDetails)
            {
                int quantity = reqDetail.Quantity;
                Requisition requisition = db.Requisitions.Include(x => x.Employee).Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                requisitionQuantity.Add(requisition, quantity);
            }
            return requisitionQuantity;
        }

        public List<Item> GetDistinctItem(List<RequisitionDetail> requisitionDetails)
        {
            List<Item> items = new List<Item>();
            foreach(var reqDetail in requisitionDetails)
            {
                Item item = db.Items.Where(x => x.ItemID == reqDetail.ItemID).SingleOrDefault();
                if (!items.Contains(item))
                    items.Add(item);
            }
            return items;
        }

        public List<Department> DepartmentsForItem(Item item, List<RequisitionDetail> pendingRequisitionDetails)
        {
            List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Where(x => x.ItemID == item.ItemID).ToList();
            List<Requisition> requisitions = new List<Requisition>();
            List<DeptEmployee> employees = new List<DeptEmployee>();
            foreach (var reqDetail in requisitionDetails)
            {
                Requisition requisition = db.Requisitions.Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                DeptEmployee depEmployee = db.DeptEmployees.Where(x => x.DeptEmployeeID == requisition.EmployeeID).SingleOrDefault();
                employees.Add(depEmployee);
            }
            List<Department> departments = new List<Department>();
            foreach (var employee in employees)
            {
                Department department = db.Departments.Where(x => x.DepartmentID == employee.DepartmentID).SingleOrDefault();
                if (!departments.Contains(department))
                    departments.Add(department);
            }
            return departments;
        }

        public int DepartmentTotalItemQuantity(Item item, Department department)
        {
            List<DeptEmployee> depEmployees = db.DeptEmployees.Where(x => x.DepartmentID == department.DepartmentID).ToList();
            int quantity = 0;
            foreach (var depEmp in depEmployees)
            {
                List<Requisition> requisitions = db.Requisitions.Where(x => x.EmployeeID == depEmp.DeptEmployeeID && x.Status != Status.Completed).ToList();
                foreach (var requisition in requisitions)
                {
                    List<RequisitionDetail> requisitionDetails = db.RequisitionDetails.Where(x => x.RequisitionID == requisition.RequisitionID && x.ItemID == item.ItemID).ToList();
                    foreach (var reqDetail in requisitionDetails)
                    {
                        quantity += (reqDetail.Quantity - reqDetail.QuantityReceived);
                    }
                }
            }
            return quantity;
        }

        public Dictionary<Item,int> GetItemAndQuantity(List<RequisitionDetail> requisitionDetails)
        {
            Dictionary<Item, int> itemQuantity = new Dictionary<Item, int>();
            int quantity;
            foreach(var reqDetail in requisitionDetails)
            {
                if (!itemQuantity.ContainsKey(reqDetail.Item))
                {
                    itemQuantity.Add(reqDetail.Item, reqDetail.Quantity-reqDetail.QuantityReceived);
                }
                else
                {
                    quantity = reqDetail.Quantity-reqDetail.QuantityReceived;
                    itemQuantity[reqDetail.Item] += quantity;
                }
            }
            return itemQuantity;
        }


        //Disbursement List
        public Dictionary<Item, int> GetDepartmentItemAndQuantity(Department department)
        {
            Dictionary<Item, int> departmentItemQuantity = new Dictionary<Item, int>();
            RetrievalList retrievalList = FindCurrentRetrievalList();
            List<RetrievalListDetail> departmentRetrievalDetails = retrievalList.RetrievalListDetails.Where(x => x.DepartmentID == department.DepartmentID).ToList();
            foreach (var detail in departmentRetrievalDetails)
            {
                if (!departmentItemQuantity.ContainsKey(detail.Item))
                {
                    departmentItemQuantity.Add(detail.Item, detail.QuantityOffered);
                }
                else
                {
                    departmentItemQuantity[detail.Item] += detail.QuantityOffered;
                }
            }
            return departmentItemQuantity;
        }

        public List<Department> GetDepartments()
        {
            return db.Departments.ToList();
        }

        public List<CollectionPoint> GetCollectionPoints()
        {
            return db.CollectionPoints.ToList();
        }

        public List<PurchaseOrder> GetPurchaseOrder()
        {
            return db.PurchaseOrders.Include(x => x.Supplier).Include(x => x.PurchaseOrderDetails).ToList();
        }

        public Requisition GetRequisitionById(int id)
        {
            return db.Requisitions.Include(x => x.Employee).Include(x => x.RequisitionDetails).Where(x => x.RequisitionID == id).SingleOrDefault();
        }

        //check if the requisition is completed
        public bool IsCompleted(Requisition requisition)
        {
            bool isCompleted = true;
            for(int i = 0; i < requisition.RequisitionDetails.Count; i++)
            {
                if(requisition.RequisitionDetails.ToList()[i].Quantity != requisition.RequisitionDetails.ToList()[i].QuantityReceived)
                {
                    isCompleted = false;
                    break;
                }
            }
            return isCompleted;
        }

        public void UpdateRequisition(Requisition requisition)
        {
            db.Entry(requisition).State = EntityState.Modified;
            db.SaveChanges();
        }

        ////After update the requisitiondetail, the incompleted requisitions need to transfer to next retrieval list
        public RetrievalList GetRetrievalListForNow()
        {
            DateTime today = DateTime.Today;
            RetrievalList retrievalList = db.RetrievalLists.Include(x => x.RequisitionRetrievals)
                                                                  .Include(x => x.RetrievalListDetails)
                                                                  .Where(x => x.StartDate <= today && x.EndDate >= today)
                                                                  .SingleOrDefault();
            return retrievalList;
        }

        //Get the distinct items for retireval list
        public List<Item> GetDistictItemForRetrievalList(RetrievalList retrievalList)
        {
            List<Item> items = retrievalList.RetrievalListDetails.GroupBy(x => x.Item)
                                                                 .Select(x => x.Key)
                                                                 .ToList();
            return items;
        }


        //Get the incpmpleted requisitionDetails
        public List<RequisitionDetail> GetIncompletedDetails(Requisition requisition)
        {
            List<RequisitionDetail> requisitionDetails = requisition.RequisitionDetails.Where(x => x.Quantity != x.QuantityReceived).ToList();
            return requisitionDetails;
        }

        //Check if the retrievallist detail contains a department
        public bool ContainsDepartment(int id, RetrievalList retrievalList)
        {
            bool contains = false;
            
            if (retrievalList == null) return contains;
            if (retrievalList.RetrievalListDetails == null) return contains;
            foreach(var item in retrievalList.RetrievalListDetails)
            {
                if(item.DepartmentID == id)
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }


        //Incompleted requisitions transfer to next Retrieval list
        public void IncompletedRequisitionTransferToRetrieval(Requisition requisition, RetrievalList retrievalList)
        {
            int departmentId = requisition.Employee.DepartmentID;
            List<RequisitionDetail> requisitionDetails = this.GetIncompletedDetails(requisition);
            if(this.ContainsDepartment(departmentId, retrievalList))
            {
                foreach(var item in requisitionDetails)
                {
                    //if(this.RetrievalListDetailContainItem(item.ItemID, retrievalList.RetrievalListDetails.Where(x => x.DepartmentID == departmentId).ToList()))
                    //{
                    RetrievalListDetail retrievalListDetail = retrievalList.RetrievalListDetails.Where(x => x.DepartmentID == departmentId).Where(x => x.ItemID == item.ItemID).SingleOrDefault();
                    if(retrievalListDetail != null)
                    {
                        retrievalListDetail.Quantity += (item.Quantity - item.QuantityReceived);
                    }
                    //}
                    else
                    {
                        RetrievalListDetail newRetrieval = new RetrievalListDetail();
                        newRetrieval.DepartmentID = departmentId;
                        newRetrieval.ItemID = item.ItemID;
                        newRetrieval.Quantity = item.Quantity-item.QuantityReceived;
                        newRetrieval.RetrievalListID = retrievalList.RetrievalListID;
                        retrievalList.RetrievalListDetails.Add(newRetrieval);
                    }
                }
            }
            else
            {
                foreach(var item in requisitionDetails)
                {
                    RetrievalListDetail newRetrievalListDetail = new RetrievalListDetail();
                    newRetrievalListDetail.DepartmentID = departmentId;
                    newRetrievalListDetail.ItemID = item.ItemID;
                    newRetrievalListDetail.Quantity = item.Quantity;
                    newRetrievalListDetail.RetrievalListID = retrievalList.RetrievalListID;
                    retrievalList.RetrievalListDetails.Add(newRetrievalListDetail);
                }
            }
        }


        public RetrievalList CreateRetrievalList()
        {
            DateTime today = DateTime.Today;
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextFriday = today.AddDays(daysUntilFriday);
            DateTime lastSaturday = nextFriday.AddDays(-6);
            RetrievalList retrievalList = new RetrievalList { StartDate = lastSaturday, EndDate = nextFriday };
            db.RetrievalLists.Add(retrievalList);
            db.SaveChanges();
            return retrievalList;
        }

        //Update retrival list
        public void UpdateRetrievalList(RetrievalList retrievalList)
        {
            db.SaveChanges();
        }

        //Get retrieval list by retrieval id
        public RetrievalList GetRetrievalListById(int id)
        {
            return db.RetrievalLists.Include(x => x.RetrievalListDetails).Where(x => x.RetrievalListID == id).SingleOrDefault();
        }

        public RetrievalList FindCurrentRetrievalList()
        {
            DateTime today = DateTime.Today;
            RetrievalList retrievalList = db.RetrievalLists.SingleOrDefault(r => r.StartDate <= today && r.EndDate >= today);

            return retrievalList;
        }

        public float FindPrice(int itemID, int supplierID)
        {
            return db.SupplierItems.SingleOrDefault(s => s.SupplierID == supplierID && s.ItemID == itemID).Price;
        }

        public DeptEmployee FindRep(int deptId)
        {
            return db.DeptUsers.SingleOrDefault(d => d.DeptEmployee.DepartmentID == deptId && d.Role == DepartmentRole.REPRESENTATIVE).DeptEmployee;
        }
    }
}
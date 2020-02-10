using AD_Team10.DAL;
using AD_Team10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

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

        //public List<Requisition> GetRequisit

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
            return db.Requisitions.Include(x => x.Employee).Where(x => x.Status == Status.Approved).ToList();
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

        public List<Requisition> GetRequisitionByReqDetails(List<RequisitionDetail> requisitionDetails)
        {
            List<Requisition> requisitions = new List<Requisition>();
            foreach (var reqDetail in requisitionDetails)
            {
                Requisition requisition = db.Requisitions.Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                requisitions.Add(requisition);
            }
            return requisitions;
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

        public int DepartmentTotalItemQuantity(Item item, Department department)
        {
            List<DeptEmployee> depEmployees = db.DeptEmployees.Where(x => x.DepartmentID == department.DepartmentID).ToList();
            int quantity=0;
            foreach(var depEmp in depEmployees)
            {
                List<Requisition> requisitions = db.Requisitions.Where(x => x.EmployeeID == depEmp.DeptEmployeeID && x.Status != Status.Completed).ToList();
                foreach(var requisition in requisitions)
                {
                    List<RequisitionDetail> requisitionDetails = db.RequisitionDetails.Where(x => x.RequisitionID == requisition.RequisitionID && x.ItemID == item.ItemID).ToList();
                    foreach(var reqDetail in requisitionDetails)
                    {
                        quantity += (reqDetail.Quantity - reqDetail.QuantityReceived);
                    }
                }
            }
            return quantity;
        }

        //public int DepartmentCountForItem(Item item, List<RequisitionDetail> pendingRequisitionDetails)
        //{
        //    List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Where(x => x.ItemID == item.ItemID).ToList();
        //    List<Requisition> requisitions = new List<Requisition>();
        //    List<DepEmployee> employees = new List<DepEmployee>();
        //    foreach(var reqDetail in requisitionDetails)
        //    {
        //        Requisition requisition = db.Requisitions.Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
        //        DepEmployee depEmployee = db.DepEmployees.Where(x => x.DepEmployeeID == requisition.EmployeeID).SingleOrDefault();
        //        employees.Add(depEmployee);
        //    }
        //    List<Department> departments = new List<Department>();
        //    foreach(var employee in employees)
        //    {
        //        Department department = db.Departments.Where(x => x.DepartmentID == employee.DepartmentID).SingleOrDefault();
        //        if (!departments.Contains(department))
        //            departments.Add(department);
        //    }
        //    int quantity = departments.Count();
        //    return quantity;
        //}

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

        //Disbursement List
        public Dictionary<Item, int> GetDepartmentItemAndQuantity(Department department)
        {
            Dictionary<Item, int> departmentItemQuantity = new Dictionary<Item, int>();
            List<RequisitionDetail> approvedRequisitionDetails = this.GetApprovedRequisitionDetail();
            List<RequisitionDetail> departmentRequisitionDetails = approvedRequisitionDetails.Where(x => x.Requisition.Employee.DepartmentID == department.DepartmentID).ToList();
            foreach(var detail in departmentRequisitionDetails)
            {
                if (!departmentItemQuantity.ContainsKey(detail.Item))
                {
                    departmentItemQuantity.Add(detail.Item, detail.QuantityReceived);
                }
                else
                {
                    departmentItemQuantity[detail.Item] += detail.QuantityReceived;
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

        public List<RequisitionDetail> GetRequisitionDetailsByRequisitionID(int Id)
        {
            List<RequisitionDetail> requisitionDetails = db.RequisitionDetails.Include(x => x.Item).Include(x => x.Requisition).Where(x => x.RequisitionID == Id).ToList();
            return requisitionDetails;
        }

        public List<PurchaseOrder> GetPendingPurchaseOrder()
        {
            return db.PurchaseOrders.Include(x => x.Supplier).Include(x => x.PurchaseOrderDetails).Where(x => x.OrderStatus != OrderStatus.Completed).ToList();
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

        //Generate the last Retrieval List
        public RetrievalList GenerateRetrievalList()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            RetrievalList retrievalList = db.RetrievalLists.Include(x => x.RequisitionRetrievals)
                                                                  .Include(x => x.RetrievalListDetails)
                                                                  .Where(x => x.StartDate <= yesterday && x.EndDate >= yesterday)
                                                                  .SingleOrDefault();
            return retrievalList;
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

        //Get the correspond item quantity for retrieval list
        //public Dictionary<Item, int> GetTotalQuantityForItem(Item item, RetrievalList retrievalList)
        //{
            
        //}

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

        //check if the retrieval list contain specific Item
        public bool RetrievalListDetailContainItem(int id, List<RetrievalListDetail> retrievalListDetails)
        {
            if (retrievalListDetails.Where(x=>x.ItemID == id) != null)
                return true;
            else
                return false;
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
            db.Entry(retrievalList).State = EntityState.Modified;
            db.SaveChanges();
        }

        //Get retrieval list by retrieval id
        public RetrievalList GetRetrievalListById(int id)
        {
            return null;
            //return db.RetrievalLists.Include(x => x.Requisitions).Include(x => x.RetrievalListDetails).Where(x => x.RetrievalListID == id).SingleOrDefault();
        }

        public RetrievalList FindCurrentRetrievalList()
        {
            DateTime today = DateTime.Today;
            RetrievalList retrievalList = db.RetrievalLists.SingleOrDefault(r => r.StartDate <= today && r.EndDate >= today);
            return retrievalList;
        }
    }
}
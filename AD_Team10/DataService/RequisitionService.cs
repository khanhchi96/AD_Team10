using AD_Team10.DAL;
using AD_Team10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace AD_Team10.DataService
{
    public class RequisitionService
    {
        private DBContext dBContext = new DBContext();

        public List<Requisition> GetRequisitionByStatus(Status status)
        {
            List<Requisition> requisitions = dBContext.Requisitions.Where(x => x.Status == status).ToList();
            return requisitions;
        }

        //public List<Requisition> GetRequisit

        public Requisition Find(int? id)
        {
            Requisition requisition = dBContext.Requisitions.Find(id);
            return requisition;
        }

        public List<RequisitionDetail> GetPendingRequisitionDetails()
        {
            return dBContext.RequisitionDetails.Include(i => i.Requisition).Include(x => x.Item).Where(x => x.Requisition.Status != Status.Completed).ToList();
        }

        public List<Requisition> GetPendingRequisitions()
        {
            return dBContext.Requisitions.Include(x => x.Employee).Where(x => x.Status != Status.Completed).ToList();
        }

        public Dictionary<Requisition, int> GetRequisitionByItem(Item item, List<RequisitionDetail> pendingRequisitionDetails)
        {
            //List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Include(x => x.Item).Where(x => x.Item == item).ToList();
            //var requisitions = dBContext.Requisitions.Where(x => x.RequisitionID == requisitionDetails.)
            List<Requisition> requisitions = new List<Requisition>();
            Dictionary<Requisition, int> requisitionQuantity = new Dictionary<Requisition, int>();
            //foreach (var reqDetail in pendingRequisitionDetails)
            //{
            //    RequisitionDetail requisitionDetails = dBContext.RequisitionDetails.Where(x => x. == reqDetail.RequisitionID).SingleOrDefault();
            //    requisitions.Add(requisition);
            //}
            //return requisitions;
            List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Where(x => x.ItemID == item.ItemID).ToList();
            foreach(var reqDetail in requisitionDetails)
            {
                int quantity = reqDetail.Quantity;
                Requisition requisition = dBContext.Requisitions.Include(x => x.Employee).Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                requisitionQuantity.Add(requisition, quantity);
            }
            return requisitionQuantity;
        }

        public List<Requisition> GetRequisitionByReqDetails(List<RequisitionDetail> requisitionDetails)
        {
            List<Requisition> requisitions = new List<Requisition>();
            foreach (var reqDetail in requisitionDetails)
            {
                Requisition requisition = dBContext.Requisitions.Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                requisitions.Add(requisition);
            }
            return requisitions;
        }

        public List<Item> GetDistinctItem(List<RequisitionDetail> requisitionDetails)
        {
            List<Item> items = new List<Item>();
            foreach(var reqDetail in requisitionDetails)
            {
                Item item = dBContext.Items.Where(x => x.ItemID == reqDetail.ItemID).SingleOrDefault();
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
                    itemQuantity.Add(reqDetail.Item, reqDetail.Quantity-reqDetail.QuantityDelivered);
                }
                else
                {
                    quantity = reqDetail.Quantity-reqDetail.QuantityDelivered;
                    itemQuantity[reqDetail.Item] += quantity;
                }
            }
            return itemQuantity;
        }

        public int DepartmentTotalItemQuantity(Item item, Department department)
        {
            List<DepEmployee> depEmployees = dBContext.DepEmployees.Where(x => x.DepartmentID == department.DepartmentID).ToList();
            int quantity=0;
            foreach(var depEmp in depEmployees)
            {
                List<Requisition> requisitions = dBContext.Requisitions.Where(x => x.EmployeeID == depEmp.DepEmployeeID && x.Status != Status.Completed).ToList();
                foreach(var requisition in requisitions)
                {
                    List<RequisitionDetail> requisitionDetails = dBContext.RequisitionDetails.Where(x => x.RequisitionID == requisition.RequisitionID && x.ItemID == item.ItemID).ToList();
                    foreach(var reqDetail in requisitionDetails)
                    {
                        quantity += (reqDetail.Quantity - reqDetail.QuantityDelivered);
                    }
                }
            }
            return quantity;
        }

        public int DepartmentCountForItem(Item item, List<RequisitionDetail> pendingRequisitionDetails)
        {
            List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Where(x => x.ItemID == item.ItemID).ToList();
            List<Requisition> requisitions = new List<Requisition>();
            List<DepEmployee> employees = new List<DepEmployee>();
            foreach(var reqDetail in requisitionDetails)
            {
                Requisition requisition = dBContext.Requisitions.Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                DepEmployee depEmployee = dBContext.DepEmployees.Where(x => x.DepEmployeeID == requisition.EmployeeID).SingleOrDefault();
                employees.Add(depEmployee);
            }
            //int quantity = employees.GroupBy(x => x.DepartmentID).;
            List<Department> departments = new List<Department>();
            foreach(var employee in employees)
            {
                Department department = dBContext.Departments.Where(x => x.DepartmentID == employee.DepartmentID).SingleOrDefault();
                if (!departments.Contains(department))
                    departments.Add(department);
            }
            int quantity = departments.Count();
            return quantity;
        }

        public List<Department> DepartmentsForItem(Item item, List<RequisitionDetail> pendingRequisitionDetails)
        {
            List<RequisitionDetail> requisitionDetails = pendingRequisitionDetails.Where(x => x.ItemID == item.ItemID).ToList();
            List<Requisition> requisitions = new List<Requisition>();
            List<DepEmployee> employees = new List<DepEmployee>();
            foreach (var reqDetail in requisitionDetails)
            {
                Requisition requisition = dBContext.Requisitions.Where(x => x.RequisitionID == reqDetail.RequisitionID).SingleOrDefault();
                DepEmployee depEmployee = dBContext.DepEmployees.Where(x => x.DepEmployeeID == requisition.EmployeeID).SingleOrDefault();
                employees.Add(depEmployee);
            }
            //int quantity = employees.GroupBy(x => x.DepartmentID).;
            List<Department> departments = new List<Department>();
            foreach (var employee in employees)
            {
                Department department = dBContext.Departments.Where(x => x.DepartmentID == employee.DepartmentID).SingleOrDefault();
                if (!departments.Contains(department))
                    departments.Add(department);
            }
            return departments;
        }

        public List<Department> GetDepartments()
        {
            return dBContext.Departments.ToList();
        }

        public List<CollectionPoint> GetCollectionPoints()
        {
            return dBContext.CollectionPoints.ToList();
        }

        public List<RequisitionDetail> GetRequisitionDetailsByRequisitionID(int Id)
        {
            //List<RequisitionDetail> requisitionDetails = dBContext.RequisitionDetails.Include(x => x.Item).Include(x => x.Requisition).Where(x => x.RequisitionID == Id).Select(x => new RequisitionDetail()
            //{
            //    Item = x.Item,
            //    Quantity = x.Quantity,
            //    QuantityDelivered = x.QuantityDelivered
            //}).ToList();
            List<RequisitionDetail> requisitionDetails = dBContext.RequisitionDetails.Include(x => x.Item).Include(x => x.Requisition).Where(x => x.RequisitionID == Id).ToList();
            return requisitionDetails;
        }

        public List<PurchaseOrder> GetPendingPurchaseOrder()
        {
            return dBContext.PurchaseOrders.Include(x => x.Supplier).Include(x => x.PurchaseOrderDetails).Where(x => x.OrderStatus != OrderStatus.Completed).ToList();
        }

        public Requisition GetRequisitionById(int id)
        {
            return dBContext.Requisitions.Include(x => x.Employee).Include(x => x.RequisitionDetails).Where(x => x.RequisitionID == id).SingleOrDefault();
        }

        public bool IsCompleted(Requisition requisition)
        {
            bool isCompleted = true;
            for(int i = 0; i < requisition.RequisitionDetails.Count; i++)
            {
                if(requisition.RequisitionDetails[i].Quantity != requisition.RequisitionDetails[i].QuantityDelivered)
                {
                    isCompleted = false;
                    break;
                }
            }
            return isCompleted;
        }

        public void UpdateRequisition(Requisition requisition)
        {
            dBContext.Entry(requisition).State = EntityState.Modified;
            dBContext.SaveChanges();
        }
    }
}
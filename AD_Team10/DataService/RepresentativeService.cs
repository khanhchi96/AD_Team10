using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AD_Team10.DAL;
using AD_Team10.Models;
using System.Data.Entity;

namespace AD_Team10.DataService
{
    public class RepresentativeService
    {

        private DBContext db = new DBContext();

        public List<Supplier> GetSuppliersList()
        {
            return db.Suppliers.ToList();
        }

        public List<PurchaseOrderDetail> GetOrderDetailByOrderId(int id)
        {
            return db.PurchaseOrderDetails.Where(x => x.OrderID == id).ToList();
        }

        public PurchaseOrder GetPurchaseOrder(int id)
        {
            return db.PurchaseOrders.Include(x => x.Supplier).Include(x => x.PurchaseOrderDetails).Where(x => x.PurchaseOrderID == id).SingleOrDefault();
        }

        public List<PurchaseOrder> GetPurchaseOrderHistory()
        {
            return db.PurchaseOrders.Include(x => x.Supplier).Include(x => x.PurchaseOrderDetails).Where(x => x.OrderStatus == OrderStatus.Completed).ToList();
        }

        public bool IsComoleted(PurchaseOrder purchaseOrder)
        {
            bool isCompleted = true;
            foreach (var item in purchaseOrder.PurchaseOrderDetails)
            {
                if (item.Quantity != item.QuantityReceived)
                {
                    isCompleted = false;
                    break;
                }
            }
            return isCompleted;
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            db.Entry(purchaseOrder).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
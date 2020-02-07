namespace AD_Team10.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using AD_Team10.Models;

    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public DbSet<AdjustmentVoucher> AdjustmentVouchers { get; set; }
        public DbSet<AdjustmentVoucherDetail> AdjustmentVoucherDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CollectionPoint> CollectionPoints { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepEmployee> DepEmployees { get; set; }
        public DbSet<DepUser> DepUsers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<RequisitionDetail> RequisitionDetails { get; set; }
        public DbSet<StoreEmployee> StoreEmployees { get; set; }
        public DbSet<StoreUser> StoreUsers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierItem> SupplierItems { get; set; }
        public DbSet<RetrievalList> RetrievalLists { get; set; }
        public DbSet<RetrievalListDetail> RetrievalListDetails { get; set; }
  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

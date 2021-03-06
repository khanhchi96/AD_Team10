namespace AD_Team10.DAL
{
    using System.Data.Entity;
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
        public DbSet<DeptEmployee> DeptEmployees { get; set; }
        public DbSet<DeptUser> DeptUsers { get; set; }
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
  
        public DbSet<RequisitionRetrieval> RequisitionRetrievals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

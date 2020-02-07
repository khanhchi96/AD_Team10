namespace AD_Team10.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using AD_Team10.Models;
    using AD_Team10.DAL;

    internal sealed class Configuration : DbMigrationsConfiguration<AD_Team10.DAL.DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AD_Team10.DAL.DBContext context)
        {

            context.CollectionPoints.AddOrUpdate(c => c.CollectionPointID,
                new CollectionPoint { CollectionPointID = 1, CollectionPointName = "Stationery Store - Administration Building" },
                new CollectionPoint { CollectionPointID = 2, CollectionPointName = "Management School" },
                new CollectionPoint { CollectionPointID = 3, CollectionPointName = "Medical School" },
                new CollectionPoint { CollectionPointID = 4, CollectionPointName = "Engineering School" },
                new CollectionPoint { CollectionPointID = 5, CollectionPointName = "Science School" },
                new CollectionPoint { CollectionPointID = 6, CollectionPointName = "University Hospital" }
            );

            context.Departments.AddOrUpdate(d => d.DepartmentID,
                new Department { DepartmentID = 1, DepartmentCode = "ENGL", DepartmentName = "English Department", CollectionPointID = 1 },
                new Department { DepartmentID = 2, DepartmentCode = "CPSC", DepartmentName = "Computer Science", CollectionPointID = 2 }
            );
            context.DepEmployees.AddOrUpdate(d => d.DepEmployeeID,
                new DepEmployee
                {
                    DepEmployeeID = 1,
                    DepartmentID = 1,
                    FirstName = "Bill",
                    LastName = "Gates",
                    Gender = "Male",
                    Email = "billgates@gmail.com",
                    Designation = "Lecturer",
                    Phone = "99996666"
                },
                new DepEmployee
                {
                    DepEmployeeID = 2,
                    DepartmentID = 2,
                    FirstName = "Yibo",
                    LastName = "Wang",
                    Gender = "Male",
                    Email = "wangyibo@gmail.com",
                    Designation = "Lecturer",
                    Phone = "83198747"
                },
                new DepEmployee
                {
                    DepEmployeeID = 3,
                    DepartmentID = 1,
                    FirstName = "Angela",
                    LastName = "Baby",
                    Gender = "Female",
                    Email = "angelababy@gmail.com",
                    Designation = "Lecturer",
                    Phone = "13889329"
                });

            context.DepUsers.AddOrUpdate(d => d.DepUserID,
                new DepUser { DepUserID = 1, Username = "bill.gates", Password = "12345678", Role = DepartmentRole.EMPLOYEE, DepEmployeeID = 1 },
                new DepUser { DepUserID = 2, Username = "wang.yibo", Password = "12345678", Role = DepartmentRole.EMPLOYEE, DepEmployeeID = 2 },
                new DepUser { DepUserID = 3, Username = "angela.baby", Password = "12345678", Role = DepartmentRole.REPRESENTATIVE, DepEmployeeID = 3 });

            context.StoreEmployees.AddOrUpdate(s => s.StoreEmployeeID,
                new StoreEmployee { StoreEmployeeID = 1,  FirstName = "Esther", LastName = "Tan", Gender = "Female", Phone = "76548097", Email = "esthertan@gmail.com", Designation = "Clerk" },
                new StoreEmployee { StoreEmployeeID = 2, FirstName = "Yibo", LastName = "Wang", Gender = "Male", Phone = "76548097", Email = "wangyibo@gmail.com", Designation = "Supervisor" },
                new StoreEmployee { StoreEmployeeID = 3, FirstName = "Justin", LastName = "Bieber", Gender = "Male", Phone = "76548097", Email = "justinbieber@gmail.com", Designation = "Manager" }
            );
            context.StoreUsers.AddOrUpdate(s => s.StoreUserID,
                new StoreUser { StoreUserID = 1, Username = "esther.tan", Password = "12345678", Role = StoreRole.CLERK, StoreEmployeeID = 1 },
                new StoreUser { StoreUserID = 2, Username = "wang.yibo", Password = "12345678", Role = StoreRole.SUPERVISOR, StoreEmployeeID = 2 },
                new StoreUser { StoreUserID = 3, Username = "justin.bieber", Password = "12345678", Role = StoreRole.MANAGER, StoreEmployeeID = 3 });

            context.Categories.AddOrUpdate(c => c.CategoryId,
                new Category { CategoryId = 1, CategoryName = "Clip" },
                new Category { CategoryId = 2, CategoryName = "Pencil" });
            context.Items.AddOrUpdate(i => i.ItemID,
                new Item { ItemID = 1, Description = "Clip 3/4", UnitOfMeasure = "Dozen", ReorderLevel = 150, ReorderQuantity = 150, CategoryID = 1, UnitsInStock = 160 },
                new Item { ItemID = 2, Description = "Clip 1/2", UnitOfMeasure = "Dozen", ReorderLevel = 150, ReorderQuantity = 150, CategoryID = 1, UnitsInStock = 120 },
                new Item { ItemID = 3, Description = "2B Pencil", UnitOfMeasure = "Dozen", ReorderLevel = 150, ReorderQuantity = 150, CategoryID = 2, UnitsInStock = 120 });
            
            context.RetrievalLists.AddOrUpdate(r => r.RetrievalListID,
                new RetrievalList { RetrievalListID = 1, StartDate = DateTime.Parse("2019-10-05"), EndDate = DateTime.Parse("2019-10-11") },
                new RetrievalList { RetrievalListID = 2, StartDate = DateTime.Parse("2019-10-12"), EndDate = DateTime.Parse("2019-10-18") },
                new RetrievalList { RetrievalListID = 3, StartDate = DateTime.Parse("2019-10-19"), EndDate = DateTime.Parse("2019-10-25") },
                new RetrievalList { RetrievalListID = 4, StartDate = DateTime.Parse("2019-10-26"), EndDate = DateTime.Parse("2019-11-01") },
                new RetrievalList { RetrievalListID = 5, StartDate = DateTime.Parse("2019-11-02"), EndDate = DateTime.Parse("2019-11-08") },
                new RetrievalList { RetrievalListID = 6, StartDate = DateTime.Parse("2019-11-09"), EndDate = DateTime.Parse("2019-11-15") },
                new RetrievalList { RetrievalListID = 7, StartDate = DateTime.Parse("2019-11-16"), EndDate = DateTime.Parse("2019-11-22") },
                new RetrievalList { RetrievalListID = 8, StartDate = DateTime.Parse("2019-11-23"), EndDate = DateTime.Parse("2019-11-29") },
                new RetrievalList { RetrievalListID = 9, StartDate = DateTime.Parse("2019-11-30"), EndDate = DateTime.Parse("2019-12-06") },
                new RetrievalList { RetrievalListID = 10, StartDate = DateTime.Parse("2019-12-07"), EndDate = DateTime.Parse("2019-12-13") },
                new RetrievalList { RetrievalListID = 11, StartDate = DateTime.Parse("2019-12-14"), EndDate = DateTime.Parse("2019-12-20") },
                new RetrievalList { RetrievalListID = 12, StartDate = DateTime.Parse("2019-12-21"), EndDate = DateTime.Parse("2019-11-27") },
                new RetrievalList { RetrievalListID = 13, StartDate = DateTime.Parse("2019-12-28"), EndDate = DateTime.Parse("2020-01-03") },
                new RetrievalList { RetrievalListID = 14, StartDate = DateTime.Parse("2020-01-04"), EndDate = DateTime.Parse("2020-01-10") },
                new RetrievalList { RetrievalListID = 15, StartDate = DateTime.Parse("2020-01-11"), EndDate = DateTime.Parse("2020-01-17") },
                new RetrievalList { RetrievalListID = 16, StartDate = DateTime.Parse("2020-01-18"), EndDate = DateTime.Parse("2020-01-24") },
                new RetrievalList { RetrievalListID = 17, StartDate = DateTime.Parse("2020-01-25"), EndDate = DateTime.Parse("2020-01-31") },
                new RetrievalList { RetrievalListID = 18, StartDate = DateTime.Parse("2020-02-01"), EndDate = DateTime.Parse("2020-02-07") },
                new RetrievalList { RetrievalListID = 19, StartDate = DateTime.Parse("2020-02-08"), EndDate = DateTime.Parse("2020-02-14") },
                new RetrievalList { RetrievalListID = 20, StartDate = DateTime.Parse("2020-02-15"), EndDate = DateTime.Parse("2020-02-21") },
                new RetrievalList { RetrievalListID = 21, StartDate = DateTime.Parse("2020-02-22"), EndDate = DateTime.Parse("2020-02-28") },
                new RetrievalList { RetrievalListID = 22, StartDate = DateTime.Parse("2020-02-29"), EndDate = DateTime.Parse("2020-03-06") },
                new RetrievalList { RetrievalListID = 23, StartDate = DateTime.Parse("2020-03-07"), EndDate = DateTime.Parse("2020-03-14") },
                new RetrievalList { RetrievalListID = 24, StartDate = DateTime.Parse("2020-03-15"), EndDate = DateTime.Parse("2019-03-21") }
            );

            context.Requisitions.AddOrUpdate(r => r.RequisitionID,
                new Requisition
                {
                    RequisitionID = 1,
                    RequisitionDate = DateTime.Parse("2019-10-22"),
                    ApprovalDate = DateTime.Parse("2019-10-23"),
                    CompletedDate = DateTime.Parse("2019-10-28"),
                    EmployeeID = 1,
                    RetrievalListID = 4,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 2,
                    RequisitionDate = DateTime.Parse("2019-11-11"),
                    ApprovalDate = DateTime.Parse("2019-11-12"),
                    CompletedDate = DateTime.Parse("2019-11-18"),
                    EmployeeID = 2,
                    RetrievalListID = 7,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 3,
                    RequisitionDate = DateTime.Parse("2019-12-22"),
                    ApprovalDate = DateTime.Parse("2019-12-23"),
                    CompletedDate = DateTime.Parse("2019-12-30"),
                    EmployeeID = 3,
                    RetrievalListID = 13,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 4,
                    RequisitionDate = DateTime.Parse("2020-01-01"),
                    ApprovalDate = DateTime.Parse("2020-01-02"),
                    CompletedDate = DateTime.Parse("2019-01-06"),
                    EmployeeID = 1,
                    RetrievalListID = 13,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 5,
                    RequisitionDate = DateTime.Parse("2020-01-10"),
                    ApprovalDate = DateTime.Parse("2020-01-11"),
                    CompletedDate = DateTime.Parse("2020-01-13"),
                    EmployeeID = 3,
                    RetrievalListID = 15,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 6,
                    RequisitionDate = DateTime.Parse("2020-01-14"),
                    ApprovalDate = DateTime.Parse("2020-01-15"),
                    CompletedDate = DateTime.Parse("2020-01-20"),
                    EmployeeID = 2,
                    RetrievalListID = 15,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 7,
                    RequisitionDate = DateTime.Parse("2020-01-16"),
                    ApprovalDate = DateTime.Parse("2020-01-17"),
                    CompletedDate = DateTime.Parse("2020-01-20"),
                    EmployeeID = 3,
                    RetrievalListID = 15,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 8,
                    RequisitionDate = DateTime.Parse("2020-01-18"),
                    ApprovalDate = DateTime.Parse("2020-01-19"),
                    CompletedDate = DateTime.Parse("2020-01-20"),
                    EmployeeID = 2,
                    RetrievalListID = 16,
                    Status = Status.Completed
                });

            context.RequisitionDetails.AddOrUpdate(r => new { r.RequisitionID, r.ItemID },
                new RequisitionDetail
                {
                    RequisitionID = 1,
                    ItemID = 1,
                    Quantity = 5,
                    QuantityReceived = 5
                },
                new RequisitionDetail
                {
                    RequisitionID = 1,
                    ItemID = 2,
                    Quantity = 7,
                    QuantityReceived = 7
                },
                new RequisitionDetail
                {
                    RequisitionID = 1,
                    ItemID = 3,
                    Quantity = 10,
                    QuantityReceived = 10
                },
                new RequisitionDetail
                {
                    RequisitionID = 2,
                    ItemID = 1,
                    Quantity = 3,
                    QuantityReceived = 3
                },
                new RequisitionDetail
                {
                    RequisitionID = 3,
                    ItemID = 1,
                    Quantity = 6,
                    QuantityReceived = 6
                },
                new RequisitionDetail
                {
                    RequisitionID = 3,
                    ItemID = 2,
                    Quantity = 5,
                    QuantityReceived = 5
                },
                new RequisitionDetail
                {
                    RequisitionID = 3,
                    ItemID = 3,
                    Quantity = 4,
                    QuantityReceived = 4
                },
                new RequisitionDetail
                {
                    RequisitionID = 4,
                    ItemID = 1,
                    Quantity = 8,
                    QuantityReceived = 8
                },
                new RequisitionDetail
                {
                    RequisitionID = 4,
                    ItemID = 2,
                    Quantity = 2,
                    QuantityReceived = 2
                },
                new RequisitionDetail
                {
                    RequisitionID = 5,
                    ItemID = 1,
                    Quantity = 9,
                    QuantityReceived = 9
                },
                new RequisitionDetail
                {
                    RequisitionID = 5,
                    ItemID = 3,
                    Quantity = 5,
                    QuantityReceived = 5
                },
                new RequisitionDetail
                {
                    RequisitionID = 6,
                    ItemID = 1,
                    Quantity = 7,
                    QuantityReceived = 7
                },
                new RequisitionDetail
                {
                    RequisitionID = 7,
                    ItemID = 1,
                    Quantity = 5,
                    QuantityReceived = 5
                },
                new RequisitionDetail
                {
                    RequisitionID = 7,
                    ItemID = 2,
                    Quantity = 7,
                    QuantityReceived = 7
                },
                new RequisitionDetail
                {
                    RequisitionID = 8,
                    ItemID = 2,
                    Quantity = 10,
                    QuantityReceived = 10
                },
                new RequisitionDetail
                {
                    RequisitionID = 8,
                    ItemID = 3,
                    Quantity = 5,
                    QuantityReceived = 5
                }
                );

            context.Suppliers.AddOrUpdate(s => s.SupplierID,
                new Supplier { SupplierID = 1, SupplierCode = "ALPA", SupplierName = "ALPHA Office Supplies" });

            context.SupplierItems.AddOrUpdate(s => new { s.SupplierID, s.ItemID },
                new SupplierItem { SupplierID = 1, ItemID = 1, Price = 20 },
                new SupplierItem { SupplierID = 1, ItemID = 2, Price = 50 },
                new SupplierItem { SupplierID = 1, ItemID = 3, Price = 40 });
            context.PurchaseOrders.AddOrUpdate(p => p.PurchaseOrderID,
                new PurchaseOrder {
                    PurchaseOrderID = 1,
                    OrderDate = DateTime.Parse("2019-12-01"),
                    SupplierID = 1,
                    CompletedDate = DateTime.Parse("2019-12-07"),
                    OrderStatus = OrderStatus.Completed
                },
                new PurchaseOrder
                {
                    PurchaseOrderID = 2,
                    OrderDate = DateTime.Parse("2019-12-07"),
                    SupplierID = 1,
                    CompletedDate = DateTime.Parse("2019-12-14"),
                    OrderStatus = OrderStatus.Completed
                },
                new PurchaseOrder
                {
                    PurchaseOrderID = 3,
                    OrderDate = DateTime.Parse("2019-12-15"),
                    SupplierID = 1,
                    CompletedDate = DateTime.Parse("2019-12-21"),
                    OrderStatus = OrderStatus.Completed
                }
                );

            context.PurchaseOrderDetails.AddOrUpdate(p => new { p.OrderID, p.ItemID },
                new PurchaseOrderDetail {
                    OrderID = 1,
                    ItemID = 1,
                    Quantity = 120,
                    QuantityReceived = 120,
                },
                new PurchaseOrderDetail
                {
                    OrderID = 1,
                    ItemID = 2,
                    Quantity = 140,
                    QuantityReceived = 140,
                },
                new PurchaseOrderDetail
                {
                    OrderID = 2,
                    ItemID = 1,
                    Quantity = 150,
                    QuantityReceived = 150,
                },
                new PurchaseOrderDetail
                {
                    OrderID = 2,
                    ItemID = 3,
                    Quantity = 120,
                    QuantityReceived = 120,
                },
                new PurchaseOrderDetail
                {
                    OrderID = 3,
                    ItemID = 3,
                    Quantity = 180,
                    QuantityReceived = 180,
                });
        }
    }
}


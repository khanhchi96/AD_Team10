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
                new Category { CategoryId = 1, CategoryName = "Clip" });
            context.Items.AddOrUpdate(i => i.ItemID,
                new Item { ItemID = 1, Description = "Clip 3/4", UnitOfMeasure = "Dozen", ReorderLevel = 150, ReorderQuantity = 150, CategoryID = 1, UnitsInStock = 160 },
                new Item { ItemID = 2, Description = "Clip 1/2", UnitOfMeasure = "Dozen", ReorderLevel = 150, ReorderQuantity = 150, CategoryID = 1, UnitsInStock = 120 });
            context.Requisitions.AddOrUpdate(r => r.RequisitionID,
                new Requisition
                {
                    RequisitionID = 1,
                    RequisitionDate = DateTime.Parse("2019-10-22"),
                    CompletedDate = DateTime.Parse("2019-10-28"),
                    EmployeeID = 1,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 2,
                    RequisitionDate = DateTime.Parse("2019-11-11"),
                    CompletedDate = DateTime.Parse("2019-11-18"),
                    EmployeeID = 2,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 3,
                    RequisitionDate = DateTime.Parse("2019-12-22"),
                    CompletedDate = DateTime.Parse("2019-12-30"),
                    EmployeeID = 3,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 4,
                    RequisitionDate = DateTime.Parse("2020-01-01"),
                    CompletedDate = DateTime.Parse("2019-01-06"),
                    EmployeeID = 1,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 5,
                    RequisitionDate = DateTime.Parse("2019-01-10"),
                    CompletedDate = DateTime.Parse("2019-01-13"),
                    EmployeeID = 3,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 6,
                    RequisitionDate = DateTime.Parse("2019-01-14"),
                    CompletedDate = DateTime.Parse("2019-01-20"),
                    EmployeeID = 2,
                    Status = Status.Completed
                },
                new Requisition
                {
                    RequisitionID = 7,
                    RequisitionDate = DateTime.Parse("2019-01-16"),
                    CompletedDate = DateTime.Parse("2019-01-20"),
                    EmployeeID = 3,
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
                }
                );
        }
    }
}


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
                });

            context.DepUsers.AddOrUpdate(d => d.DepUserID,
                new DepUser { DepUserID = 1, Username = "bill.gates", Password = "12345678", Role = DepartmentRole.EMPLOYEE, DepEmployeeID = 1 });

            context.StoreEmployees.AddOrUpdate(s => s.StoreEmployeeID,
                new StoreEmployee { StoreEmployeeID = 1,  FirstName = "Esther", LastName = "Tan", Gender = "Female", Phone = "76548097", Email = "esthertan@gmail.com", Designation = "Clerk" },
                new StoreEmployee { StoreEmployeeID = 2, FirstName = "Yibo", LastName = "Wang", Gender = "Male", Phone = "76548097", Email = "wangyibo@gmail.com", Designation = "Supervisor" },
                new StoreEmployee { StoreEmployeeID = 3, FirstName = "Justin", LastName = "Bieber", Gender = "Male", Phone = "76548097", Email = "justinbieber@gmail.com", Designation = "Manager" }
            );
            context.StoreUsers.AddOrUpdate(s => s.StoreUserID,
                new StoreUser { StoreUserID = 1, Username = "esther.tan", Password = "12345678", Role = StoreRole.CLERK, StoreEmployeeID = 1 },
                new StoreUser { StoreUserID = 2, Username = "wang.yibo", Password = "12345678", Role = StoreRole.SUPERVISOR, StoreEmployeeID = 2 },
                new StoreUser { StoreUserID = 3, Username = "justin.bieber", Password = "12345678", Role = StoreRole.MANAGER, StoreEmployeeID = 3 });

            context.Items.AddOrUpdate(i => i.ItemID,
                new Item { ItemID = 1, Description = "Pencil 2B", UnitOfMeasure = "dozen", ReorderLevel = 100, ReorderQuantity = 200, UnitsInStock = 150, CategoryID = 1 },
                new Item { ItemID = 2, Description = "Pencil 4B", UnitOfMeasure = "dozen", ReorderLevel = 100, ReorderQuantity = 200, UnitsInStock = 150, CategoryID = 1 },
                new Item { ItemID = 3, Description = "Pencil 6B", UnitOfMeasure = "dozen", ReorderLevel = 100, ReorderQuantity = 200, UnitsInStock = 150, CategoryID = 1 },
                new Item { ItemID = 4, Description = "Eraser (hard)", UnitOfMeasure = "each", ReorderLevel = 40, ReorderQuantity = 60, UnitsInStock = 45, CategoryID = 2 },
                new Item { ItemID = 5, Description = "Eraser (soft)", UnitOfMeasure = "each", ReorderLevel = 40, ReorderQuantity = 60, UnitsInStock = 50, CategoryID = 2 },
                new Item { ItemID = 6, Description = "Envelope Brown(3'x6')", UnitOfMeasure = "each", ReorderLevel = 600, ReorderQuantity = 400, UnitsInStock = 700, CategoryID = 3 },
                new Item { ItemID = 7, Description = "Envelope Brown(5'x7')", UnitOfMeasure = "each", ReorderLevel = 600, ReorderQuantity = 400, UnitsInStock = 400, CategoryID = 3 });

            context.Categories.AddOrUpdate(c => c.CategoryId,
                new Category { CategoryId = 1, CategoryName = "Pen" },
                new Category { CategoryId = 2, CategoryName = "Eraser" },
                new Category { CategoryId = 3, CategoryName = "Envelope" });

            context.Requisitions.AddOrUpdate(r => r.RequisitionID,
                new Requisition { RequisitionID = 1, RequisitionDate = new DateTime(2019, 09, 10), Status = Status.Completed, EmployeeID = 1 },
                new Requisition { RequisitionID = 2, RequisitionDate = new DateTime(2019, 09, 30), Status = Status.Incompleted, EmployeeID = 1 },
                new Requisition { RequisitionID = 3, RequisitionDate = new DateTime(2019, 10, 5), Status = Status.Pending, EmployeeID = 1});

            context.RequisitionDetails.AddOrUpdate(rd => new { rd.RequisitionID, rd.ItemID },
                new RequisitionDetail { RequisitionID = 1, ItemID = 1, Quantity = 2, QuantityReceived = 2 },
                new RequisitionDetail { RequisitionID = 1, ItemID = 2, Quantity = 1, QuantityReceived = 1 },
                new RequisitionDetail { RequisitionID = 1, ItemID = 5, Quantity = 3, QuantityReceived = 3 },
                new RequisitionDetail { RequisitionID = 2, ItemID = 1, Quantity = 2, QuantityReceived = 2 },
                new RequisitionDetail { RequisitionID = 2, ItemID = 4, Quantity = 3, QuantityReceived = 1 },
                new RequisitionDetail { RequisitionID = 2, ItemID = 6, Quantity = 2, QuantityReceived = 2 },
                new RequisitionDetail { RequisitionID = 3, ItemID = 3, Quantity = 2, QuantityReceived = 0 },
                new RequisitionDetail { RequisitionID = 3, ItemID = 6, Quantity = 10, QuantityReceived = 0 },
                new RequisitionDetail { RequisitionID = 3, ItemID = 7, Quantity = 5, QuantityReceived = 0 });


        }
    }
}

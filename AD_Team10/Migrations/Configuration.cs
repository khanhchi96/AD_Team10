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

            //context.Requisitions.AddOrUpdate(
            //    new Requisition { RequisitionID = 1, EmployeeID = 1 });

            //context.RequisitionDetails.AddOrUpdate(
            //    new RequisitionDetail { ItemID = 1, RequisitionID = 1 });

        }
    }
}

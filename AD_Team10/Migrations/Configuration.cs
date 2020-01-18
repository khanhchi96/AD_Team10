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
                new CollectionPoint { CollectionPointName = "Stationery Store - Administration Building" },
                new CollectionPoint { CollectionPointName = "Management School" },
                new CollectionPoint { CollectionPointName = "Medical School" },
                new CollectionPoint { CollectionPointName = "Engineering School" },
                new CollectionPoint { CollectionPointName = "Science School" },
                new CollectionPoint { CollectionPointName = "University Hospital" }
            );

            context.Departments.AddOrUpdate(d => d.DepartmentID,
                new Department { DepartmentCode = "ENGL", DepartmentName = "English Department", CollectionPointID = 1 },
                new Department { DepartmentCode = "CPSC", DepartmentName = "Computer Science", CollectionPointID = 2 }
            );
            context.DepEmployees.AddOrUpdate(d => d.DepEmployeeID,
                new DepEmployee
                {
                    DepartmentID = 1,
                    FirstName = "Bill",
                    LastName = "Gates",
                    Gender = "Male",
                    Email = "billgates@gmail.com",
                    Designation = "Lecturer",
                    Phone = "99996666"
                });

            context.DepUsers.AddOrUpdate(d => d.DepUserID,
                new DepUser { Username = "bill.gates", Password = "12345678", Role = DepartmentRole.EMPLOYEE, DepEmployeeID = 1 });

            context.StoreEmployees.AddOrUpdate(s => s.StoreEmployeeID,
                new StoreEmployee { FirstName = "Esther", LastName = "Tan", Gender = "Female", Phone = "76548097", Email = "esthertan@gmail.com", Designation = "Clerk" });
            context.StoreUsers.AddOrUpdate(s => s.StoreUserID,
                new StoreUser { Username = "esther.tan", Password = "12345678", Role = StoreRole.CLERK, StoreEmployeeID = 1 });

        }
    }
}

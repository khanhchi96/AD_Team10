using AD_Team10.DAL;
using AD_Team10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AD_Team10.DataService
{
    public class RepresentativeService
    {
        public static string stringCurrentCollection = "Your current collection point is:";
        public static string changeCollection = "Change Collection Point";

        private DBContext dB = new DBContext();

        //Get the department by department id
        public Department GetDepartmentById(int id)
        {
            return dB.Departments.Include(x => x.CollectionPoint)
                                 .Include(x => x.Employees)
                                 .Where(x => x.DepartmentID == id)
                                 .SingleOrDefault();
        }

        public CollectionPoint GetCollectionPointById(int id)
        {
            return dB.CollectionPoints.Include(x => x.Departments)
                                      .Where(x => x.CollectionPointID == id)
                                      .SingleOrDefault();
        }

        public List<CollectionPoint> GetCollectionPointsList()
        {
            return dB.CollectionPoints.Include(x => x.Departments)
                                      .ToList();
        }

        //Update collection point
        public void UpdateDepartment(Department department)
        {
            dB.Entry(department).State = EntityState.Modified;
            dB.SaveChanges();
        }
    }
}
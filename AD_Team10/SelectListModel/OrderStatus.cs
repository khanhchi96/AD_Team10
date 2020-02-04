using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AD_Team10.SelectListModel
{
    public class SelectOrderStatus
    {
        //public string Status { get; set; }
        //public List<SelectListItem> GetSelectList()
        //{
        //    List<SelectListItem> listItems = new List<SelectListItem>
        //    {
        //        new SelectListItem{Text="Status", Value="-1"},
        //        new SelectListItem{Text="Pending", Value="0"},
        //        new SelectListItem{Text="Delivering", Value="1" },
        //        new SelectListItem{Text="Incompleted", Value="2" },
        //        new SelectListItem{Text="Completed", Value="3" }
        //    };
        //    return listItems;
        //}
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }

        public List<SelectOrderStatus> GetSelectOrderList()
        {
            List<SelectOrderStatus> selectOrderStatuses = new List<SelectOrderStatus>
        {
            new SelectOrderStatus{OrderStatusId = 0, OrderStatusName="Status" },
            new SelectOrderStatus{OrderStatusId = 1, OrderStatusName="Pending" },
            new SelectOrderStatus{OrderStatusId = 2, OrderStatusName="Delivering" },
            new SelectOrderStatus{OrderStatusId = 3, OrderStatusName="Incompleted" },
            new SelectOrderStatus{OrderStatusId = 4, OrderStatusName="Completed" }
        };
            return selectOrderStatuses;
        }
        
    }
}
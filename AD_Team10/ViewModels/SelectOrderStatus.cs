using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class SelectOrderStatus
    {
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }

        public List<SelectOrderStatus> GetSelectOrderList()
        {
            List<SelectOrderStatus> selectOrderStatuses = new List<SelectOrderStatus>
        {
            new SelectOrderStatus{OrderStatusId = 0, OrderStatusName="Status" },
            new SelectOrderStatus{OrderStatusId = 1, OrderStatusName="Pending" },
            new SelectOrderStatus{OrderStatusId = 2, OrderStatusName="Delivering" },
            new SelectOrderStatus{OrderStatusId = 3, OrderStatusName="Incomplete" },
            new SelectOrderStatus{OrderStatusId = 4, OrderStatusName="Completed" }
        };
            return selectOrderStatuses;
        }

    }
}
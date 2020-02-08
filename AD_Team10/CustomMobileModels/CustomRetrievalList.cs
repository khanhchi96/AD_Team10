using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.CustomMobileModels
{
    public class CustomRetrievalList
    {
        public int ItemID { get; set; }
        public CustomRetrievalListDetail[] RetrievalListDetails { get; set; }
    }
}
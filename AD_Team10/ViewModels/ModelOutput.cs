using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class ModelOutput
    {
        [ColumnName("PredictedQuantity")]
        public float PredictedQuantity { get; set; }
    }
}
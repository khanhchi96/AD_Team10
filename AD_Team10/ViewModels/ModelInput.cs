using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AD_Team10.ViewModels
{
    public class ModelInput
    {
        [ColumnName("HistoricalQuantity")]
        [VectorType(4)]
        public float[] HistoricalQuantity{ get; set; }
        [ColumnName("Label")]
        public float Quantity { get; set; }
    }
}
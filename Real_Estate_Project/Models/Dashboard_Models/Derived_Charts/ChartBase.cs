using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Dashboard_Models
{
    public abstract class ChartBase
    {
        public string Type { get; set; }

        public DataModel Data { get; set; }
    }
}
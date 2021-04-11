using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Dashboard_Models
{
    public class PieChart :ChartBase
    {
        public PieChart()
        {
            Type = "pie";
        }

        public object Options { get {

                return new
                {
                    Plugins = new
                    {
                        datalabels = new
                        {
                            display = true,
                            align = "bottom",
                            backgroundColor = "#ccc",
                            borderRadius = 6,
                            font = new
                            {
                                color = "red",
                                size = 12
                            }
                        }
                    }
                };
            } }
    }
}
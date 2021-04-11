using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models
{
    public static class CityAreas
    {
        public enum Area
        {
            NorthEnd,
            SouthEnd,
            EastEnd,
            WestEnd,
            CityCentral
        }

        public static Dictionary<string, Area> AreaMap = new Dictionary<string, Area>()
        {
            {"North End",Area.NorthEnd },
            {"South End",Area.SouthEnd },
            {"East End",Area.EastEnd },
            {"West End",Area.WestEnd },
            { "City Center",Area.CityCentral}
        };
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.DataAccess.Data_Models
{
    [NotMapped]
    public static class Provinces
    {
        public enum Province
        {
            NewBrunswick,
            BritishColumbia,
            Alberta,
            Manitoba,
            NewfoundlandandLabrador,
            NorthwestTerritories,
            NovaScotia,
            Nunavut,
            Ontario,
            PrinceEdwardIsland,
            Quebec,
            Saskatchewan,
            Yukon
        }

        public static Dictionary<string, Province> ProvinceMap = new Dictionary<string, Province>()
        {
            {"New Brunswick",Province.NewBrunswick },
            {"British Columbia",Province.BritishColumbia },
            {"Alberta",Province.Alberta },
            {"Manitoba",Province.Manitoba},
            {"New Foundland and labrador",Province.NewfoundlandandLabrador},
            {"Northwest Territories",Province.NorthwestTerritories },
            {"Nova Scotia",Province.NovaScotia},
            { "Nunavut",Province.Nunavut},
            { "Ontario",Province.Ontario},
            {"Prince Edward Island",Province.PrinceEdwardIsland},
            { "Quebec",Province.Quebec},
            {"Saskatchewan",Province.Saskatchewan},
            {"Yukon",Province.Yukon}
        };

    }
}
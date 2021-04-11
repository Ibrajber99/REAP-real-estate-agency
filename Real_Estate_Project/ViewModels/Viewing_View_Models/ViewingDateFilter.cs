using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels.Viewing_View_Models
{
    public class ViewingDateFilter
    {
        [Required]
        [Display(Name ="From")]
        public DateTime StartingDateRange { get; set; }

        [Required]
        [Display(Name ="To")]
        public DateTime EndingDateRange { get; set; }
    }
}
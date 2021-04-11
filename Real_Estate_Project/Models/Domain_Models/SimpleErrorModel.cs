using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Domain_Models
{
    [NotMapped]
    public class SimpleErrorModel
    {
        public string ErrorMessage { get; set; }
    }
}
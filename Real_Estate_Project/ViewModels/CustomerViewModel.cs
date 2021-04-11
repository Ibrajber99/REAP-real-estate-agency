using Real_Estate_Project.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels
{
    public class CustomerViewModel
    {
        public Customer inputModel { get; set; }

        public List<Customer> customerList { get; set; }

        public CustomerViewModel()
        {
            customerList = new List<Customer>();
            inputModel = new Customer();
        }

    }
}
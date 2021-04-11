using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Real_Estate_Project.Controllers
{
    public class RoleFilterController : Controller
    {
        private const string USER_OEPRATIONS_PATH = "UserOperations";
        private const string CUSTOMER_OPERATIONS_PATH = "CustomerOperations";
        private const string IDENTITY_OPERATIONS_PATH = "Account";
        private const string NOT_AUTHORIZED_PATH = "RoleNotAuthorized";
        private SimpleErrorModel errorModel;

        public RoleFilterController(SimpleErrorModel erroM)
        {
            errorModel = erroM;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: RoleFilter
        public ActionResult Index()
        {
            return View();
        }


        #region User related Operations
        //Only authorized user will be redirected
        public ActionResult GetUsers()
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.ADMIN)
                || UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);

            if (res)
            {
                return RedirectToAction("Index", USER_OEPRATIONS_PATH);
            }
            else
            {
                errorModel.ErrorMessage = "Agent cannot access users resource";
                //send to an error page for role is not authorized
                return View(NOT_AUTHORIZED_PATH, errorModel);
            }
        }

        public ActionResult CreateUser()
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);
            if (res)
            {
                return RedirectToAction("Register", IDENTITY_OPERATIONS_PATH);
            }
            else
            {
                errorModel.ErrorMessage = "Agent cannot access users resource";
                //send to an error page for role is not authorized
                return View(NOT_AUTHORIZED_PATH, errorModel);
            }
        }

        public ActionResult GetUserDetails(int ?id)
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);
            if (id.HasValue)
            {
                if (res)
                {
                    return RedirectToAction("Details", USER_OEPRATIONS_PATH, new {id= id});
                }
                else
                {
                    errorModel.ErrorMessage = "Agent cannot access users resource";
                    //send to an error page for role is not authorized
                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }
            }
            return HttpNotFound();
        }

        public ActionResult EditUser(int? id)
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);
            if (id.HasValue)
            {
                if (res)
                {
                    return RedirectToAction("Edit", USER_OEPRATIONS_PATH, new { id = id });
                }
                else
                {
                    errorModel.ErrorMessage = "Agent cannot access users resource";
                    //send to an error page for role is not authorized
                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }
            }
            return HttpNotFound();
        }
        #endregion

        #region Customer Related Operations
        public ActionResult GetCustomers()
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.AGENT)
                || UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);
            if (res)
            {
                return RedirectToAction("Index", CUSTOMER_OPERATIONS_PATH);
            }
            else
            {
                errorModel.ErrorMessage = "Admin cannot access customers resource";
                return View(NOT_AUTHORIZED_PATH, errorModel);
            }

        }

        public ActionResult CreateCustomer()
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.AGENT)
                || UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);
            if (res)
            {
                return RedirectToAction("Create", CUSTOMER_OPERATIONS_PATH);
            }
            else
            {
                errorModel.ErrorMessage = "Admin cannot access customers resource";
                return View(NOT_AUTHORIZED_PATH, errorModel);
            }
        }

        public ActionResult GetCustomerDetails(int? id)
        {
            var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.AGENT)
            || UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);
            if (id.HasValue)
            {
                if (res)
                {
                    return RedirectToAction("Details", CUSTOMER_OPERATIONS_PATH, new { id = id });
                }
                else
                {
                    errorModel.ErrorMessage = "Admin cannot access customers resource";
                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }
            }
            return HttpNotFound();
        }

        public ActionResult EditCustomer(int? id)
        {
            if (id.HasValue)
            {
                var res = UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.AGENT)
                || UserManager.IsInRole(User.Identity.GetUserId(), RoleNames.OFFICE_MANAGER);

                if (res)
                {
                    return RedirectToAction("Edit", CUSTOMER_OPERATIONS_PATH, new { id = id });
                }
                else
                {
                    errorModel.ErrorMessage = "Admin cannot access customers resource";
                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }
            }
            return HttpNotFound();
        }
        #endregion

    }
}
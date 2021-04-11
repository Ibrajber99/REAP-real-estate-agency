using Microsoft.AspNet.Identity;
using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Real_Estate_Project.Controllers
{
    [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
    public class CustomerOperationsController : Controller
    {
        private ICustomerRepo _customerRepo;
        private CustomerViewModel model;

        public CustomerOperationsController
            (ICustomerRepo customerRepo,
            CustomerViewModel customerVM )
        {
            _customerRepo = customerRepo;
            model = customerVM;
        }


        public async Task<ActionResult> Index()
        {
            try
            {
                var customers =await _customerRepo.GetAll();

                if (customers != null)
                    model.customerList = customers;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        public ActionResult Create()
        {
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer inputModel)
        {
            model.inputModel = inputModel;

            if (ModelState.IsValid)
            {
                try
                {
                    var currentLoggedUserId = User.Identity.GetUserId();
                    var currentLoggedUserRes = await UserIdentityManager
                                                .GetUserById(currentLoggedUserId);

                    inputModel.UserCreatorId = currentLoggedUserRes.registeredUser.ID;
                    inputModel.UserUpdatorId = currentLoggedUserRes.registeredUser.ID;
                    inputModel.DateCreated = DateTime.Now;
                    inputModel.DateUpdated = DateTime.Now;
                    inputModel.IsActive = true;

                    await _customerRepo.Create(inputModel);

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View("Create", model);
                }
                
            }
            return View("Create", model);
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var customer = await _customerRepo.Get(id.Value);

                    if (customer == null)
                        return HttpNotFound();

                    model.inputModel = customer;
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
              
                return View(model);
            }

            return HttpNotFound();
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var customer = await _customerRepo.Get(id.Value);

                    if (customer == null)
                        return HttpNotFound();

                    model.inputModel = customer;
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
              
                return View(model);
            }

            return HttpNotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerViewModel returnModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentLoggedUserId = User.Identity.GetUserId();

                    var currentLoggedUserRes = await UserIdentityManager
                        .GetUserById(currentLoggedUserId);

                    if (currentLoggedUserRes != null)
                    {

                        returnModel.inputModel.UserUpdatorId = 
                            currentLoggedUserRes.registeredUser.ID;

                        returnModel.inputModel.DateUpdated = DateTime.Now;

                        await _customerRepo.Update(returnModel.inputModel);

                        model.inputModel = returnModel.inputModel;


                        return View("Details", model);
                    }
                    else
                    {
                        return HttpNotFound();
                    }

                }
                catch (Exception ex)
                {
                    model.inputModel = returnModel.inputModel;

                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View("Edit", model);
                }

            }
            model.inputModel = returnModel.inputModel;

            return View("Edit", model);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var customer = await _customerRepo.Get(id.Value);

                    if (customer == null)
                        return HttpNotFound();

                    model.inputModel = customer;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(model);
            }
            return HttpNotFound();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var customer = await _customerRepo.Get(id);

                customer.IsActive = false;
                var res = await _customerRepo.Delete(customer);

                if(res != 1)
                {
                    ModelState.AddModelError(string.Empty,
                        "couldn't Archive Customer");

                    model.inputModel = customer;

                    return View(model);
                }

            }catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                model.inputModel = await _customerRepo.Get(id);

                return View(model);
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _customerRepo.Dispose();
            }
        }
    }
}
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.ViewModels;
using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Real_Estate_Project.Authorization_Utilities;
using System.Threading.Tasks;
using Real_Estate_Project.ViewModels.OperatingUser_ViewModels;
using Real_Estate_Project.View_Uitlity_Model_Helpers;
using PagedList;
using System.Linq;
using PagedList.EntityFramework;
using Real_Estate_Project.Models.Domain_Models;

namespace Real_Estate_Project.Controllers
{
    public class UserOperationsController : Controller
    {
        private SimpleErrorModel errorModel;
        private IOperatingUserRepo _userRepo;
        private OperatingUserViewModel model;
        private VerificationListViewModel verificationListModel;
        private IUserImageHelper imageHelper;
        private const string NOT_AUTHORIZED_PATH = "RoleNotAuthorized";



        public async Task<ActionResult> DisplayProfileImage(int id)
        {
            try
            {
                var imageToDisplay = await _userRepo.GetUserProfileImage(id);


                if(imageToDisplay == null)
                    return File(new byte[0], "");


                return File(imageToDisplay.Content, imageToDisplay.ContentType);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return File(new byte[0], "");
            }
        }

        public async Task<ActionResult> DisplayUserLicense(int id)
        {
            try
            {
                var imageToDisplay = await _userRepo.GetUserLatestLicesne(id);


                if (imageToDisplay == null)
                    return File(new byte[0], "");


                return File(imageToDisplay.Content, imageToDisplay.ContentType);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return File(new byte[0], "");
            }
        }

        public UserProfileViewModel GetDefaultProfileViewModel(OperatingUser user)
        {
            var model = new UserProfileViewModel
            {
                UserID = user.ID,
                UserName = user.UserName,
                FullName = user.FullName,
                UserProfileImage = user.ProfileImage,
                UserProfileDrivingLicense = user.CurrentDrivingLicense,
                IsVerified = user.IsVerified,
                DateOfBirth = user.DateOfBirth,
                StreetAddress = user.StreetAddress,
                Municipality = user.Municipality,
                Province = user.Province,
                PostalCode = user.PostalCode,
                CellPhone = user.CellPhone,
                IsTypeAgent = 
                user.RoleID == RoleNames.AGENT
            };

            return model;
        }

        public UserOperationsController(IOperatingUserRepo userRepo,
            OperatingUserViewModel userModel, 
            VerificationListViewModel verificationListModel,
            IUserImageHelper imageHelper)
        {
            _userRepo = userRepo;
            model = userModel;
            this.verificationListModel = verificationListModel;
            this.imageHelper = imageHelper;
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> Index(UserSearchViewModel searchModel,int page = 1)
        {
            try
            {
                var users = _userRepo.GetIqueryAbleUsers
                    (searchname: searchModel?.SearchQuery)
                    .OrderByDescending(u => u.ID);


                var pagedUsers = await users.ToPagedListAsync(page, 8);

                if (pagedUsers != null)
                    model.usersList = pagedUsers;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError
                    (string.Empty, ex.Message);
            }
            return View(model);
        }




        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var userFound = await _userRepo.Get(id.Value);

                   
                    if (userFound == null)
                        return HttpNotFound();

                    model.InputModel = userFound;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError
                     (string.Empty, ex.Message);

                }
                return View(model);
            }

            return HttpNotFound();
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var userFound = _userRepo.Get(id.Value);
                    var rolesList = UserIdentityManager.GetRoles();

                    await Task.WhenAll(userFound, rolesList);

                    if (userFound.Result == null)
                        return HttpNotFound();

                    if (rolesList.Result == null)
                        return HttpNotFound();

                    model.InputModel = userFound.Result;
                    model.rolesList = rolesList.Result;
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError
                    (string.Empty, ex.Message);

                }
                return View(model);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> Edit(OperatingUser inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentLoggedUserId = User.Identity.GetUserId();

                    var currentLoggedUserRes = await UserIdentityManager.GetUserById
                                               (currentLoggedUserId);


                    var userMappedRes = inputModel;
                    userMappedRes.UserUpdatorId = currentLoggedUserRes.registeredUser.ID;
                    userMappedRes.UpdatedDate = DateTime.Now;

                    var identityUser = await UserIdentityManager
                        .GetUserFromModelId(userMappedRes.ID);

                    if (identityUser != null)
                    {
                        var result = await UserIdentityManager.Changerole
                           (identityUser, userMappedRes.RoleID);

                        userMappedRes.IsActive = true;

                        if (userMappedRes.RoleID != RoleNames.AGENT)
                            userMappedRes.IsVerified = true;





                        var res = await _userRepo.Update(userMappedRes);

                        if(res > 0)
                        {
                            return RedirectToAction("Details",
                            new { id = userMappedRes.ID });
                        }
                        else
                        {
                            model.InputModel = inputModel;
                            model.rolesList = await UserIdentityManager.GetRoles();

                            return View("Edit", model);
                        }
                    }
                }
                catch (Exception ex)
                {
                    model.InputModel = inputModel;
                    model.rolesList = await UserIdentityManager.GetRoles();
                    ModelState.AddModelError(string.Empty, ex.Message);

                    return View("Edit", model);
                }
            }
            model.InputModel = inputModel;
            model.rolesList = await UserIdentityManager.GetRoles();

            return View("Edit", model);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var user = await _userRepo.Get(id.Value);

                    if (user == null)
                        return HttpNotFound();

                    model.InputModel = user;
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
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userToDelete = await _userRepo.Get(id);

                var identityUser = await UserIdentityManager.
                                GetUserFromModelId(userToDelete.ID);

                if (identityUser != null
                  && userToDelete != null)
                {
                    userToDelete.RoleID = RoleNames.ARCHIVED;

                    var result = await UserIdentityManager.Changerole
                         (identityUser, userToDelete.RoleID);

                    userToDelete.IsActive = false;
                    var res = await _userRepo.Delete(userToDelete);

                    if (res == 0)
                    {
                        var user = await _userRepo.Get(id);
                        if (user == null)
                            return HttpNotFound();

                        model.InputModel = user;

                        ModelState.AddModelError
                            (string.Empty, "Agent can't be deleted.");

                        return View("Delete", model);
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError
               (string.Empty, ex.Message);

                var user = await _userRepo.Get(id);

                model.InputModel = user;

                return View("Delete", model);
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> UserProfile()
        {
            try
            {
                var currentLoggedUserId = User.Identity.GetUserId();

                var currentLoggedUserRes = await UserIdentityManager.GetUserById
                                           (currentLoggedUserId);


                if (currentLoggedUserRes == null)
                    return HttpNotFound();


                var user = await _userRepo.Get
                    (currentLoggedUserRes.registeredUser.ID);

                if (user == null)
                    return HttpNotFound();

                var model = GetDefaultProfileViewModel(user);


                if (user.RoleID == RoleNames.AGENT)
                    model.IsTypeAgent = true;
                else
                    model.IsTypeAgent = false;


                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new UserProfileViewModel());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> UserProfile(UserProfileViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userFound = await _userRepo.Get(model.UserID);

                    if (userFound == null)
                        return HttpNotFound();

                    //If user Added a new Profile Image
                    if (model.ProfileImageFile != null)
                    {
                        var profileImage = model.ProfileImageFile;

                        if (imageHelper.isValidFileUpload(profileImage))
                        {
                            imageHelper.SetUserProfileImage
                                (profileImage, userFound);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty,
                                   "Please Provide a valid File type");

                            model = GetDefaultProfileViewModel(userFound);
                            return View(model);
                        }
                    }


                    //If user Added a new Driving License
                    if (model.ProfileDriverLicense != null)
                    {
                        var licenseFile = model.ProfileDriverLicense;

                        if (imageHelper.IsDrivingLicenseValid
                            (licenseFile,
                             model.LicenseExpiryDate,ModelState))
                        {
                            imageHelper.SetProfileDriverLicense
                             (licenseFile,
                             userFound, model.LicenseExpiryDate);
                        }
                        else
                        {
                            model = GetDefaultProfileViewModel(userFound);

                            return View(model);
                        }
                    }

                    var res = await _userRepo.SaveUserImages(userFound);

                    return RedirectToAction("UserProfile", new { id = userFound.ID });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," +RoleNames.AGENT)]
        public async Task<ActionResult> UserVerificationList
            (int? selectedUser,int ? page,string orderBy = null)
        {
            if (User.IsInRole(RoleNames.AGENT))
            {
                errorModel = new SimpleErrorModel 
                { ErrorMessage = "Agent cannot access verification resource." };

                return View(NOT_AUTHORIZED_PATH, errorModel);
            }

            try
            {
                verificationListModel.IsVerifiedSearchParam = orderBy == 
                    "IsVerified" ? "IsVerified_Desc" : "IsVerified";


                verificationListModel.CurrentSort = orderBy;

                var users = await GetSortedUsers(orderBy,page);

                if (users != null)
                    verificationListModel.usersList = users;

                if (selectedUser.HasValue)
                {
                    verificationListModel.CurrentViewingModel =
                        await _userRepo.Get(selectedUser.Value);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(verificationListModel);
        }


        public async Task<IPagedList<OperatingUser>> GetSortedUsers
            (string orderBy = null, int? page = 1)
        {
            var query = _userRepo.GetIqueryAbleUsers
                (includeOnlyAgents: true);
            
            switch (orderBy)
            {
                case "IsVerified":
                    query = query.OrderBy(i => i.IsVerified);
                    break;
                case "IsVerified_Desc":
                    query = query.OrderByDescending(i => i.IsVerified);
                    break;
                default:
                    query = query.OrderBy(i => i.ID);
                    break;
            }
            int pageNumber = page ?? 1;
            return await query.ToPagedListAsync(pageNumber, 4);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN)]
        public async Task<ActionResult> VerifyUser(int? userId)
        {
            try
            {
                if (userId.HasValue)
                {
                    var userFound = await _userRepo.Get(userId.Value);

                    var currentLoggedUserId = User.Identity.GetUserId();

                    var currentLoggedUserRes = await UserIdentityManager.GetUserById
                                               (currentLoggedUserId);

                    if (userFound == null)
                        return HttpNotFound();

                    userFound.IsVerified = true;

                    userFound.CurrentDrivingLicense.UserUpdatorId = 
                        currentLoggedUserRes.registeredUser.ID;

                    
                    var res = await _userRepo.Update(userFound);

                    return RedirectToAction("UserVerificationList",
                        new { selectedUser = userFound.ID });
                }

                return HttpNotFound();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);

                return View("UserVerificationList", 
                    new VerificationListViewModel());
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _userRepo.Dispose();
            }
        }
    }
}
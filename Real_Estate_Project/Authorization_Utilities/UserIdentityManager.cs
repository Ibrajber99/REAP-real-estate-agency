using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Real_Estate_Project.Authorization_Utilities
{
    public static class UserIdentityManager
    {
        public static void CreateDefaultRoles()
        {
            var context = ApplicationDbContext.Create();
            var roleManager = context.CreateRoleManager();
            roleManager.CreateRoleIfNotExists(RoleNames.ADMIN);
            roleManager.CreateRoleIfNotExists(RoleNames.OFFICE_MANAGER);
            roleManager.CreateRoleIfNotExists(RoleNames.AGENT);
            roleManager.CreateRoleIfNotExists(RoleNames.ARCHIVED);
        }

        public static void  CreateAssociatedListingEntries()
        {
            using (var context = new ApplicationDbContext())
            {
                var heatingTable = context.HeatingType.ToList();
                var featuresTable = context.ListingFeatures.ToList();

                if(heatingTable.Count <= 0)
                {
                    var heatingListToInsert = new List<Heating>()
                    {
                        new Heating{HeatingType="Electric"},
                        new Heating{HeatingType="Gas" },
                        new Heating{HeatingType="Heat Pump" },
                        new Heating{HeatingType ="Wood Furnace" },
                        new Heating{HeatingType="Geothermal" }
                    };

                    heatingListToInsert.ForEach(h => context.HeatingType.Add(h));
                }
                
                if(featuresTable.Count <= 0)
                {
                    var featuresListToInsert = new List<PropertyFeatures>()
                    {
                        new PropertyFeatures{FeatureName="Fireplace" },
                        new PropertyFeatures{FeatureName="Garage" },
                        new PropertyFeatures{FeatureName="Baby Barn" },
                        new PropertyFeatures{FeatureName="Central air" },
                        new PropertyFeatures{FeatureName="Screened porch" }
                    };

                    featuresListToInsert.ForEach(f => context.ListingFeatures.Add(f));
                }
                context.SaveChanges();

            }
        }

        public static void CreateRoleIfNotExists(this RoleManager<IdentityRole> roleManager, string RoleName)
        {
            if (!roleManager.RoleExists(RoleName))
            {
                var role = new IdentityRole { Name = RoleName };
                roleManager.Create(role);
            }
        }

        public static async Task<IdentityResult> AssignRole(ApplicationUser userToAssing, string roleToAssign)
        {
            IdentityResult assignRoleResult = new IdentityResult();
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                var roleManager = context.CreateRoleManager();

                if (roleManager.RoleExists(roleToAssign))
                {
                    assignRoleResult = await userManager.AddToRoleAsync(userToAssing.Id, roleToAssign);
                    return assignRoleResult;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return assignRoleResult;
        }

        public static async Task<IdentityResult> Changerole(ApplicationUser userToAssing, string roleToAssign)
        {
            IdentityResult assignRoleResult = new IdentityResult();
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                var roleManager = context.CreateRoleManager();

                await userManager.RemoveFromRolesAsync(userToAssing.Id, userManager.
                    GetRoles(userToAssing.Id).ToArray());


                if (roleManager.RoleExists(roleToAssign))
                {
                    assignRoleResult = await userManager.AddToRoleAsync(userToAssing.Id, roleToAssign);
                    await userManager.UpdateAsync(userToAssing);

                    return assignRoleResult;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return assignRoleResult;
        }

        public static async Task<List<IdentityResult>> CreateIdnetityUser
            (ApplicationUser userInfo,string userPassword,string loggedInUserId)
        {
            List<IdentityResult> resultList = new List<IdentityResult>();


            var context = ApplicationDbContext.Create();
            var userManager = context.CreateUserManager();

            var currentUser = await GetUserById(loggedInUserId);
            userInfo.registeredUser.UserUpdatorId = currentUser.registeredUser.ID;//Adding current user details
            userInfo.registeredUser.UserCreatorId = currentUser.registeredUser.ID;//Adding current user details
            userInfo.registeredUser.IsActive = true;

            if (userInfo.registeredUser.RoleID != RoleNames.AGENT)
                userInfo.registeredUser.IsVerified = true;

            var result = await userManager.CreateAsync(userInfo, userPassword);

            if (result.Succeeded)
            {
                var createdUser = await GetUserByEmail(userInfo.Email);
                if (createdUser != null)
                {
                    var userAffiliatedInfo = createdUser.registeredUser;                  
                    var assignRoleResult = AssignRole(createdUser, userAffiliatedInfo.RoleID);

                    await Task.WhenAll(assignRoleResult);
                    resultList.Add(assignRoleResult.Result);

                    return resultList;
                }
            }
            return resultList;
        }

        public static async Task<List<IdentityRole>> GetRoles()
        {
            try
            {
                var context = ApplicationDbContext.Create();
                var rolesList = context.Roles
                    .Where(r => !r.Name.Contains(RoleNames.ARCHIVED))
                    .ToListAsync();

                return await rolesList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<ApplicationUser> GetUserByEmail(string email)
        {
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                var currentUser = await userManager.FindByEmailAsync(email);

                return currentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<ApplicationUser> GetUserById(string id)
        {
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                var currentUser = await userManager.FindByIdAsync(id);

                return currentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static async Task<IdentityResult> AddInitialPassword(ApplicationUser user, string password)
        {
            IdentityResult passwrdChnageRes = new IdentityResult();
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                passwrdChnageRes =  await userManager.AddPasswordAsync(user.Id,password);

                return passwrdChnageRes;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return passwrdChnageRes;
        }

        public static async Task<IdentityResult> ChangeExisitngPassword(ApplicationUser user, string newPassword,string currentPassword)
        {
            IdentityResult passwrdChnageRes = new IdentityResult();
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                passwrdChnageRes = await userManager.ChangePasswordAsync(user.Id, currentPassword, newPassword);

                return passwrdChnageRes;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return passwrdChnageRes;
        }

        public static UserManager<ApplicationUser> CreateUserManager(this ApplicationDbContext context)
        {
            return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        public static RoleManager<IdentityRole> CreateRoleManager(this ApplicationDbContext context)
        {
            return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public static async Task<bool> CreateDefaultEntryIfNotExist()
        {
            try
            {
                var context = ApplicationDbContext.Create();
                var userManager = context.CreateUserManager();
                var findUser = await GetUserByEmail("Default@init.com");

                if (findUser == null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = "Default@init.com",
                        Email = "Default@init.com",
                        registeredUser = GetDefaultEntry()
                    };
                    var res = await userManager.CreateAsync(user, "123qweA!");

                    if (res.Succeeded)
                    {
                        var createdUser = await GetUserByEmail(user.Email);
                        if (createdUser != null)
                        {
                            var result = await AssignRole(createdUser,RoleNames.OFFICE_MANAGER);
                            if (result.Succeeded)
                            {
                                return true;
                            }
                        }
                    }

                } 
            }
            catch (Exception ex)
            {
                // do nothing because user already exists
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        private static OperatingUser GetDefaultEntry()
        {
            var defaultUser = new OperatingUser
            {
                UserName = "Default_Admin",
                SocialSecurityNumber = "120912786",
                FirstName = "Admin",
                LastName = "Admin",
                StreetAddress = "Default_Street",
                Municipality = "Default",
                Province = "New Brunswick",
                PostalCode = "E1A3O1",
                HomePhone = "46456656756",
                CellPhone = "5069871209",
                OfficePhoneNumber = "5069871209",
                DateOfBirth = new DateTime(1999, 05, 09, 00, 00, 00),
                RoleID = RoleNames.OFFICE_MANAGER,
                RegistrationDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsActive = true,
                IsVerified = true,
            };

            return defaultUser;
        }

        public static async Task<ApplicationUser> GetUserFromModelId(int id)
        {
            var context = ApplicationDbContext.Create();
            var user = await context.Users.FirstOrDefaultAsync
                (res => res.registeredUser.ID == id);

            return user;
        }
    }
}
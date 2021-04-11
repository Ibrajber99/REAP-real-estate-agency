using Real_Estate_Project.Business_services;
using Real_Estate_Project.Business_services.ViewingService;
using Real_Estate_Project.Controllers;
using Real_Estate_Project.DataAccess;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.DataAccess.Sql_Classes;
using Real_Estate_Project.Models.Dashboard_Models;
using Real_Estate_Project.Models.Dashboard_Models.ChartClient;
using Real_Estate_Project.View_Uitlity_Model_Helpers;
using System;
using System.Configuration;
using Unity;
using Unity.Injection;

namespace Real_Estate_Project
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.

            var baseAddress = ConfigurationManager.AppSettings["ChartClientBaseAddress"];
            var pathAddress = ConfigurationManager.AppSettings["ChartClientBasePath"];


            container.RegisterType<AccountController>(new InjectionConstructor());


            container.RegisterType<IDashboardRepo, SqlDashboard>();


            container.RegisterType<IBaseClient<ChartBase>, BaseClient<ChartBase>>
                (new InjectionConstructor(baseAddress,pathAddress));


            container.RegisterType<IOperatingUserRepo, SqlOperatingUsers>();


            container.RegisterType<ICustomerRepo, SqlCustomer>();


            container.RegisterType<IListingRepo, SqlListing>();


            container.RegisterType<IViewingRepo, SqlViewing>();


            container.RegisterType<IDashboardService, DashboardService>();


            container.RegisterType<IViewingService, ViewingService>();


            container.RegisterType<IUserImageHelper, UserImageHelper>();


            container.RegisterType<IListingImageHelper, ListingImageHelper>();
        }
    }
}
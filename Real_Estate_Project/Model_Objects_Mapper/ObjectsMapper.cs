using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project
{
    public static class ObjectsMapper
    {
        public static OperatingUser MapInputUserToModel(OperatingUserInputModel inputModel)
        {
            var userRes = new OperatingUser()
            {
                ID = inputModel.ID,
                UserName = inputModel.UserName,
                SocialSecurityNumber = inputModel.SocialSecurityNumber,
                FirstName = inputModel.FirstName,
                LastName = inputModel.LastName,
                MiddleName = inputModel.MiddleName,
                StreetAddress = inputModel.StreetAddress,
                Municipality = inputModel.Municipality,
                Province = inputModel.Province,
                PostalCode = inputModel.PostalCode,
                HomePhone = inputModel.HomePhone,
                CellPhone = inputModel.CellPhone,
                OfficePhoneNumber = inputModel.OfficePhoneNumber,
                DateOfBirth = inputModel.DateOfBirth,
                RoleID = inputModel.RoleID,
                RegistrationDate = inputModel.RegistrationDate,
                UpdatedDate = inputModel.UpdatedDate,
                UserCreatorId = inputModel.UserCreatorId,
                UserUpdatorId = inputModel.UserUpdatorId,
                IsVerified = inputModel.IsVerified
            };
            return userRes;
        }

        public static Listing MapHeatingAndFeaturesToListingModel(ListingViewModel model)
        {
            foreach (var heatType in model.HeatingListToSelect)
            {
                if (heatType.Selected)
                {
                    model.InputModel.Heating.Add(new Heating()
                    {
                        ID = Convert.ToInt32(heatType.Value),
                        HeatingType = heatType.Text
                    });
                }
            }

            foreach(var feature in model.FeaturesListToSelect)
            {
                if (feature.Selected)
                {
                    model.InputModel.Features.Add(new PropertyFeatures()
                    {
                        ID = Convert.ToInt32(feature.Value),
                        FeatureName = feature.Text
                    });
                }
            }



            return model.InputModel;
        }
    }
}
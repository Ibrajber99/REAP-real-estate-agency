using Real_Estate_Project.DataAccess.Data_Models;
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
                OfficeEmail = inputModel.OfficeEmail,
                OfficePhoneNumber = inputModel.OfficePhoneNumber,
                DateOfBirth = inputModel.DateOfBirth,
                RoleID = inputModel.RoleID,
                RegistrationDate = inputModel.RegistrationDate,
                UpdatedDate = inputModel.UpdatedDate,
                UserCreatorId = inputModel.UserCreatorId,
                UserUpdatorId = inputModel.UserUpdatorId
            };
            return userRes;
        }
    }
}
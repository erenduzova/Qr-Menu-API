using System;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.DTOs.Converter
{
	public class CompanyConverter
	{
        private readonly StateConverter _stateConverter;

        public CompanyConverter(StateConverter stateConverter)
        {
            _stateConverter = stateConverter;
        }

		public Company Convert(CompanyCreate companyCreate)
		{
            Company company = new()
            {
                Name = companyCreate.Name,
                PostalCode = companyCreate.PostalCode,
                AddressDetails = companyCreate.AddressDetails,
                Phone = companyCreate.Phone,
                EMail = companyCreate.EMail,
                RegisterDate = DateTime.Now,
                TaxNumber = companyCreate.TaxNumber,
                WebAddress = companyCreate.WebAddress,
                ParentCompanyId = companyCreate.ParentCompanyId,
                StateId = (byte)1,
                Restaurants = new List<Restaurant>()
            };

            if (company.ParentCompanyId == 0)
            {
                company.ParentCompanyId = null;
            }

            return company;
        }

        public CompanyResponse Convert(Company company)
        {
            CompanyResponse companyResponse = new()
            {
                Id = company.Id,
                Name = company.Name,
                PostalCode = company.PostalCode,
                AddressDetails = company.AddressDetails,
                Phone = company.Phone,
                EMail = company.EMail,
                RegisterDate = company.RegisterDate,
                TaxNumber = company.TaxNumber,
                WebAddress = company.WebAddress,
                ParentCompanyId = company.ParentCompanyId,
                StateResponse = _stateConverter.Convert(company.State!),
                RestaurantIds = company.Restaurants?.Select(r => r.Id).ToList()
            };

            return companyResponse;
        }

        public List<CompanyResponse> Convert(List<Company> companies)
        {
            List<CompanyResponse> companyResponses = new List<CompanyResponse>();
            foreach (Company company in companies)
            {
                companyResponses.Add(Convert(company));
            }
            return companyResponses;
        }

        public Company Convert(Company existingCompany, CompanyCreate updatedCompany)
        {
            existingCompany.Name = updatedCompany.Name;
            existingCompany.PostalCode = updatedCompany.PostalCode;
            existingCompany.AddressDetails = updatedCompany.AddressDetails;
            existingCompany.Phone = updatedCompany.Phone;
            existingCompany.EMail = updatedCompany.EMail;
            existingCompany.TaxNumber = updatedCompany.TaxNumber;
            existingCompany.WebAddress = updatedCompany.WebAddress;
            return existingCompany;
        }
    }
}


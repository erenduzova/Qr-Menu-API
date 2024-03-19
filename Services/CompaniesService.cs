using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using System.ComponentModel.Design;

namespace Qr_Menu_API.Services
{
    public class CompaniesService
    {
        private readonly ApplicationContext _context;
        private readonly RestaurantsService _restaurantService;

        public CompaniesService(ApplicationContext context, RestaurantsService restaurantService)
        {
            _context = context;
            _restaurantService = restaurantService;
        }

        public CompanyResponse GetCompanyResponse(Company company)
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
                StateResponse = new StateResponse
                {
                    Id = company.StateId,
                    Name = _context.States!.Find(company.StateId)!.Name
                },
                RestaurantIds = company.Restaurants?.Select(r => r.Id).ToList()
            };
            return companyResponse;
        }
        public List<CompanyResponse> GetCompaniesResponses()
        {
            List<Company> companies = _context.Companies!.Include(c => c.Restaurants).ToList();
            List<CompanyResponse> companyResponses = new();
            companies.ForEach(company =>
            {
                companyResponses.Add(GetCompanyResponse(company));
            });
            return companyResponses;
        }


        public int CreateCompany(CompanyCreate companyCreate)
        {
            Company newCompany = new()
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

            _context.Companies!.Add(newCompany);
            _context.SaveChanges();

            return newCompany.Id;
        }

        public CompanyResponse UpdateCompany(Company existingCompany, CompanyCreate updatedCompany)
        {
            existingCompany.Name = updatedCompany.Name;
            existingCompany.PostalCode = updatedCompany.PostalCode;
            existingCompany.AddressDetails = updatedCompany.AddressDetails;
            existingCompany.Phone = updatedCompany.Phone;
            existingCompany.EMail = updatedCompany.EMail;
            existingCompany.TaxNumber = updatedCompany.TaxNumber;
            existingCompany.WebAddress = updatedCompany.WebAddress;

            _context.Update(existingCompany);
            _context.SaveChanges();
            return GetCompanyResponse(existingCompany);
        }

        public void DeleteCompanyAndRelatedEntities(Company company)
        {
            company.StateId = 0;
            _context.Companies!.Update(company);
            var restaurants = company.Restaurants;
            if (restaurants != null)
            {
                foreach (var restaurant in restaurants)
                {
                    _restaurantService.DeleteRestaurantAndRelatedEntities(restaurant);
                }
            }
            _context.SaveChanges();
        }

    }
}

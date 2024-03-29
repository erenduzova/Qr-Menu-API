﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.Converter;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Qr_Menu_API.Services
{
    public class CompaniesService
    {
        private readonly ApplicationContext _context;
        private readonly RestaurantsService _restaurantService;
        private readonly UsersService _userService;
        private readonly CompanyConverter _companyConverter;

        public CompaniesService(ApplicationContext context, RestaurantsService restaurantService, UsersService usersService, CompanyConverter companyConverter)
        {
            _context = context;
            _restaurantService = restaurantService;
            _userService = usersService;
            _companyConverter = companyConverter;
        }

        private Company GetCompany(int companyId)
        {
            return _context.Companies!
                .Include(c => c.State)
                .First(c => c.Id == companyId);
        }

        private List<Company> GetCompanies()
        {
            return _context.Companies!
                .Include(c => c.State)
                .ToList();
        }

        private Company GetCompanyWithRestaurants(int companyId)
        {
            return _context.Companies!
                .Include(c => c.Restaurants)
                .Include(c => c.State)
                .First(c => c.Id == companyId);
        }

        private List<Company> GetCompaniesWithRestaurants()
        {
            return _context.Companies!
                .Include(c => c.Restaurants)
                .Include(c => c.State)
                .ToList();
        }

        private Company GetCompanyWithRestaurantsAndUsers(int companyId)
        {
            return _context.Companies!
                .Include(c => c.Restaurants)
                .Include(c => c.State)
                .Include(c => c.Users)
                .First(c => c.Id == companyId);
        }

        private List<Company> GetCompaniesWithRestaurantsAndUsers()
        {
            return _context.Companies!
                .Include(c => c.Restaurants)
                .Include(c => c.State)
                .Include(c => c.Users)
                .ToList();
        }

        private bool ParentCompanyExists(int companyId)
        {
            return _context.Companies!
                .Any(c => c.Id == companyId && c.StateId != 0);
        }

        public CompanyResponse GetCompanyResponse(int companyId)
        {
            Company foundCompany = GetCompanyWithRestaurants(companyId);
            return _companyConverter.Convert(foundCompany);
        }


        public List<CompanyResponse> GetCompaniesResponses()
        {
            List<Company> companies = GetCompaniesWithRestaurants();
            return _companyConverter.Convert(companies);
        }


        public int CreateCompany(CompanyCreate companyCreate)
        {
            var parentCompanyId = companyCreate.ParentCompanyId;
            if (parentCompanyId != null && !ParentCompanyExists((int)companyCreate.ParentCompanyId!))
            {
                return -1;
            }
            Company newCompany = _companyConverter.Convert(companyCreate);
            _context.Companies!.Add(newCompany);
            _context.SaveChanges();
            return newCompany.Id;
        }

        public CompanyResponse UpdateCompany(int companyId, CompanyCreate updatedCompany)
        {
            Company existingCompany = GetCompanyWithRestaurants(companyId);
            existingCompany = _companyConverter.Convert(existingCompany, updatedCompany);
            _context.Update(existingCompany);
            _context.SaveChanges();
            return _companyConverter.Convert(existingCompany);
        }

        public void DeleteCompanyAndRelatedEntities(Company company)
        {
            company.StateId = 0;
            _context.Companies!.Update(company);

            ICollection<Restaurant>? restaurants = company.Restaurants;
            if (restaurants != null)
            {
                foreach (Restaurant restaurant in restaurants)
                {
                    _restaurantService.DeleteRestaurantAndRelatedEntities(restaurant);
                }
            }

            ICollection<ApplicationUser>? users = company.Users;
            if (users != null)
            {
                foreach( ApplicationUser user in users)
                {
                    _userService.DeleteApplicationUserAndRelatedEntities(user);
                }
            }

            // Delete Sub Companies
            ICollection<Company>? subCompanies = _context.Companies
                .Where(c => c.ParentCompanyId == company.Id)
                .ToList();
            foreach (Company subCompany in subCompanies)
            {
                DeleteCompanyAndRelatedEntities(subCompany);
            }

            _context.SaveChanges();
        }

        public void DeleteCompanyAndRelatedEntitiesById(int companyId)
        {
            Company company = GetCompanyWithRestaurantsAndUsers(companyId);
            DeleteCompanyAndRelatedEntities(company);
        }
    }
}

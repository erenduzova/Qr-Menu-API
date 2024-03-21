﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Data;
using Qr_Menu_API.DTOs.CreateDTOs;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using Qr_Menu_API.Services;

namespace Qr_Menu_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly CompaniesService _companiesService;

        public CompaniesController(ApplicationContext context, CompaniesService companiesService)
        {
            _context = context;
            _companiesService = companiesService;
        }

        private bool CompaniesIsNull()
        {
            return _context.Companies == null;
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies!
                .Any(c => c.Id == id);
        }

        // GET: api/Companies
        [HttpGet]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<IEnumerable<CompanyResponse>> GetCompanies()
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            return _companiesService.GetCompaniesResponses();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<CompanyResponse> GetCompany(int companyId)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (!CompanyExists(companyId))
            {
                return NotFound("Company not found with this id: " + companyId);
            }
            return _companiesService.GetCompanyResponse(companyId);
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<CompanyResponse> PutCompany(int companyId, CompanyCreate updatedCompany)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (!CompanyExists(companyId))
            {
                return NotFound("Company not found with this id: " + companyId);
            }
            return _companiesService.UpdateCompany(companyId, updatedCompany);
        }

        // POST: api/Companies
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<CompanyResponse> PostCompany(CompanyCreate companyCreate)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            return _companiesService.CreateCompany(companyCreate);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCompany(int companyId)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (!CompanyExists(companyId))
            {
                return NotFound("Company not found with this id: " + companyId);
            }
            _companiesService.DeleteCompanyAndRelatedEntities(companyId);
            return Ok();
        }

    }
}

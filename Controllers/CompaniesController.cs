using System;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        [Authorize]
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
        [Authorize]
        public ActionResult<CompanyResponse> GetCompany(int id)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (!CompanyExists(id))
            {
                return NotFound("Company not found with this id: " + id);
            }
            return _companiesService.GetCompanyResponse(id);
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        [Authorize(Roles = "CompanyAdministrator")]
        [Authorize(Policy = "CompanyAdministrator")]
        public ActionResult<CompanyResponse> PutCompany(int id, CompanyCreate updatedCompany)
        {
            if (User.HasClaim("CompanyId", id.ToString()) == false)
            {
                return Unauthorized();
            }
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (!CompanyExists(id))
            {
                return NotFound("Company not found with this id: " + id);
            }
            return _companiesService.UpdateCompany(id, updatedCompany);
        }

        // POST: api/Companies
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<int> PostCompany(CompanyCreate companyCreate)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            int newCompanyId = _companiesService.CreateCompany(companyCreate);
            if (newCompanyId == -1)
            {
                return BadRequest("Invalid parentCompanyId provided.");
            }
            return Ok(newCompanyId);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCompany(int id)
        {
            if (CompaniesIsNull())
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (!CompanyExists(id))
            {
                return NotFound("Company not found with this id: " + id);
            }
            _companiesService.DeleteCompanyAndRelatedEntitiesById(id);
            return Ok();
        }

    }
}

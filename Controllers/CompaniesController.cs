using System;
using System.Collections.Generic;
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

        // GET: api/Companies
        [HttpGet]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<IEnumerable<CompanyResponse>> GetCompanies()
        {
            if (_context.Companies == null)
            {
                return NotFound();
            }
            return _companiesService.GetCompaniesResponses();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<CompanyResponse> GetCompany(int id)
        {
            if (_context.Companies == null)
            {
                return NotFound();
            }
            var company = _context.Companies.Include(c => c.Restaurants).FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return _companiesService.GetCompanyResponse(company);
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,CompanyAdministrator")]
        public ActionResult<CompanyResponse> PutCompany(int id, CompanyCreate updatedCompany)
        {
            var existingCompany = _context.Companies.Include(c => c.Restaurants).FirstOrDefault(c => c.Id == id);
            if (existingCompany == null)
            {
                return BadRequest();
            }
            return _companiesService.UpdateCompany(existingCompany, updatedCompany);
        }

        // POST: api/Companies
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<int> PostCompany(CompanyCreate companyCreate)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'ApplicationContext.Companies'  is null.");
            }
            if (_context.States == null)
            {
                return Problem("Entity set 'ApplicationContext.States'  is null.");
            }
            return _companiesService.CreateCompany(companyCreate);
        }
        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCompany(int id)
        {
            var company = _context.Companies.Include(c => c.Restaurants).FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            _companiesService.DeleteCompanyAndRelatedEntities(company);

            //IQueryable<ApplicationUser> applicationUsers = _context.Users.Where(u => u.CompanyId == id);
            //UsersService usersService = new UsersService(_context);
            //foreach (ApplicationUser applicationUser in applicationUsers)
            //{
            //    usersService.DeleteUserAndRelatedEntities(applicationUser.Id);
            //}
            //_context.SaveChanges();

            return Ok();

        }

    }
}

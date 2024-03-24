using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.Data
{
    public class DbInitializer
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationContext applicationContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public void Initialize()
        {
            if (_applicationContext != null)
            {
                _applicationContext.Database.Migrate();
                if (!_applicationContext.States!.Any())
                {
                    State stateDeleted = new State(0, "Deleted");
                    _applicationContext.States!.Add(stateDeleted);
                    State stateActive = new State(1, "Active");
                    _applicationContext.States.Add(stateActive);
                    State statePassive = new State(2, "Passive");
                    _applicationContext.States.Add(statePassive);
                }
                _applicationContext.SaveChanges();
                if (!_applicationContext.Companies!.Any())
                {
                    Company company = new()
                    {
                        Name = "TAB Gıda",
                        PostalCode = "34349",
                        AddressDetails = "Dikilitaş, Emirhan Cd. No:109, 34349 Beşiktaş/İstanbul",
                        Phone = "02123106600",
                        EMail = "tabgıda@gmail.com",
                        RegisterDate = DateTime.Today,
                        TaxNumber = "8150037902",
                        WebAddress = "www.tabgida.com.tr",
                        ParentCompanyId = null,
                        StateId = (byte)1
                    };
                    _applicationContext.Companies!.Add(company);

                    _applicationContext.SaveChanges();
                    if (!_roleManager.Roles.Any())
                    {
                        IdentityRole administratorRole = new IdentityRole("Administrator");
                        _roleManager.CreateAsync(administratorRole).Wait();
                        IdentityRole companyAdministratorRole = new IdentityRole("CompanyAdministrator");
                        _roleManager.CreateAsync(companyAdministratorRole).Wait();
                        IdentityRole restaurantAdministratorRole = new IdentityRole("RestaurantAdministrator");
                        _roleManager.CreateAsync(restaurantAdministratorRole).Wait();
                    }

                    if (!_userManager.Users.Any())
                    {
                        if (company != null)
                        {
                            ApplicationUser applicationUser = new()
                            {
                                UserName = "Administrator",
                                Name = "TabGıdaAdmin",
                                Email = "tabgidaadmin@gmail.com",
                                PhoneNumber = "11111111111",
                                RegisterDate = DateTime.Today,
                                CompanyId = company.Id,
                                StateId = (byte)1
                            };

                            _userManager.CreateAsync(applicationUser, "Admin123!").Wait();
                            _userManager.AddToRoleAsync(applicationUser, "Administrator").Wait();
                            Claim claim = new Claim("CompanyId", company.Id.ToString());
                            _userManager.AddClaimAsync(applicationUser, claim).Wait();
                            _userManager.AddToRoleAsync(applicationUser, "CompanyAdministrator").Wait();
                        }


                    }
                }
            }
        }
    }
}


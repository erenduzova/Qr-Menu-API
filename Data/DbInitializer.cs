using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.Models;

namespace Qr_Menu_API.Data
{
    public class DbInitializer
    {
        private readonly ApplicationContext _applicationContext;
        public DbInitializer(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
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

            }
        }
    }
}


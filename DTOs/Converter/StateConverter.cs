using System;
using Microsoft.EntityFrameworkCore;
using Qr_Menu_API.DTOs.ResponseDTOs;
using Qr_Menu_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Qr_Menu_API.DTOs.Converter
{
	public class StateConverter
    {
		public static StateResponse Convert(State state)
        {
            StateResponse stateResponse = new()
            {
                Id = state.Id,
                Name = state.Name
            };
            return stateResponse;
		}
	}
}


using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
	public class RestaurantUserResponse
	{
        public int RestaurantId { get; set; }
        public string UserId { get; set; } = "";
    }
}


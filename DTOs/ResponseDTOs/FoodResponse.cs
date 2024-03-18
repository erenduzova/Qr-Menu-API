using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
	public class FoodResponse
	{
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public float Price { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public StateResponse? StateResponse { get; set; }
    
	}
}


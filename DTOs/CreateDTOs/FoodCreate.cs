using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qr_Menu_API.DTOs.CreateDTOs
{
	public class FoodCreate
	{
        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = "";

        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        public int CategoryId { get; set; }
    }
}


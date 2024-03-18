using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qr_Menu_API.DTOs.CreateDTOs
{
	public class ApplicationUserCreate
	{
        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        public string? UserName { get; set; } = "";

        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; } = "";

        [EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        [Column(TypeName = "varchar(100)")]
        public string? Email { get; set; } = "";

        [Phone]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string? PhoneNumber { get; set; } = "";

        public int CompanyId { get; set; }

    }
}


using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qr_Menu_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
	public class ApplicationUserResponse
	{
        public string Id { get; set; } = "";

        public string? UserName { get; set; } = "";

        public string? Name { get; set; } = "";

        public string? Email { get; set; } = "";

        public string? PhoneNumber { get; set; } = "";

        public DateTime RegisterDate { get; set; }

        public int CompanyId { get; set; }

        public StateResponse? StateResponse { get; set; }
    }
}


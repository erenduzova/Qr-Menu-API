using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qr_Menu_API.DTOs.ResponseDTOs
{
    public class StateResponse
    {
        public byte Id { get; set; }
        public string Name { get; set; } = "";
    }
}

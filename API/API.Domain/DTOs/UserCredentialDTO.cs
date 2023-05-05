using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserCredentialDTO
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}

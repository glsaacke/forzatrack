using System.ComponentModel.DataAnnotations;

namespace api.core.controllers.models
{
    public class UserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        public int Deleted { get; set; } = 0;
    }
}
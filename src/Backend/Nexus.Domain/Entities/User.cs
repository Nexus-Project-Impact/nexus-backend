using Microsoft.AspNetCore.Identity;


namespace Nexus.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? CPF { get; set; } 
    }
}

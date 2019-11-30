using Microsoft.AspNetCore.Identity;

namespace AuthenticationManager.Persisted.Entities
{
    public class AuthUser : IdentityUser
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }

}

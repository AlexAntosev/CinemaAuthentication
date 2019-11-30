using AuthenticationManager.Persisted.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AuthenticationManager.Business.Interfaces
{
    public interface IUserService
    {
        Task<AuthUser> SignInAsync(string username, string password);
        Task<AuthUser> SignUpAsync(string username, string email, string password);
        Task<IdentityResult> GiveAdminRole(string userId);
        Task SignOutAsync();
        Task<IdentityResult> CreateRole(string name);
    }

}

using AuthenticationManager.Business.Exceptions;
using AuthenticationManager.Business.Interfaces;
using AuthenticationManager.Persisted.Entities;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManager.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;

        public UserService(
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSettings = options.Value;
        }

        public async Task<AuthUser> SignInAsync(string username, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (signInResult != SignInResult.Success)
            {
                throw new HttpBadRequestException("Wrong email or/and password.");
            }

            var authUser = _userManager.Users.FirstOrDefault(u => u.UserName == username);

            authUser.Token = await CreateJWT(authUser);

            return authUser;
        }

        public async Task<AuthUser> SignUpAsync(string username, string email, string password)
        {
            AuthUser authUser = new AuthUser() { UserName = username, Email = email };

            var signUpResult = await _userManager.CreateAsync(authUser, password);

            if (!signUpResult.Succeeded)
            {
                throw new HttpBadRequestException("Cannot create user with current email or password.");
            }

            var roleResult = await _userManager.AddToRoleAsync(authUser, Role.MANAGER);

            if (!roleResult.Succeeded)
            {
                throw new HttpBadRequestException("Cannot create user with default role");
            }

            authUser.Token = await CreateJWT(authUser);
            authUser.Role = Role.MANAGER;

            return authUser;
        }

        public async Task<IdentityResult> GiveAdminRole(string userId)
        {
            AuthUser user = await _userManager.FindByIdAsync(userId);

            Ensure.That(user, nameof(user), opt => opt.WithException(new HttpNotFoundException($"User with id {userId} is not exist."))).IsNotNull();

            return await _userManager.AddToRoleAsync(user, Role.ADMIN);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateRole(string name)
        {
            IdentityResult roleResult = IdentityResult.Failed();
            var role = await _roleManager.RoleExistsAsync(name);
            if (!role)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
            }

            return roleResult;
        }

        private async Task<string> CreateJWT(AuthUser authUser)
        {
            string audience = _appSettings.Audience;
            string issuer = _appSettings.Issuer;
            string key = _appSettings.Secret;

            byte[] bytes = Encoding.UTF8.GetBytes(key);
            var secret = Convert.ToBase64String(bytes);

            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var issuedAt = DateTime.Now.ToUniversalTime();
            var expiresAt = issuedAt.AddMinutes(30);

            IList<Claim> claims = await _userManager.GetClaimsAsync(authUser);
            IList<string> roles = await _userManager.GetRolesAsync(authUser);

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, authUser.Id));

            var token = new JwtSecurityToken(issuer,
                audience,
                claims,
                issuedAt,
                expiresAt,
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

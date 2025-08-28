using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Prj.TaskManager.Data;
using Prj.TaskManager.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Prj.TaskManager.Sevice
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        //user lai authenticate garna (tesma bool return garna pareo)

        public async Task<UserModel> Register(UserModel model, string password)
        {
            model.PasswordHash = HashPassword(password);
            _context.Users.Add(model);
            await _context.SaveChangesAsync(); // awaits waits foe result and proceed to next line

            return model;
        }


        public async Task<bool> AuthenticateUser(HttpContext httpContext, string userName, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if(user == null)
            {
                return false;
            }
            //yo vitra password verify garna pareo
            else if (!VerifyPassword(password, user.PasswordHash))
            {
                return false;
            }
            //yo mathi ko gari sake  paxi authenticcate vayo vanne bujhxau 

            //aba sign in process

            //roles defined and store

            //maile roles rakhna ko lagi i have to create claims(i claimthat role)
            ////tyo user sana vako sabai cheese claim garnu parx

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Role),
                //new Claim(ClaimTypes.Role, "user")

            };
            // yo chai claim property ma collection banaya ra add gare

            //yo tala ko chai session ma store garne code lekhako

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //persistant cookie/login

            var authProperties = new AuthenticationProperties() { IsPersistent = true };


            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity),
                authProperties
                );


            return true;
        }
        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        //method for verify password vanera method banako
        private bool VerifyPassword(string inputPassword, string PasswordHash)
        {
            return HashPassword(inputPassword) == PasswordHash;     //input psw === hunu pare db ma store gareko psw sanaga
        }
        //hash pasw ma convert garna incretion ko code hun
        
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);

        }

        public async Task<bool> ConfirmEmail(string token)
        {
            var user = _context.Users.FirstOrDefault(x => x.EmailConfirmationToken == token);
            if (user == null)
            {
                return false;
            }
            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = "";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

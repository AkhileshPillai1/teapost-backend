using Microsoft.AspNetCore.Mvc;

namespace TeaPost.Interfaces
{
    public interface IUserService
    {
        public IActionResult Login(string username, string password);
    }
}

using Microsoft.EntityFrameworkCore;
using CRUDMVC.Models;

namespace CRUDMVC.Services.Contract
{
    public interface IUserService
    {
        Task<User> GetUser(string username, string password);
        Task<User> SaveUser(User modelo);
        Task<User> UpdateUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> SavePasswordResetToken(User user, string token);
        Task<User> MarkPasswordResetTokenAsUsed(string token);
        Task<User> GetUserByPasswordResetToken(string token);

    }
}

using Microsoft.EntityFrameworkCore;
using CRUDMVC.Models;
using CRUDMVC.Services.Contract;
using CRUDMVC.Resources;

namespace CRUDMVC.Services.Implementation
{
    public class UserService : IUserService
    {

        private readonly CrudmvcContext _mvcContext;
        public UserService(CrudmvcContext crudmvcContext)
        {
            _mvcContext = crudmvcContext;
        }
        public async Task<User> GetUser(string email, string password)
        {
            User user_founded = await _mvcContext.Users.Where(u => u.Email == email && u.Password == password)
                .FirstOrDefaultAsync();

            return user_founded;

        }

        public async Task<User> SaveUser(User modelo)
        {
            _mvcContext.Users.Add(modelo);
            await _mvcContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<User> UpdateUser(User user)
        {
            _mvcContext.Update(user);
            await _mvcContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _mvcContext.Users.Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<User> SavePasswordResetToken(User user, string token)
        {
            // Aquí debes implementar la lógica para guardar el token en la base de datos
            // Esto dependerá de cómo hayas modelado tu base de datos
            // Por ejemplo, podrías tener una tabla de tokens de restablecimiento de contraseña
            // donde cada fila tenga una referencia al usuario, el token y la fecha de vencimiento

            // Ejemplo:
            PasswordResetToken passwordResetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(1) // El token expira en 1 hora
            };

            _mvcContext.PasswordResetToken.Add(passwordResetToken);
            await _mvcContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> MarkPasswordResetTokenAsUsed(string token)
        {
            // Aquí debes implementar la lógica para marcar el token como utilizado
            // Esto dependerá de cómo hayas modelado tu base de datos
            // Por ejemplo, podrías tener una columna en la tabla de tokens de restablecimiento de contraseña
            // que indique si el token ha sido utilizado o no

            // Ejemplo:
            PasswordResetToken passwordResetToken = await _mvcContext.PasswordResetToken
                .Where(t => t.Token == token)
                .FirstOrDefaultAsync();

            if (passwordResetToken != null)
            {
                passwordResetToken.Used = true;
                _mvcContext.Update(passwordResetToken);
                await _mvcContext.SaveChangesAsync();
            }

            return passwordResetToken.User;
        }

        public async Task<User> GetUserByPasswordResetToken(string token)
        {
            // Aquí debes implementar la lógica para buscar el usuario por el token de restablecimiento de contraseña
            // Esto dependerá de cómo hayas modelado tu base de datos
            // Por ejemplo, podrías buscar el token en la tabla de tokens de restablecimiento de contraseña
            // y luego obtener el usuario asociado a ese token

            // Ejemplo:
            PasswordResetToken passwordResetToken = await _mvcContext.PasswordResetToken
                .Include(t => t.User)
                .Where(t => t.Token == token && !t.Used && t.ExpiryDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            return passwordResetToken?.User;
        }

        public Task<User> SavePasswordResetToken(string username, string token)
        {
            throw new NotImplementedException();
        }
    }
}

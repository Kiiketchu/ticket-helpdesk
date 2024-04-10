using Microsoft.AspNetCore.Mvc;
using CRUDMVC.Models;
using CRUDMVC.Resources;
using CRUDMVC.Services.Contract;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVC.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public InicioController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(User modelo)
        {
            modelo.Password = Utilities.EncriptarClave(modelo.Password);

            modelo.RoleId = 4;

            modelo.CreatedAt = System.DateTime.Now;

            User user_created = await _userService.SaveUser(modelo);

            if (user_created.Id > 0)
                return RedirectToAction("IniciarSesion", "Inicio");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string email, string password)
        {
            User user_founded = await _userService.GetUser(email, Utilities.EncriptarClave(password));

            if (user_founded == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>(){
                new Claim(ClaimTypes.Email, user_founded.Email),
                new Claim("name", user_founded.Name),
                new Claim("lastname", user_founded.Lastname),
                new Claim(ClaimTypes.Role, user_founded.RoleId.ToString()),
                new Claim("ProjectId", user_founded.ProjectId.ToString())

            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RecuperarPassword()
        {
            return View();
        }

        // Acción para procesar la solicitud de recuperación de contraseña
        [HttpPost]
        public async Task<IActionResult> RecuperarPassword(string email)
        {
            // Buscar el usuario con el correo electrónico proporcionado
            User user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                ViewData["Mensaje"] = "No se encontró un usuario con ese correo electrónico";
                return View();
            }

            // Generar un token único
            string token = Guid.NewGuid().ToString();

            // Guardar el token en la base de datos
            await _userService.SavePasswordResetToken(user, token);

            // Enviar el correo electrónico con el enlace para restablecer la contraseña
            string resetLink = Url.Action("RestablecerPassword", "Inicio", new { token = token }, Request.Scheme);
            await _emailService.SendPasswordResetEmail(user.Email, resetLink);

            return RedirectToAction("IniciarSesion");
        }

        // Acción para mostrar la vista de restablecimiento de contraseña
        public IActionResult RestablecerPassword(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        // Acción para procesar el restablecimiento de la contraseña
        [HttpPost]
        public async Task<IActionResult> RestablecerPassword(string token, string newPassword)
        {
            // Buscar el token en la base de datos
            User user = await _userService.GetUserByPasswordResetToken(token);
            if (user == null)
            {
                ViewData["Mensaje"] = "El token de restablecimiento de contraseña es inválido o ha expirado";
                return View();
            }

            // Restablecer la contraseña del usuario
            user.Password = Utilities.EncriptarClave(newPassword);
            await _userService.UpdateUser(user);

            // Marcar el token como utilizado
            await _userService.MarkPasswordResetTokenAsUsed(token);

            return RedirectToAction("IniciarSesion");
        }


    }
}


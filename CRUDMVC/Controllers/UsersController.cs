using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDMVC.Models;
using CRUDMVC.Resources;

namespace CRUDMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly CrudmvcContext _context;

        public UsersController(CrudmvcContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                View(await _context.Users
                    .Where(u => u.IsActive || !u.IsActive)
                    .Include(u => u.Project)
                    .OrderBy(p => p.IsActive ? 0 : 1)
                    .ToListAsync()) :
                Problem("Entity set 'CrudmvcContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(t => t.Role)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");

            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Name,Lastname,Email,Password,IsActive,Kind,CreatedAt,RoleId,ProjectId")] User user)
        {
            user.Password = Utilities.EncriptarClave(user.Password);

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", user.ProjectId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
            .Include(u => u.TicketLogs)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", user.ProjectId);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Name,Lastname,Email,Password,IsActive,Kind,CreatedAt,RoleId,ProjectId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            var currentUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (currentUser == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(p => p.ErrorMessage)).ToList();

                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }

                ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", user.ProjectId);
                return View(user);
            }

            if (!HttpContext.Request.Form.ContainsKey("IsActive"))
            {
                // If not, set IsActive to the current value in the database
                user.IsActive = currentUser.IsActive;
            }

            // Comprobar si la contraseña ingresada es diferente a la contraseña actual antes de encriptarla
            if (user.Password != currentUser.Password)
            {
                user.Password = Utilities.EncriptarClave(user.Password);
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    // Log the exception
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            catch (Exception ex) // Catch any other exception
            {
                // Log the exception
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Project)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Cambiar el estado de IsActive
            user.IsActive = !user.IsActive;

            // Guardar los cambios en la base de datos
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
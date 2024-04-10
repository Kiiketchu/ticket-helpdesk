using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDMVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CRUDMVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CrudmvcContext _context;

        public TicketsController(CrudmvcContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int? searchId)
        {
            var tickets = from t in _context.Tickets
                          select t;

            if (searchId.HasValue)
            {
                tickets = tickets.Where(s => s.Id == searchId.Value);
            }

            var crudmvcContext = tickets
                .Include(t => t.Category)
                .Include(t => t.Kind)
                .Include(t => t.Priority)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.User)
                .Include(t => t.TicketLogs);

            return View(await crudmvcContext.ToListAsync());
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Kind)
                .Include(t => t.Priority)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.User)
                .Include(t => t.TicketLogs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["KindId"] = new SelectList(_context.Kinds, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,UpdatedAt,CreatedAt,KindId,UserId,AsignedId,ProjectId,CategoryId,PriorityId,StatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                {
                    ticket.CreatedAt = DateTime.Now;
                    ticket.StatusId = 1;
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", ticket.CategoryId);
            ViewData["KindId"] = new SelectList(_context.Kinds, "Id", "Name", ticket.KindId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", ticket.PriorityId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", ticket.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.TicketLogs)
                // .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", ticket.CategoryId);
            ViewData["KindId"] = new SelectList(_context.Kinds, "Id", "Name", ticket.KindId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", ticket.PriorityId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", ticket.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", ticket.UserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UpdatedAt,KindId,UserId,AsignedId,ProjectId,CategoryId,PriorityId,StatusId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalTicket = await _context.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (originalTicket == null)
                    {
                        return NotFound();
                    }

                    ticket.CreatedAt = originalTicket.CreatedAt; // Preserve the original CreatedAt value
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", ticket.CategoryId);
            ViewData["KindId"] = new SelectList(_context.Kinds, "Id", "Id", ticket.KindId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Id", ticket.PriorityId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Id", ticket.ProjectId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", ticket.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Kind)
                .Include(t => t.Priority)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'CrudmvcContext.Tickets' is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToLog(int id, string comment)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var log = new TicketLog
            {
                TicketId = id,
                Comment = comment,
                ModificationDate = DateTime.Now

            };

            _context.TicketLogs.Add(log);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = id });
        }


        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}

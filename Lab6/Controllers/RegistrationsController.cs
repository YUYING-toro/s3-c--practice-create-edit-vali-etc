using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.Models.DataAccess;

namespace Lab6.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly StudentRecordContext _context;

        public RegistrationsController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            var studentRecordContext = _context.Registrations.Include(r => r.CourseCourse).Include(r => r.StudentStudentNumNavigation);
            return View(await studentRecordContext.ToListAsync());
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.CourseCourse)
                .Include(r => r.StudentStudentNumNavigation)
                .FirstOrDefaultAsync(m => m.CourseCourseId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            ViewData["CourseCourseId"] = new SelectList(_context.Courses, "Code", "Code");
            ViewData["StudentStudentNum"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCourseId,StudentStudentNum")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCourseId"] = new SelectList(_context.Courses, "Code", "Code", registration.CourseCourseId);
            ViewData["StudentStudentNum"] = new SelectList(_context.Students, "Id", "Id", registration.StudentStudentNum);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["CourseCourseId"] = new SelectList(_context.Courses, "Code", "Code", registration.CourseCourseId);
            ViewData["StudentStudentNum"] = new SelectList(_context.Students, "Id", "Id", registration.StudentStudentNum);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseCourseId,StudentStudentNum")] Registration registration)
        {
            if (id != registration.CourseCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.CourseCourseId))
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
            ViewData["CourseCourseId"] = new SelectList(_context.Courses, "Code", "Code", registration.CourseCourseId);
            ViewData["StudentStudentNum"] = new SelectList(_context.Students, "Id", "Id", registration.StudentStudentNum);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.CourseCourse)
                .Include(r => r.StudentStudentNumNavigation)
                .FirstOrDefaultAsync(m => m.CourseCourseId == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(string id)
        {
            return _context.Registrations.Any(e => e.CourseCourseId == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.ViewModels;

namespace SchoolManager.Controllers
{
    public class SchoolController : Controller
    {
        private readonly AppDbContext _context;

        public SchoolController(AppDbContext context)
        {
            _context = context;
        }

        //A parada não tá funcionado!!!
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var schools = _context.Schools.ToListAsync(cancellationToken);

            var model = schools.Result.Select(school => new IndexSchoolVM
            {
                Id = school.Id,
                Name = school.Name,
                City = school.City
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndexSchoolVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var administrator = await _context.Administrators.FirstOrDefaultAsync(cancellationToken);

            if (administrator == null)
            {
                ModelState.AddModelError(string.Empty, "No administrator found.");
                return View(model);
            }

            // testando pull request

            var school = new School
            {
                Name = model.Name,
                City = model.City,
                Administrator = administrator,
            };

            await _context.Schools.AddAsync(school, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSchoolVM model)
        {
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }
    }
}

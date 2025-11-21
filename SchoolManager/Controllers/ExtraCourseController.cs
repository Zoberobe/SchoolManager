using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.ViewModels.ExtraCourse;

namespace SchoolManager.Controllers
{
    public class ExtraCourseController : Controller
    {
        private readonly AppDbContext _context;

        public ExtraCourseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var extraCourses = _context.ExtraCourses
                .Where(ec => !ec.IsDeleted)
                .Select(ec => new IndexExtraCourseVM
                {
                    Uuid = ec.Uuid,
                    Name = ec.Name,
                    Hours = ec.Hours
                })
                .ToListAsync(cancellationToken).Result;


            return View(extraCourses);
        }

        public async Task<IActionResult> Details(Guid uuid, CancellationToken cancellationToken)
        {
            var extraCourse = await _context.ExtraCourses.FirstOrDefaultAsync(ec => ec.Uuid == uuid, cancellationToken);

            if (extraCourse is null)
            {
                ModelState.AddModelError(string.Empty, "Extra course not found.");
                return View();
            }

            var model = new DetailsExtraCourseVM
            {
                Uuid = extraCourse.Uuid,
                Name = extraCourse.Name,
                Hours = extraCourse.Hours
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExtraCourseVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var extraCourse = new ExtraCourse(model.Name, model.Hours);
            
            await _context.ExtraCourses.AddAsync(extraCourse, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid uuid, CancellationToken cancellationToken)
        {
            var extraCourse = await _context.ExtraCourses.FirstOrDefaultAsync(ec => ec.Uuid == uuid && !ec.IsDeleted, cancellationToken);
            
            if (extraCourse is null)
            {
                ModelState.AddModelError(string.Empty, "Extra course not found.");
                return View();
            }

            var model = new EditExtraCourseVM
            {
                Uuid = extraCourse.Uuid,
                Name = extraCourse.Name,
                Hours = extraCourse.Hours
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditExtraCourseVM model, CancellationToken cancellationToken)
        {
            var extraCourse = await _context.ExtraCourses.FirstOrDefaultAsync(ec => ec.Uuid == model.Uuid && !ec.IsDeleted, cancellationToken);

            if (!ModelState.IsValid)
                return View(model);

            if (extraCourse is null)
            {
                ModelState.AddModelError(string.Empty, "Extra course not found.");
                return View(model);
            }

            extraCourse.Edit(model.Name, model.Hours);

            _context.ExtraCourses.Update(extraCourse);

            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid uuid, CancellationToken cancellationToken)
        {
            var extraCourse = await _context.ExtraCourses.FirstOrDefaultAsync(ec => ec.Uuid == uuid && !ec.IsDeleted, cancellationToken);

            if (extraCourse is null)
            {
                ModelState.AddModelError(string.Empty, "Extra course not found.");
                return View();
            }

            var model = new DeleteExtraCourseVM
            {
                Uuid = extraCourse.Uuid,
                Name = extraCourse.Name,
                Hours = extraCourse.Hours
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteExtraCourseVM model, CancellationToken cancellationToken)
        {
            var extraCourse = await _context.ExtraCourses.FirstOrDefaultAsync(ec => ec.Uuid == model.Uuid && !ec.IsDeleted, cancellationToken);

            if (extraCourse is null)
            {
                ModelState.AddModelError(string.Empty, "Extra course not found.");
                return View(model);
            }

            _context.ExtraCourses.Remove(extraCourse);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}

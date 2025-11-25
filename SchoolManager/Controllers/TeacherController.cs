using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.Enums;

using SchoolManager.Models.ViewModels.TeacherVM;
using X.PagedList.Extensions;

namespace SchoolManager.Controllers
{
    public class TeacherController : Controller
    {
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        private readonly AppDbContext _context;

        public async Task<IActionResult> Index(int? page, CancellationToken cancellationToken)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var teachers = await _context.Teachers
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);

            var model = teachers.Select(t => new IndexTeacherIVM
            {
                Uuid = t.Uuid,
                Name = t.Name,
                Matter = t.Matter
            });
            var pagedList = model.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var model = new CreateTeacherIVM
            {
                Schoollist = await GetSchoolSelectList(cancellationToken),
                Matterlists = GetMatterSelectList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeacherIVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // Se der erro, PRECISA popular as listas novamente antes de devolver a View
                model.Matterlists = GetMatterSelectList();
                model.Schoollist = await GetSchoolSelectList(cancellationToken);
                return View(model);
            }

            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Name == model.SchoolName, cancellationToken);

            if (school == null)
            {
                ModelState.AddModelError(nameof(model.SchoolName), "Identificador de escola inválido ou não encontrado.");
                // Repopula as listas aqui também
                model.Matterlists = GetMatterSelectList();
                model.Schoollist = await GetSchoolSelectList(cancellationToken);
                return View(model);
            }

            var teacher = new Teacher(model.Name, model.Birth)
            {
                Matter = model.Matter,
                SchoolId = school.Id
            };
            teacher.SetSalary(model.Salary);

            await _context.Teachers.AddAsync(teacher, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid uuid, CancellationToken cancellationToken)
        {
            var teacher = await _context.Teachers
                                    .Include(t => t.School)
                                    .FirstOrDefaultAsync(t => t.Uuid == uuid, cancellationToken);

            if (teacher == null) return View();

            var model = new EditTeacherIVM
            {
                Uuid = teacher.Uuid,
                Name = teacher.Name,
                Birth = teacher.Birth,
                Matter = teacher.Matter,
                InputSalary = teacher.Salary,
                SchoolName = teacher.School?.Name ?? "",

                // Aqui você já está passando os dados corretamente para o Model
                Matterlists = GetMatterSelectList(),
                Schoollist = await GetSchoolSelectList(cancellationToken)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid uuid, EditTeacherIVM model, CancellationToken cancellationToken)
        {
            if (uuid != model.Uuid) return View();

            if (!ModelState.IsValid)
            {
                // Repopula listas em caso de erro
                model.Matterlists = GetMatterSelectList();
                model.Schoollist = await GetSchoolSelectList(cancellationToken);
                return View(model);
            }

            var dbTeacherToUpdate = await _context.Teachers.FirstOrDefaultAsync(t => t.Uuid == uuid, cancellationToken);
            if (dbTeacherToUpdate == null) return View();

            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Name == model.SchoolName, cancellationToken);

            if (school == null)
            {
                ModelState.AddModelError(nameof(model.SchoolName), "Escola não encontrada.");
                // Repopula listas
                model.Matterlists = GetMatterSelectList();
                model.Schoollist = await GetSchoolSelectList(cancellationToken);
                return View(model);
            }

            dbTeacherToUpdate.SetBirth(model.Birth);
            dbTeacherToUpdate.Matter = model.Matter;
            dbTeacherToUpdate.SchoolId = school.Id;
            dbTeacherToUpdate.SetSalary(model.InputSalary);
            dbTeacherToUpdate.UpdateName(model.Name);

            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid uuid, CancellationToken cancellationToken)
        {
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Uuid == uuid, cancellationToken);

            if (teacher == null)
                return NotFound();

            var model = new DeleteTeacher
            {
                Uuid = teacher.Uuid,
                Name = teacher.Name,
                Birth = teacher.Birth,
                Matter = teacher.Matter,
                Salary = teacher.Salary
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid uuid, CancellationToken cancellationToken)
        {
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Uuid == uuid, cancellationToken);

            if (teacher == null)
                return NotFound();

            teacher.MarkAsDeleted();
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid uuid, CancellationToken cancellationToken)
        {
            var teacher = await _context.Teachers
                .Include(t => t.School)
                .FirstOrDefaultAsync(t => t.Uuid == uuid, cancellationToken);

            if (teacher == null)
                return NotFound();

            var model = new DetailsTeacherIVM
            {
                Name = teacher.Name,
                Birth = teacher.Birth,
                Matter = teacher.Matter,
                Salary = teacher.Salary,
                SchoolName = teacher.School?.Name ?? "N/A"
            };

            return View(model);
        }


        private IEnumerable<SelectListItem> GetMatterSelectList()
        {
            return Enum.GetValues(typeof(Matter))
                       .Cast<Matter>()
                       .Select(m => new SelectListItem
                       {
                           Text = m.ToString(),
                           Value = m.ToString()
                       });
        }

        private async Task<IEnumerable<SelectListItem>> GetSchoolSelectList(CancellationToken cancellationToken)
        {
            var schools = await _context.Schools
                                        .OrderBy(s => s.Name)
                                        .Select(s => s.Name)
                                        .ToListAsync(cancellationToken);

            return schools.Select(s => new SelectListItem
            {
                Text = s,
                Value = s
            });
        }
    }
}


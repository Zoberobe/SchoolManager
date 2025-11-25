using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.Enums;
using SchoolManager.Models.ViewModels;
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
                // ❌ Salary removido porque tem private set e nem é usado na listagem
            });

            var pagedList = model.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Matters = Enum.GetNames(typeof(Models.Enums.Matter));
            ViewBag.SchoolNames = await _context.Schools
                                        .OrderBy(s => s.Name) // Ordenar alfabeticamente ajuda
                                        .Select(s => s.Name)
                                        .ToListAsync();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeacherIVM model, CancellationToken cancellationToken)
        {
            // Se o model enviado do formulário tiver erro (campo obrigatório não preenchido, etc)
            // volta para a mesma tela mostrando o erro
            if (!ModelState.IsValid)
            {
                ViewBag.Matters = Enum.GetNames(typeof(Models.Enums.Matter));
                ViewBag.SchoolNames = await _context.Schools
                                            .OrderBy(s => s.Name)
                                            .Select(s => s.Name)
                                            .ToListAsync(cancellationToken);
                return View(model);
            }

            var school = await _context.Schools
                                 .FirstOrDefaultAsync(s => s.Name == model.SchoolName, cancellationToken);

            if (school == null)
            {
                ModelState.AddModelError(nameof(model.SchoolName), "Identificador de escola inválido ou não encontrado.");
                // ... Recarrega ViewBags ...
                return View(model);
            }
            // Cria o objeto School que será salvo no banco
            var teacher = new Teacher
            {
                // Use ! to suppress CS8601 warning, assuming Name is required and validated by ModelState
                Birth = model.Birth,
                Matter = model.Matter,
                SchoolId = school.Id,

            };
            teacher.UpdateName(model.Name);
            teacher.SetSalary(model.Salary);

            // Adiciona a nova escola no banco
            await _context.Teachers.AddAsync(teacher, cancellationToken);

            // Salva as mudanças no banco de dados
            await _context.SaveChangesAsync(cancellationToken);

            // Redireciona para a página Index (lista de escolas)
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid uuid)
        {
            var teacher = await _context.Teachers
                                        .Include(t => t.School)
                                        .FirstOrDefaultAsync(t => t.Uuid == uuid);

            if (teacher == null)
                return NotFound();

            // Envia ENUMS para o datalist
            ViewBag.Matters = Enum.GetNames(typeof(Matter));

            // Envia lista de escolas existentes
            ViewBag.SchoolNames = await _context.Schools
                                                .OrderBy(s => s.Name)
                                                .Select(s => s.Name)
                                                .ToListAsync();

            // Cria o model preenchido
            var model = new EditTeacherIVM
            {
                Uuid = teacher.Uuid,
                Name = teacher.Name,
                Birth = teacher.Birth,
                Matter = teacher.Matter,
                InputSalary = teacher.Salary,
                SchoolName = teacher.School?.Name ?? ""
            };

            return View(model);
        }


        // Recebe dados do formulário de edição (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid uuid, EditTeacherIVM model)
        {
            if (uuid != model.Uuid)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Matters = Enum.GetNames(typeof(Matter));
                ViewBag.SchoolNames = await _context.Schools
                                                    .OrderBy(s => s.Name)
                                                    .Select(s => s.Name)
                                                    .ToListAsync();
                return View(model);
            }

            var dbTeacherToUpdate = await _context.Teachers
                                                  .FirstOrDefaultAsync(t => t.Uuid == uuid);

            if (dbTeacherToUpdate == null)
                return NotFound();

            // 🔥 1. Buscar a escola CORRETAMENTE
            var school = await _context.Schools
                                       .FirstOrDefaultAsync(s => s.Name == model.SchoolName);

            if (school == null)
            {
                ModelState.AddModelError(nameof(model.SchoolName), "Escola não encontrada.");
                ViewBag.Matters = Enum.GetNames(typeof(Matter));
                ViewBag.SchoolNames = await _context.Schools
                                                    .OrderBy(s => s.Name)
                                                    .Select(s => s.Name)
                                                    .ToListAsync();
                return View(model);
            }

            // 🔥 2. Atualizar os dados normalmente
            dbTeacherToUpdate.Birth = model.Birth;
            dbTeacherToUpdate.Matter = model.Matter;
            dbTeacherToUpdate.SchoolId = school.Id; // CORRETO
            dbTeacherToUpdate.SetSalary(model.InputSalary);
            dbTeacherToUpdate.UpdateName(model.Name ?? "");

            await _context.SaveChangesAsync();

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

            teacher.IsDeleted = true;

            _context.Teachers.Update(teacher);
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

    }
}


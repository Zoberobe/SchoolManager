using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.ViewModels;
using X.PagedList.Extensions;

namespace SchoolManager.Controllers
{
    public class StudentController : Controller
    {

        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        //Pega todos os registros no banco  
        public async Task<IActionResult> Index(int? page)
        {
            
            int pageSize = 10;
            
            int pageNumber = page ?? 1;

            
            var students = await _context.Students
                .Where(s => s.IsDeleted == false)
                .OrderBy(s => s.Name) 
                .ToListAsync();

            // Mapeia para a ViewModel
            var viewModels = students.Select(s => new StudentFormViewModel
            {
                Uuid = s.Uuid,
                Name = s.Name,
                IsScholarshipRecipient = s.IsScholarshipRecipient,
                MonthlyFee = s.MonthlyFee
            });

            var pagedList = viewModels.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        //Pega o detalhe de um Estudante especifico

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .Include(s => s.School)                 
                .Include(s => s.StudyGroup)            
                    .ThenInclude(g => g.Teacher)        
                .FirstOrDefaultAsync(m => m.Uuid == id);

            if (student == null) return NotFound();

            var viewModel = new StudentFormViewModel
            {
                Uuid = student.Uuid,
                Name = student.Name,
                IsScholarshipRecipient = student.IsScholarshipRecipient,
                MonthlyFee = student.MonthlyFee,

                SchoolName = student.School?.Name ?? "Sem Escola",

                // Formatando nome da turma (ex: 2025 - 2026)
                StudyGroupName = student.StudyGroup != null
                    ? $"{student.StudyGroup.InitialDate.Year} - {student.StudyGroup.FinalDate.Year}"
                    : "Sem Turma",

                TeacherName = student.StudyGroup?.Teacher?.Name ?? "Sem Professor"
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {

            ViewData["Schools"] = new SelectList(await _context.Schools.ToListAsync(), "Uuid", "Name");
            await LoadviewBags(); 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Criar um novo estudante
        public async Task<IActionResult> Create(StudentFormViewModel viewModel)
        {
            // Regra do Bolsista
            if (viewModel.IsScholarshipRecipient)
            {
                viewModel.MonthlyFee = 0;
                ModelState.Remove("MonthlyFee");
            }

            if (ModelState.IsValid)
            {
                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == viewModel.SchoolUuid);

                
                var studyGroup = await _context.StudyGroups.FirstOrDefaultAsync(g => g.Uuid == viewModel.StudyGroupUuid);

                if (school == null || studyGroup == null)
                {
                    if (school == null) ModelState.AddModelError("SchoolUuid", "Escola inválida.");
                    if (studyGroup == null) ModelState.AddModelError("StudyGroupUuid", "Turma inválida.");
                }
                else
                {
                    var student = new Student(
                        viewModel.Name,
                        viewModel.MonthlyFee,
                        viewModel.IsScholarshipRecipient,
                        school.Id,     
                        studyGroup.Id  
                    );

                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Recarrega os dropdowns em caso de erro
            await LoadviewBags(viewModel.SchoolUuid, viewModel.StudyGroupUuid);
            return View(viewModel);
        }
        //Editar um estudante existente

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Uuid == id);
            if (student == null) return NotFound();

            
            var currentSchool = await _context.Schools.FindAsync(student.SchoolId);
            var currentGroup = await _context.StudyGroups.FindAsync(student.StudyGroupId); 

            var viewModel = new StudentFormViewModel
            {
                Uuid = student.Uuid,
                Name = student.Name,
                IsScholarshipRecipient = student.IsScholarshipRecipient,
                MonthlyFee = student.MonthlyFee,
                SchoolUuid = currentSchool?.Uuid ?? Guid.Empty,
                StudyGroupUuid = currentGroup?.Uuid ?? Guid.Empty 
            };

            
            await LoadviewBags(viewModel.SchoolUuid, viewModel.StudyGroupUuid);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentFormViewModel viewModel)
        {
            if (id != viewModel.Uuid) return NotFound();

            if (!viewModel.IsScholarshipRecipient && viewModel.MonthlyFee < 500)
            {
                ModelState.AddModelError("MonthlyFee", "Para alunos não bolsistas, o valor mínimo é R$ 500,00.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var studentOriginal = await _context.Students.FirstOrDefaultAsync(s => s.Uuid == id);
                    if (studentOriginal == null) return NotFound();

                    // 1. Busca a Nova Escola
                    var newSchool = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == viewModel.SchoolUuid);

                    // 2. Busca a Nova Turma (FALTAVA ISSO)
                    var newGroup = await _context.StudyGroups.FirstOrDefaultAsync(g => g.Uuid == viewModel.StudyGroupUuid);

                    if (newSchool == null || newGroup == null)
                    {
                        if (newSchool == null) ModelState.AddModelError("SchoolUuid", "Escola inválida.");
                        if (newGroup == null) ModelState.AddModelError("StudyGroupUuid", "Turma inválida.");

                        await LoadviewBags(viewModel.SchoolUuid, viewModel.StudyGroupUuid);
                        return View(viewModel);
                    }

                    // 3. Atualiza o perfil completo
                    studentOriginal.UpdateFullProfile(
                        viewModel.Name,
                        viewModel.MonthlyFee,
                        newSchool.Id,   
                        newGroup.Id,   
                        viewModel.IsScholarshipRecipient
                    );

                    _context.Update(studentOriginal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(viewModel.Uuid)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await LoadviewBags(viewModel.SchoolUuid, viewModel.StudyGroupUuid);
            return View(viewModel);
        }

        //Excluir um estudante

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Uuid == id);

            if (student == null) return NotFound();

            var viewModel = new StudentFormViewModel
            {
                Uuid = student.Uuid,
                Name = student.Name,
                IsScholarshipRecipient = student.IsScholarshipRecipient,
                MonthlyFee = student.MonthlyFee
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) // Recebe Guid
        {
            // Busca pelo UUID
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Uuid == id);

            if (student != null)
            {
                student.Delete();

                _context.Update(student); // Atualiza o registro
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid uuid)
        {
            return _context.Students.Any(e => e.Uuid == uuid);
        }

        private async Task LoadviewBags(Guid? selectedSchoolUuid = null, Guid? selectedGroupUuid = null)
        {
            ViewData["Schools"] = new SelectList(await _context.Schools.ToListAsync(), "Uuid", "Name", selectedSchoolUuid);


            var groups = new List<object>();

            if (selectedSchoolUuid != null)
            {
                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == selectedSchoolUuid);

                if (school != null)
                {
                    groups = await _context.StudyGroups
                        .Where(g => g.SchoolId == school.Id) 
                        .Include(g => g.Teacher)
                        .Select(g => new
                        {
                            Uuid = g.Uuid,
                            DisplayName = $"{g.Teacher.Name} - {g.InitialDate.Year}"
                        })
                        .ToListAsync<object>();
                }
            }

            ViewData["StudyGroups"] = new SelectList(groups, "Uuid", "DisplayName", selectedGroupUuid);
        }

        [HttpGet]
        public async Task<JsonResult> GetStudyGroupsBySchool(Guid schoolUuid)
        {
            var school = await _context.Schools
                .FirstOrDefaultAsync(s => s.Uuid == schoolUuid);

            if (school == null)
            {
                return Json(new List<object>());
            }

            var groups = await _context.StudyGroups
                .Where(g => g.SchoolId == school.Id) 
                .Include(g => g.Teacher) 
                .Select(g => new
                {
                    
                    value = g.Uuid, 
                    text = $"{g.Teacher.Name} - {g.InitialDate.Year}" 
                })
                .ToListAsync();


            return Json(groups);
        }
    }
}

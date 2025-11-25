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
        public async Task<IActionResult> Index(int? page, CancellationToken cancellationToken)
        {
            
            int pageSize = 10;
            
            int pageNumber = page ?? 1;

            
            var students = await _context.Students
                .Where(s => s.IsDeleted == false)
                .OrderBy(s => s.Name) 
                .ToListAsync(cancellationToken);

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

        public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .Include(s => s.School)                 
                .Include(s => s.StudyGroup)            
                    .ThenInclude(g => g.Teacher)        
                .FirstOrDefaultAsync(m => m.Uuid == id, cancellationToken);

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
            var viewModel = new StudentFormViewModel();
            await ViewStudents(viewModel); // Chamada corrigida
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Criar um novo estudante
        public async Task<IActionResult> Create(StudentFormViewModel viewModel, CancellationToken cancellationToken)
        {
            // Regra do Bolsista
            if (viewModel.IsScholarshipRecipient)
            {
                viewModel.MonthlyFee = 0;
                ModelState.Remove("MonthlyFee");
            }

            if (ModelState.IsValid)
            {
                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == viewModel.SchoolUuid, cancellationToken);

                
                var studyGroup = await _context.StudyGroups.FirstOrDefaultAsync(g => g.Uuid == viewModel.StudyGroupUuid, cancellationToken);

                if (school == null || studyGroup == null)
                {
                    if (school == null) ModelState.AddModelError("SchoolUuid", "Escola inválida.");
                    if (studyGroup == null) ModelState.AddModelError("StudyGroupUuid", "Turma inválida.");
                }
                else
                {
                    var student = new Student(
                        viewModel.Name,
                        DateOnly.FromDateTime(DateTime.Now),
                        viewModel.MonthlyFee,
                        viewModel.IsScholarshipRecipient,
                        school.Id,     
                        studyGroup.Id  
                    );

                    _context.Add(student);
                    await _context.SaveChangesAsync(cancellationToken);
                    return RedirectToAction(nameof(Index));
                }
            }

            // Recarrega os dropdowns em caso de erro
            await ViewStudents(viewModel, cancellationToken);
            return View(viewModel);
        }
        //Editar um estudante existente

        public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);
            if (student == null) return NotFound();

            
            var currentSchool = await _context.Schools.FindAsync(student.SchoolId, cancellationToken);
            var currentGroup = await _context.StudyGroups.FindAsync(student.StudyGroupId, cancellationToken); 

            var viewModel = new StudentFormViewModel
            {
                Uuid = student.Uuid,
                Name = student.Name,
                IsScholarshipRecipient = student.IsScholarshipRecipient,
                MonthlyFee = student.MonthlyFee,
                SchoolUuid = currentSchool?.Uuid ?? Guid.Empty,
                StudyGroupUuid = currentGroup?.Uuid ?? Guid.Empty 
            };


            await ViewStudents(viewModel, cancellationToken);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentFormViewModel viewModel, CancellationToken cancellationToken)
        {
            if (id != viewModel.Uuid) return NotFound();

            if (!viewModel.IsScholarshipRecipient && viewModel.MonthlyFee < 500)
            {
                ModelState.AddModelError("MonthlyFee", "Para alunos não bolsistas, o valor mínimo é R$ 500,00.");
            }

            if (ModelState.IsValid)
            {
                    var studentOriginal = await _context.Students.FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);
                    if (studentOriginal == null) return NotFound();

                    var newSchool = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == viewModel.SchoolUuid, cancellationToken);
                    var newGroup = await _context.StudyGroups.FirstOrDefaultAsync(g => g.Uuid == viewModel.StudyGroupUuid, cancellationToken);

                    if (newSchool == null || newGroup == null)
                    {
                        if (newSchool == null) ModelState.AddModelError("SchoolUuid", "Escola inválida.");
                        if (newGroup == null) ModelState.AddModelError("StudyGroupUuid", "Turma inválida.");

                        await ViewStudents(viewModel, cancellationToken);
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
                    await _context.SaveChangesAsync(cancellationToken);
                }

            await ViewStudents(viewModel, cancellationToken);
            return View(viewModel);
        }

        //Excluir um estudante

        public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Uuid == id, cancellationToken);

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
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken) // Recebe Guid
        {
            // Busca pelo UUID
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);

            if (student != null)
            {
                student.MarkAsDeleted();

                _context.Update(student); // Atualiza o registro
                await _context.SaveChangesAsync(cancellationToken);
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task ViewStudents(StudentFormViewModel viewModel, CancellationToken cancellationToken = default)
        {
            // Carrega escolas
            var schools = await _context.Schools.OrderBy(x => x.Name).ToListAsync(cancellationToken);
            viewModel.SchoolsList = new SelectList(schools, "Uuid", "Name", viewModel.SchoolUuid);

            // Lógica de Grupos
            var groups = new List<object>();

            if (viewModel.SchoolUuid != Guid.Empty)
            {
                var school = schools.FirstOrDefault(s => s.Uuid == viewModel.SchoolUuid);

                if (school != null)
                {
                    groups = await _context.StudyGroups
                        .Where(g => g.SchoolId == school.Id)
                        .Include(g => g.Teacher)
                        .Select(g => new
                        {
                            DisplayName = $"{g.Teacher.Name} - {g.InitialDate.Year}"
                        })
                        .ToListAsync<object>(cancellationToken);
                }
            }

            viewModel.StudyGroupsList = new SelectList(groups, "Uuid", "DisplayName", viewModel.StudyGroupUuid);
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

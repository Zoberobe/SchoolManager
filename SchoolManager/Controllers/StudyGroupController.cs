using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.ViewModels.StudyGroupVM;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using X.PagedList.Extensions;
using static SchoolManager.Models.ViewModels.StudyGroupVM.DetailsStudyGroupVM;


namespace SchoolManager.Controllers
{
    public class StudyGroupController : Controller
    {

        private readonly AppDbContext _context;

        public StudyGroupController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? page, CancellationToken cancellationToken)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var studygroupQuery = _context.StudyGroups
                .Where(s => s.IsDeleted == false)
                .OrderBy(s => s.Name)
                .Select(s => new IndexStudyGroupVM
                {
                    Uuid = s.Uuid,
                    Name = s.Name,
                    SchoolName = s.School.Name,
                    TeacherName = s.Teacher.Name,
                    StudentsCount = s.Students.Count(st => st.IsDeleted == false),
                    InitialDate = s.InitialDate,
                    FinalDate = s.FinalDate
                });

            var model = studygroupQuery.ToPagedList(pageNumber, pageSize);

            return View(model);
        }
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var model = new CreateStudyGroupVM();

            var schools = await _context.Schools
                .Where(s => s.IsDeleted == false)
                .ToListAsync(cancellationToken);


            model.SchoolsList = new SelectList(schools, "Id", "Name");
            model.TeachersList = new SelectList(new List<Teacher>(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudyGroupVM model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string gradeText = GetEnumDisplayName(model.Grade);
                string shiftText = GetEnumDisplayName(model.Shift);

                string finalName = $"{gradeText} - Turma {model.Section} ({shiftText})";

                var studyGroup = new StudyGroup(
                    finalName,
                    model.InitialDate,
                    model.FinalDate,
                    model.SchoolId,
                    model.TeacherId
                );

                _context.Add(studyGroup);


                await _context.SaveChangesAsync(cancellationToken);

                return RedirectToAction(nameof(Index));
            }

            var schools = await _context.Schools.ToListAsync(cancellationToken);
            var teachers = await _context.Teachers.ToListAsync(cancellationToken);

            model.SchoolsList = new SelectList(schools, "Id", "Name", model.SchoolId);
            model.TeachersList = new SelectList(teachers, "Id", "Name", model.TeacherId);

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null) return NotFound();

            var studyGroup = await _context.StudyGroups
                .Include(s => s.School)
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);

            if (studyGroup == null) return NotFound();

            var model = new StudyGroupEditViewModel
            {
                Uuid = studyGroup.Uuid,
                Name = studyGroup.Name,
                InitialDate = studyGroup.InitialDate,
                FinalDate = studyGroup.FinalDate,
                SchoolUuid = studyGroup.School.Uuid,
                TeacherUuid = studyGroup.Teacher.Uuid
            };

            var schools = await _context.Schools
                .Where(s => !s.IsDeleted)
                .ToListAsync(cancellationToken);

            var teachers = await _context.Teachers
                .Where(t => t.SchoolId == studyGroup.SchoolId && !t.IsDeleted)
                .ToListAsync(cancellationToken);

            model.SchoolsList = new SelectList(schools, "Uuid", "Name", model.SchoolUuid);
            model.TeachersList = new SelectList(teachers, "Uuid", "Name", model.TeacherUuid);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudyGroupEditViewModel model, CancellationToken cancellationToken)
        {
            if (id != model.Uuid) return NotFound();

            ModelState.Remove("Grade");
            ModelState.Remove("Section");
            ModelState.Remove("Shift");

            if (ModelState.IsValid)
            {
                var studyGroup = await _context.StudyGroups.FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);
                if (studyGroup == null) return NotFound();

                var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == model.SchoolUuid, cancellationToken);
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Uuid == model.TeacherUuid, cancellationToken);

                if (school != null && teacher != null)
                {


                    studyGroup.FinalDate = model.FinalDate; 
                    studyGroup.SchoolId = school.Id;        
                    studyGroup.TeacherId = teacher.Id;      

                    _context.Update(studyGroup);
                    await _context.SaveChangesAsync(cancellationToken);

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {   
            var studyGroup = await _context.StudyGroups
                .AsNoTracking()
                .Where(v => v.IsDeleted == false)
                .Include(s => s.Students.Where(st => st.IsDeleted == false))
                .Include(t => t.Teacher)
                .FirstOrDefaultAsync(ec => ec.Uuid == id, cancellationToken);

            if (studyGroup is null)
            {       
                return NotFound();
            }

            var school = await _context.Schools.FirstOrDefaultAsync(x => x.Id == studyGroup.SchoolId, cancellationToken);

            var model = new DetailsStudyGroupVM
            {
                Uuid = studyGroup.Uuid,
                Name = studyGroup.Name,
                InitialDate = studyGroup.InitialDate,
                FinalDate = studyGroup.FinalDate,
                Students = studyGroup.Students,
                Teacher = new TeacherProjectionVM { Name = studyGroup.Name, Uuid = studyGroup.Uuid },
                school = new SchoolProjectionVM { Name = school.Name, Uuid = school.Uuid }
            };

            return View(model);
        }


        public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null) return View();

            var studyGroup = await _context.StudyGroups
            .Include(s => s.Teacher)
            .Include(s => s.School)
            .Include(s => s.Students.Where(st => st.IsDeleted == false))
            .FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);

            if (studyGroup == null) return View();
            string groupName = studyGroup.Name ?? "Grupo sem nome";
            var viewModel = new DeleteStudyGroupVM
            {
                Uuid = studyGroup.Uuid,
                Name = groupName,
                InitialDate = studyGroup.InitialDate,
                FinalDate = studyGroup.FinalDate,
                SchoolName = studyGroup.School?.Name ?? "Escola não encontrada",
                TeacherName = studyGroup.Teacher?.Name ?? "Professor não encontrado",
                StudentCount = studyGroup.Students.Count
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID inválido fornecido para exclusão.");
            }
            var studyGroup = await _context.StudyGroups
                .FirstOrDefaultAsync(s => s.Uuid == id, cancellationToken);
            if (studyGroup == null)
            {
                return NotFound();
            }
            studyGroup.MarkAsDeleted();
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        private string GetEnumDisplayName(Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue.ToString();
        }

        [HttpGet]
        public async Task<JsonResult> GetTeachersBySchool(int schoolId, CancellationToken cancellationToken)
        {
            var teachers = await _context.Teachers
                .Where(t => t.SchoolId == schoolId && t.IsDeleted == false)
                .OrderBy(t => t.Name)
                .Select(t => new { t.Id, t.Name })
                .ToListAsync(cancellationToken);

            return Json(teachers);
        }

        [HttpGet]
        public async Task<JsonResult> GetTeachersBySchoolUuid(Guid schoolUuid, CancellationToken cancellationToken)
        {
            // Descobre o ID inteiro
            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == schoolUuid, cancellationToken);

            if (school == null) return Json(new List<object>());

            var teachers = await _context.Teachers
                .Where(t => t.SchoolId == school.Id && t.IsDeleted == false)
                .OrderBy(t => t.Name)
                .Select(t => new { value = t.Uuid, text = t.Name }) 
                .ToListAsync(cancellationToken);

            return Json(teachers);
        }

    }
}

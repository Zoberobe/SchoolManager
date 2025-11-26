using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.ViewModels.StudyGroupVM;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using X.PagedList.Extensions;


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

            var model =  studygroupQuery.ToPagedList(pageNumber, pageSize);

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

        public async Task<IActionResult> Edit(CancellationToken cancellationToken)
        {
            return View();
        }
        [HttpPost]
        //public async Task<IActionResult> Edit(CancellationToken cancellationToken)
        //{
        //    return View();
        //}

        public async Task<IActionResult> Details(CancellationToken cancellationToken)
        {
            return View();
        }

        public async Task<IActionResult> Delete(CancellationToken cancellationToken)
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> DeletConfirmed(CancellationToken cancellationToken)
        {
            return View();
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

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models.ViewModels.SchoolVM;
using SchoolManager.Models.ViewModels.StudyGroupVM;

namespace SchoolManager.Controllers
{
    public class StudyGroupController : Controller
    {

        private readonly AppDbContext _context;

        public StudyGroupController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {

            var studygroupQuery = _context.StudyGroups
                .Where(s => s.IsDeleted == false)
                .OrderBy(s => s.Id);


            var studygroup = await studygroupQuery.ToListAsync(cancellationToken);


            var model = studygroup.Select(a => new IndexStudyGroupVM
            {
                Uuid = a.Uuid,
                Id = a.Id,
                Teacher = a.Teacher,
                SchoolId = a.SchoolId,
                // Students = a.Students
            });
            return View(model);


        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.ViewModels.SchoolVM; // Assumindo que IndexSchoolVM e DetailsSchoolVM existem
using X.PagedList.Extensions;

namespace SchoolManager.Controllers
{
    public class SchoolController : Controller
    {
        private readonly AppDbContext _context;

        public SchoolController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page, CancellationToken cancellationToken)
        {
            
            int pageSize = 10; 
            int pageNumber = page ?? 1; 


            var schoolsQuery = _context.Schools
                .Where(s => s.IsDeleted == false)
                .OrderBy(s => s.Name);


            var schools = await schoolsQuery.ToListAsync(cancellationToken);


            var model = schools.Select(school => new IndexSchoolVM
            {
                Uuid = school.Uuid,
                Name = school.Name,
                City = school.City
            });

           
            var pagedList = model.ToPagedList(pageNumber, pageSize);

            
            return View(pagedList);
        }


        public IActionResult Create()
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
            if (administrator is null)
            {
                ModelState.AddModelError(string.Empty, "Administrador não encontrado para associar à escola.");
                return View(model);
            }
            var school = new School(model.Name, model.City, administrator);



            await _context.Schools.AddAsync(school, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid uuid, CancellationToken cancellationToken)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(ec => ec.Uuid == uuid, cancellationToken);

            if (school is null)
            {
                ModelState.AddModelError(string.Empty, "Detalhes não encontrado");
                return View();
            }

            var model = new DetailsSchoolVM
            {
                Uuid = school.Uuid,
                Name = school.Name,
                City = school.City
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid uuid, CancellationToken cancellationToken)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == uuid, cancellationToken);

            if (school is null)
            {
                ModelState.AddModelError(string.Empty, "Edit não encontrado");
                return View();
            }

            var model = new EditSchoolVM
            {
                Uuid = school.Uuid
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSchoolVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == model.Uuid, cancellationToken);
            if (school is null)
            {
                ModelState.AddModelError(string.Empty, "Escola não encontrada");
                return View(model);
            }
            school.Edit(model.Name, model.City);

            _context.Schools.Update(school);

            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(Guid uuid, CancellationToken cancellationToken)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == uuid, cancellationToken);

            if (school is null)
            {
                ModelState.AddModelError(string.Empty, "Edit não encontrado");
                return View();
            }

            var model = new DeleteSchoolVM
            {
                Uuid = school.Uuid
            };

            return View(model);
        }
        [HttpDelete]
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(DeleteSchoolVM model, CancellationToken cancellationToken)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(s => s.Uuid == model.Uuid, cancellationToken);
            if (school is null)
            {
                ModelState.AddModelError(string.Empty, "Escola não encontrada");
                return View();
            }

            school.MarkAsDeleted();
            _context.Schools.Update(school);

            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));

        }

        public ActionResult Link()
        {
            return Redirect("http://www.google.com.br/search?q=%s");
        }

    }
}
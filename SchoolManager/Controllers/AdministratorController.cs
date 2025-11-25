using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.Models;
using SchoolManager.Models.ViewModels;
using SchoolManager.Models.ViewModels.AdministratorVM;
using System.Collections.Generic;
using System.Threading;


namespace SchoolManager.Controllers
{
    public class AdministratorController : Controller
    {
        // Injeção de dependência do DbContext
        private readonly AppDbContext _context;

        // Construtor
        public AdministratorController(AppDbContext context)
        {
            _context = context;
        }

        //Honepage dos Administradores
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            
            var administrators = await _context.Administrators.Where(s => s.IsDeleted == false).ToListAsync(cancellationToken);
            var model = administrators.Select(administrator => new IndexAdministratorVM
            {
                Uuid = administrator.Uuid, // Ensure Uuid is set
                Name = administrator.Name,
                Birth = administrator.Birth,
                Capital = administrator.Capital
            }).ToList();
            return View(model);
        }

        // Ação para criar um novo administrador
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdministratorVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var administrator = new Administrator(model.Name, model.Birth, model.Capital);

            await _context.Administrators.AddAsync(administrator, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }


   
        public async Task<IActionResult> Edit(Guid uuid, CancellationToken cancellationToken)
        {
            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(a => a.Uuid == uuid, cancellationToken);

            var model = new EditAdministratorVM
            {
                Uuid = administrator!.Uuid,
                Name = administrator.Name,
                Birth = administrator.Birth,
                Capital = administrator.Capital
            };

            return View(model);
        }



        // POST: aplica as alterações e salva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid uuid, EditAdministratorVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(a => a.Uuid == uuid, cancellationToken);



            if (administrator == null)
                return NotFound();

            // Aplicar mudanças na entidade (use o método de domínio existente)
            administrator.UpdateProperties(model.Name, model.Birth, model.Capital);

            _context.Administrators.Update(administrator);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(Guid uuid, CancellationToken cancellationToken)
        {

            var administrator = _context.Administrators
                .FirstOrDefaultAsync(a => a.Uuid == uuid, cancellationToken).Result;

            if (administrator == null)
                return NotFound();

            var model = new DeleteAdministratorVM
            {
                Uuid = administrator.Uuid
            };


            return View();
        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete( DeleteAdministratorVM model, CancellationToken cancellationToken)
        {
            var administrator = await _context.Administrators
                .FirstOrDefaultAsync(a => a.Uuid == model.Uuid, cancellationToken);
            if (administrator == null)
                return NotFound();

            administrator.MarkAsDeleted();
            _context.Administrators.Update(administrator);
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid uuid, CancellationToken cancellationToken)
        {
            var administrator = _context.Administrators
                .FirstOrDefaultAsync(a => a.Uuid == uuid, cancellationToken).Result;
            if (administrator == null)
                return NotFound();

            var model = new DetailsAdministratorVM
            {
                Uuid = administrator.Uuid,
                Name = administrator.Name,
                Birth = administrator.Birth
                
            };
            return View(model);

        }

    }
}
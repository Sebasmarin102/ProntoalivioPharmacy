using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data;
using ProntoalivioPharmacy.Data.Entities;
using Vereyon.Web;

namespace ProntoalivioPharmacy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MedicineTypesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public MedicineTypesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicineTypes.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MedicineType medicinetype = await _context.MedicineTypes
                .FirstOrDefaultAsync(c => c.Id == id);
            if (medicinetype == null)
            {
                return NotFound();
            }

            return View(medicinetype);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicineType medicinetype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(medicinetype);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un tipo de medicina con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            return View(medicinetype);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MedicineType medicinetype = await _context.MedicineTypes.FindAsync(id);
            if (medicinetype == null)
            {
                return NotFound();
            }

            return View(medicinetype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicineType medicinetype)
        {
            if (id != medicinetype.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicinetype);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un tipo de medicina con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            return View(medicinetype);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MedicineType medicinetypes = await _context.MedicineTypes
                .FirstOrDefaultAsync(c => c.Id == id);
            if (medicinetypes == null)
            {
                return NotFound();
            }

            return View(medicinetypes);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            MedicineType medicinetype = await _context.MedicineTypes.FindAsync(id);
            _context.MedicineTypes.Remove(medicinetype);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Tipo de medicamento eliminado.");
            return RedirectToAction(nameof(Index));
        }
    }
}

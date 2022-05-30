using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data;
using ProntoalivioPharmacy.Data.Entities;
using ProntoalivioPharmacy.Helpers;
using Vereyon.Web;
using static ProntoalivioPharmacy.Helpers.ModalHelper;

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
            return View(await _context.MedicineTypes
                .Include(m => m.ProductCategories)
                .ToListAsync());
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

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            MedicineType medicineType = await _context.MedicineTypes.FirstOrDefaultAsync(c => c.Id == id);
            try
            {
                _context.MedicineTypes.Remove(medicineType);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el tipo de medicamento/producto porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new MedicineType());
            }
            else
            {
                MedicineType medicineType = await _context.MedicineTypes.FindAsync(id);
                if (medicineType == null)
                {
                    return NotFound();
                }

                return View(medicineType);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, MedicineType medicineType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _context.Add(medicineType);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _context.Update(medicineType);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un tipo de medicamento/producto con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                    return View(medicineType);
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(medicineType);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", 
                    _context.MedicineTypes.Include(c => c.ProductCategories).ToList()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", 
                medicineType) });
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data;
using ProntoalivioPharmacy.Data.Entities;
using ProntoalivioPharmacy.Helpers;
using ProntoalivioPharmacy.Models;
using Vereyon.Web;
using static ProntoalivioPharmacy.Helpers.ModalHelper;

namespace ProntoalivioPharmacy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public CitiesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cities
                .Include(c => c.Neighborhoods)
                .ThenInclude(s => s.Laboratories)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(c => c.Neighborhoods)
                .ThenInclude(c => c.Laboratories)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        public async Task<IActionResult> DetailsNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .Include(n => n.Laboratories)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddNeighborhood(int id)
        {
            City city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            NeighborhoodViewModel model = new()
            {
                CityId = city.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNeighborhood(NeighborhoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Neighborhood neighborhood = new()
                    {
                        Laboratories = new List<Laboratory>(),
                        City = await _context.Cities.FindAsync(model.CityId),
                        Name = model.Name,
                    };
                    _context.Add(neighborhood);
                    await _context.SaveChangesAsync();
                    City city = await _context.Cities
                        .Include(c => c.Neighborhoods)
                        .ThenInclude(s => s.Laboratories)
                        .FirstOrDefaultAsync(c => c.Id == model.CityId);
                    _flashMessage.Info("Registro creado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllNeighborhoods", city) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un barrio con el mismo nombre en esta ciudad.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddNeighborhood", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddLaboratory(int id)
        {
            Neighborhood neighborhood = await _context.Neighborhoods.FindAsync(id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            LaboratoryViewModel model = new()
            {
                NeighborhoodId = neighborhood.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLaboratory(LaboratoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Laboratory laboratory = new()
                    {
                        Neighborhood = await _context.Neighborhoods.FindAsync(model.NeighborhoodId),
                        Name = model.Name,
                    };
                    _context.Add(laboratory);
                    await _context.SaveChangesAsync();
                    Neighborhood neighborhood = await _context.Neighborhoods
                        .Include(s => s.Laboratories)
                        .FirstOrDefaultAsync(c => c.Id == model.NeighborhoodId);
                    _flashMessage.Confirmation("Registro creado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllLaboratories", neighborhood) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un laboratorio/farmacia con el mismo nombre en este barrio.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddNeighborhood", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditNeighborhood(int id)
        {
            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            NeighborhoodViewModel model = new()
            {
                CityId = neighborhood.City.Id,
                Id = neighborhood.Id,
                Name = neighborhood.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNeighborhood(int id, NeighborhoodViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Neighborhood neighborhood = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(neighborhood);
                    City city = await _context.Cities
                        .Include(c => c.Neighborhoods)
                        .ThenInclude(s => s.Laboratories)
                        .FirstOrDefaultAsync(c => c.Id == model.CityId);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro actualizado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllNeighborhoods", city) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un barrio con el mismo nombre en esta ciudad.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditNeighborhood", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditLaboratory(int id)
        {
            Laboratory laboratory = await _context.Laboratories
                .Include(l => l.Neighborhood)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }

            LaboratoryViewModel model = new()
            {
                NeighborhoodId = laboratory.Neighborhood.Id,
                Id = laboratory.Id,
                Name = laboratory.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLaboratory(int id, LaboratoryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Laboratory laboratory = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context.Update(laboratory);
                    await _context.SaveChangesAsync();
                    Neighborhood neighborhood = await _context.Neighborhoods
                        .Include(s => s.Laboratories)
                        .FirstOrDefaultAsync(c => c.Id == model.NeighborhoodId);
                    _flashMessage.Confirmation("Registro actualizado.");
                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllLaboratories", neighborhood) });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un laboratorio/farmacia con el mismo nombre en este barrio.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditLaboratory", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            try
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el país porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new City());
            }
            else
            {
                City city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound();
                }

                return View(city);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _context.Add(city);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _context.Update(city);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(
                            this,
                            "_ViewAll",
                            _context.Cities
                                .Include(c => c.Neighborhoods)
                                .ThenInclude(s => s.Laboratories)
                                .ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una ciudad con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", city) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteLaboratory(int id)
        {
            Laboratory laboratory = await _context.Laboratories
                .Include(c => c.Neighborhood)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }

            try
            {
                _context.Laboratories.Remove(laboratory);
                await _context.SaveChangesAsync();
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el laboratorio porque tiene registros relacionados.");
            }

            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(DetailsNeighborhood), new { Id = laboratory.Neighborhood.Id });
        }


    }
}

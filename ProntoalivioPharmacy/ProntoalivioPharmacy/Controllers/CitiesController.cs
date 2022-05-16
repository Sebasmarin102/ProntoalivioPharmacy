using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data;
using ProntoalivioPharmacy.Data.Entities;
using ProntoalivioPharmacy.Models;

namespace ProntoalivioPharmacy.Controllers
{
    public class CitiesController : Controller
    {
        private readonly DataContext _context;

        public CitiesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cities
                .Include(c => c.Neighborhoods)
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

        public async Task<IActionResult> DetailsLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Laboratory laboratory = await _context.Laboratories
                .Include(l => l.Neighborhood)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }

            return View(laboratory);
        }

        public IActionResult Create()
        {
            City city = new() { Neighborhoods = new List<Neighborhood>() };
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una ciudad con el mismo nombre.");
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

            return View(city);
        }

        public async Task<IActionResult> AddNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities.FindAsync(id);
            if(city == null) 
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
                    return RedirectToAction(nameof(Details), new { Id = model.CityId });
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

            return View(model);
        }

        public async Task<IActionResult> AddLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsNeighborhood), new { Id = model.NeighborhoodId });
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

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(c => c.Neighborhoods)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, City city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una ciudad con el mismo nombre.");
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
            return View(city);
        }

        public async Task<IActionResult> EditNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.CityId });
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
            return View(model);
        }

        public async Task<IActionResult> EditLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(DetailsNeighborhood), new { Id = model.NeighborhoodId });
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
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City city = await _context.Cities
                .Include(c => c.Neighborhoods)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            City city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteNeighborhood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .Include(n => n.Laboratories)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (neighborhood == null)
            {
                return NotFound();
            }

            return View(neighborhood);
        }

        [HttpPost, ActionName("DeleteNeighborhood")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNeighborhoodConfirmed(int id)
        {
            Neighborhood neighborhood = await _context.Neighborhoods
                .Include(n => n.City)
                .FirstOrDefaultAsync(n => n.Id == id);
            _context.Neighborhoods.Remove(neighborhood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = neighborhood.City.Id });
        }

        public async Task<IActionResult> DeleteLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Laboratory laboratory = await _context.Laboratories
                .Include(l => l.Neighborhood)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }

            return View(laboratory);
        }

        [HttpPost, ActionName("DeleteLaboratory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLaboratoryConfirmed(int id)
        {
            Laboratory laboratory = await _context.Laboratories
                .Include(l => l.Neighborhood)
                .FirstOrDefaultAsync(l => l.Id == id);
            _context.Laboratories.Remove(laboratory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsNeighborhood), new { Id = laboratory.Neighborhood.Id });
        }

    }
}

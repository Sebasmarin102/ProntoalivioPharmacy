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
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public ProductsController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper, 
            IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.MedicineType)
                .ToListAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> Create()
        {
            CreateProductViewModel model = new()
            {
                MedicineTypes = await _combosHelper.GetComboMedicineTypesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                }

                Product product = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                };

                product.ProductCategories = new List<ProductCategory>()
                {
                    new ProductCategory
                    {
                        MedicineType = await _context.MedicineTypes.FindAsync(model.MedicineTypeId)
                    }
                };

                if (imageId != Guid.Empty)
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                        new ProductImage { ImageId = imageId }
                    };
                }

                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro creado.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllProducts", _context.Products
                        .Include(p => p.ProductImages)
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.MedicineType).ToList())
                    });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un medicamento/producto con el mismo nombre.");
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

            model.MedicineTypes = await _combosHelper.GetComboMedicineTypesAsync();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            EditProductViewModel model = new()
            {
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Product product = await _context.Products.FindAsync(model.Id);
                product.Description = model.Description;
                product.Name = model.Name;
                product.Price = model.Price;
                product.Stock = model.Stock;
                _context.Update(product);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllProducts", _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.MedicineType).ToList())
                });

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un medicamento/producto con el mismo nombre.");
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

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", model) });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.MedicineType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddImage(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            AddProductImageViewModel model = new()
            {
                ProductId = product.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                Product product = await _context.Products.FindAsync(model.ProductId);
                ProductImage productImage = new()
                {
                    Product = product,
                    ImageId = imageId,
                };

                try
                {
                    _context.Add(productImage);
                    await _context.SaveChangesAsync();
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.MedicineType)
                    .FirstOrDefaultAsync(p => p.Id == model.ProductId))
                    });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddImage", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteImage(int id)
        {
            ProductImage productImage = await _context.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(productImage.ImageId, "products");
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { Id = productImage.Product.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddCategory(int id)
        {
            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.MedicineType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            List<MedicineType> medicinetypes = product.ProductCategories.Select(pc => new MedicineType
            {
                Id = pc.MedicineType.Id,
                Name = pc.MedicineType.Name,
            }).ToList();

            AddCategoryProductViewModel model = new()
            {
                ProductId = product.Id,
                MedicineTypes = await _combosHelper.GetComboMedicineTypesAsync(medicinetypes),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddCategoryProductViewModel model)
        {
            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.MedicineType)
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);
            if (ModelState.IsValid)
            {
                
                ProductCategory productCategory = new()
                {
                    MedicineType = await _context.MedicineTypes.FindAsync(model.MedicineTypeId),
                    Product = product,
                };

                try
                {
                    _context.Add(productCategory);
                    await _context.SaveChangesAsync();
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Products
                            .Include(p => p.ProductImages)
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.MedicineType)
                            .FirstOrDefaultAsync(p => p.Id == model.ProductId))
                    });

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            List<MedicineType> medicinetypes = product.ProductCategories.Select(pc => new MedicineType
            {
                Id = pc.MedicineType.Id,
                Name = pc.MedicineType.Name,
            }).ToList();

            model.MedicineTypes = await _combosHelper.GetComboMedicineTypesAsync(medicinetypes);
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddCategory", model) });

        }

        public async Task<IActionResult> DeleteMedicineType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductCategory productCategory = await _context.ProductCategories
                .Include(pc => pc.Product)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { Id = productCategory.Product.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            foreach (ProductImage productImage in product.ProductImages)
            {
                await _blobHelper.DeleteBlobAsync(productImage.ImageId, "products");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro eliminado.");
            return RedirectToAction(nameof(Index));
        }



    }
}

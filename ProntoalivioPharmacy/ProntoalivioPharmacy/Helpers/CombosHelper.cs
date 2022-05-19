using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data;

namespace ProntoalivioPharmacy.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync()
        {
            List<SelectListItem> list = await _context.Cities.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            })
                .OrderBy(m => m.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un tipo de Medicina...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboMedicineTypesAsync()
        {
            List<SelectListItem> list = await _context.MedicineTypes.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Id.ToString(),
            })
                .OrderBy(m => m.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un tipo de Medicina...]", Value = "0" });
            return list;
        }
    }
}

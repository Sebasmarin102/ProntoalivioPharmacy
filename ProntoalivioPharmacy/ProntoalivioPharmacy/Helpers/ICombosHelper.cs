using Microsoft.AspNetCore.Mvc.Rendering;
using ProntoalivioPharmacy.Data.Entities;

namespace ProntoalivioPharmacy.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboMedicineTypesAsync();
        Task<IEnumerable<SelectListItem>> GetComboMedicineTypesAsync(IEnumerable<MedicineType> filter);
        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync();
    }
}

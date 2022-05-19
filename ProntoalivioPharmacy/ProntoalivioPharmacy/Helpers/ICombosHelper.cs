using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProntoalivioPharmacy.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboMedicineTypesAsync();

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync();
    }
}

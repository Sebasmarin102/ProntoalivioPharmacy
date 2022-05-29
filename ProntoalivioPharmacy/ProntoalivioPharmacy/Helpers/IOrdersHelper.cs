using ProntoalivioPharmacy.Common;
using ProntoalivioPharmacy.Models;

namespace ProntoalivioPharmacy.Helpers
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
    }
}

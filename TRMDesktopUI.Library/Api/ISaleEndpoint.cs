using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSaleAsync(SaleModel sale);
    }
}
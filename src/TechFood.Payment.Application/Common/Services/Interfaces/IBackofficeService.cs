using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Dto.Product;

namespace TechFood.Payment.Application.Common.Services.Interfaces
{
    public interface IBackofficeService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

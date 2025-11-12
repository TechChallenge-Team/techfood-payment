using System;
using System.Threading.Tasks;
using TechFood.Application.Common.Data.Order;

namespace TechFood.Application.Common.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResult> GetByIdAsync(Guid orderId);
    }
}

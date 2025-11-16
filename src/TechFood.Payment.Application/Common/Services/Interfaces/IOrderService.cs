using System;
using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Data.Order;

namespace TechFood.Payment.Application.Common.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResult> GetByIdAsync(Guid orderId);
    }
}

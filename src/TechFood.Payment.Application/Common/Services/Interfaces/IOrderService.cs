using System;
using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Dto.Order;

namespace TechFood.Payment.Application.Common.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(Guid orderId);
    }
}

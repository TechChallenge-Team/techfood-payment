using System;
using System.Threading.Tasks;

namespace TechFood.Payment.Domain.Repositories;

public interface IPaymentRepository
{
    Task<Guid> AddAsync(Entities.Payment payment);

    Task<Entities.Payment?> GetByIdAsync(Guid id);
}

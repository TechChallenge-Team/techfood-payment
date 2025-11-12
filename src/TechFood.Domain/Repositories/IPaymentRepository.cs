using System;
using System.Threading.Tasks;
using TechFood.Domain.Entities;

namespace TechFood.Domain.Repositories;

public interface IPaymentRepository
{
    Task<Guid> AddAsync(Payment payment);

    Task<Payment?> GetByIdAsync(Guid id);
}

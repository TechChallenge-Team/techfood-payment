using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.Payment.Domain.Repositories;
using TechFood.Payment.Infra.Persistence.Contexts;

namespace TechFood.Payment.Infra.Persistence.Repositories;

public class PaymentRepository(PaymentContext dbContext) : IPaymentRepository
{
    private readonly DbSet<Domain.Entities.Payment> _payments = dbContext.Payments;

    public async Task<Guid> AddAsync(Domain.Entities.Payment payment)
    {
        var entry = await _payments.AddAsync(payment);

        return entry.Entity.Id;
    }

    public Task<Domain.Entities.Payment?> GetByIdAsync(Guid id)
    {
        return _payments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Domain.Entities.Payment?> GetByOrderIdAsync(Guid orderId)
    {
        return _payments.FirstOrDefaultAsync(x => x.OrderId == orderId);
    }
}

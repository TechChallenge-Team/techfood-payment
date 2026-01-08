using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Payment.Application.Common.Resources;
using TechFood.Payment.Application.Payments.Events.Integration.Outgoing;
using TechFood.Payment.Domain.Repositories;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.Payment.Application.Payments.Commands.ConfirmPayment;

public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Unit>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMediator _mediator;

    public ConfirmPaymentCommandHandler(
        IPaymentRepository repo,
        IMediator mediator
        )
    {
        _paymentRepository = repo;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.Id);

        if (payment == null)
            throw new ApplicationException(Exceptions.Payment_PaymentNotFound);

        payment.Confirm();

        await _mediator.Publish(new PaymentConfirmedEvent(payment.Id, payment.OrderId), cancellationToken);

        return Unit.Value;
    }
}

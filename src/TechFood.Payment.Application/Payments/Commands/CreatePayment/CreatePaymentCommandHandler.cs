using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechFood.Payment.Application.Common.Resources;
using TechFood.Payment.Application.Common.Services.Interfaces;
using TechFood.Payment.Application.Payments.Dto;
using TechFood.Payment.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler(
    IOrderService orderService,
    IBackofficeService productService,
    IPaymentRepository paymentRepository,
    IServiceProvider serviceProvider)
        : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await orderService.GetByIdAsync(request.OrderId);

        if (order == null)
        {
            throw new ApplicationException(Exceptions.Order_OrderNotFound);
        }

        var payment = new Domain.Entities.Payment(request.OrderId, request.Type, order.Amount);
        var products = await productService.GetAllAsync();

        var qrCodeData = string.Empty;
        var paymentService = serviceProvider.GetRequiredKeyedService<IPaymentService>(request.Type);

        if (request.Type == PaymentType.MercadoPago)
        {
            var paymentRequest = await paymentService.GenerateQrCodePaymentAsync(
                new(
                    "TOTEM01",
                    order.Id,
                    "TechFood - Order #" + order.Number,
                    order.Amount,
                    order.Items.ToList().ConvertAll(i => new Common.Dto.PaymentItem(
                        products.FirstOrDefault(p => p.Id == i.Id)?.Name ?? "",
                        i.Quantity,
                        "unit",
                        i.Price,
                        i.Price * i.Quantity))
                    ));

            qrCodeData = paymentRequest.QrCodeData;
        }
        else if (request.Type == PaymentType.CreditCard)
        {
            throw new NotImplementedException("Credit card payment is not implemented yet.");
        }

        await paymentRepository.AddAsync(payment);

        return new PaymentDto(
            payment.Id,
            payment.OrderId,
            payment.CreatedAt,
            payment.PaidAt,
            payment.Type,
            payment.Status,
            payment.Amount
        );
    }
}

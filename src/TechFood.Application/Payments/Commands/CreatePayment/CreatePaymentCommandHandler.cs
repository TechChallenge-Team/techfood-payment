using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechFood.Application.Common.Data;
using TechFood.Application.Common.Resources;
using TechFood.Application.Common.Services.Interfaces;
using TechFood.Application.Payments.Dto;
using TechFood.Domain.Entities;
using TechFood.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler(
    IOrderService orderService,
    IProductService productService,
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

        var payment = new Payment(request.OrderId, request.Type, order.Amount);
        var products = await productService.GetAllProducts();

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
                    order.Items.ToList().ConvertAll(i => new PaymentItem(
                        products.FirstOrDefault(p => p.Id == i.Id)?.Name ?? "",
                        i.Quantity,
                        "unit",
                        i.UnitPrice,
                        i.UnitPrice * i.Quantity))
                    ));

            qrCodeData = paymentRequest.QrCodeData;
        }
        else if (request.Type == PaymentType.CreditCard)
        {
            // TODO: Implement credit card payment
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

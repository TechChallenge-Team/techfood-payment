using System;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Payment.Application.Payments.Dto;

public record PaymentDto(Guid Id, Guid OrderId, DateTime CreatedAt, DateTime? PaidAt,
    PaymentType Type, PaymentStatusType Status, decimal Amount, string? QrCodeData = null);

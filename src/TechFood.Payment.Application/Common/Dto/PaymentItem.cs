namespace TechFood.Payment.Application.Common.Dto;

public record PaymentItem(string Title, int Quantity, string Unit, decimal UnitPrice, decimal Amount);

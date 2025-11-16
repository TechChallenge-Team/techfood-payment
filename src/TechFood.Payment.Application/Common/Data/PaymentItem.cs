namespace TechFood.Payment.Application.Common.Data;

public record PaymentItem(string Title, int Quantity, string Unit, decimal UnitPrice, decimal Amount);

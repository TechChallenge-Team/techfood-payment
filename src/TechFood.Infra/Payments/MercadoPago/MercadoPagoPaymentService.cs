using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TechFood.Application.Common.Data.QrCode;
using TechFood.Application.Common.Services.Interfaces;

namespace TechFood.Infra.Payments.MercadoPago;

internal class MercadoPagoPaymentService(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor httpContextAccessor) : IPaymentService
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(MercadoPagoOptions.ClientName);
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)
        },
    };

    public async Task<QrCodePaymentResult> GenerateQrCodePaymentAsync(QrCodePaymentRequest data)
    {
        var http = _httpContextAccessor.HttpContext!.Request;

        var response = await _client.PostAsJsonAsync("v1/orders",
            new OrderRequest(
                OrderType.QR,
                data.OrderId.ToString(),
                data.Title,
                data.Amount.ToString(CultureInfo.InvariantCulture),
                new(new OrderQRConfig(data.PosId, OrderQRConfigMode.Dynamic)),
                new OrderTransaction([new(data.Amount.ToString(CultureInfo.InvariantCulture))]),
                data.Items.ConvertAll(
                    i => new OrderItem(
                        i.Title,
                        i.Quantity,
                        i.Unit,
                        i.UnitPrice.ToString(CultureInfo.InvariantCulture)
                        ))
                ), _jsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            var errorResult = await response.Content.ReadFromJsonAsync<ErrorResult>();
            var error = errorResult?.Errors.FirstOrDefault();
            if (error != null)
            {
                throw new Exception($"Error {error.Code}: {error.Message}");
            }

            throw new Exception("An error occurred while generating the QR code payment.");
        }

        var result = await response.Content.ReadFromJsonAsync<OrderResult>(_jsonOptions);

        return new(
            result!.Id,
            result.TypeResponse.QrData);
    }
}

enum OrderType
{
    QR
}

enum OrderQRConfigMode
{
    Static,
    Dynamic
}

record OrderRequest(
    OrderType Type,
    string ExternalReference,
    string Description,
    string TotalAmount,
    OrderConfig Config,
    OrderTransaction Transactions,
    List<OrderItem> Items
    );

record OrderItem(
    string Title,
    int Quantity,
    string UnitMeasure,
    string UnitPrice
    );

record OrderTransaction(List<OrderPayment> Payments);

record OrderPayment(string Amount);

record OrderConfig(OrderQRConfig? Qr);

record OrderQRConfig(string ExternalPosId, OrderQRConfigMode Mode);

record OrderResult(string Id, OrderTypeResponse TypeResponse);

record OrderTypeResponse(string QrData);

record ErrorResult(List<ErrorData> Errors);

record ErrorData(
    string Code,
    string Message
    );

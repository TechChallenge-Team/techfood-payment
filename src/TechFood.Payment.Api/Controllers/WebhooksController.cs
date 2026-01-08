using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechFood.Payment.Application.Webhooks.Commands.ProcessMercadoPagoWebhook;
using TechFood.Payment.Contracts.Webhooks;
using TechFood.Payment.Infra.Payments.MercadoPago;

namespace TechFood.Payment.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("v1/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WebhooksController> _logger;
    private readonly string _secretKey;

    public WebhooksController(
        IMediator mediator,
        ILogger<WebhooksController> logger,
        IOptions<MercadoPagoOptions> mercadoPagoOptions)
    {
        _mediator = mediator;
        _logger = logger;
        _secretKey = mercadoPagoOptions.Value.WebhookSecret ?? string.Empty;
    }

    [HttpPost("mercadopago")]
    public async Task<IActionResult> InvokeAsync([FromBody] object serialObject)
    {
        var xSignature = HttpContext.Request.Headers["x-signature"].ToString();
        var xRequestId = HttpContext.Request.Headers["x-request-id"].ToString();

        // Se houver secretKey e houver assinatura, validar
        if (!string.IsNullOrEmpty(_secretKey) && !string.IsNullOrEmpty(xSignature))
        {
            var parts = xSignature.Split(',');

            string? ts = null;
            string? v1 = null;

            foreach (var part in parts)
            {
                var kv = part.Split('=');
                if (kv.Length == 2)
                {
                    if (kv[0].Trim() == "ts")
                        ts = kv[1].Trim();
                    else if (kv[0].Trim() == "v1")
                        v1 = kv[1].Trim();
                }
            }

            var dataId = HttpContext.Request.Query["data.id"].ToString();

            var manifest = $"id:{dataId};request-id:{xRequestId};ts:{ts};";

            var hash = ComputeHmacSha256(_secretKey, manifest);

            if (hash != v1)
            {
                _logger.LogWarning("Invalid webhook signature");
                return NotFound();
            }
        }
        else
        {
            _logger.LogWarning("Webhook signature validation skipped - no secret or no signature provided");
        }

        var request = JsonConvert.DeserializeObject<MercadoPagoWebhookRequest>(serialObject.ToString()!);

        if (request == null)
        {
            _logger.LogWarning("Failed to deserialize webhook request");
            return BadRequest();
        }

        _logger.LogInformation(
            "Received MercadoPago webhook. Action: {Action}, Type: {Type}, DataId: {DataId}",
            request.Action, request.Type, request.Data?.Id);

        var command = new ProcessMercadoPagoWebhookCommand(
            request.Action,
            request.Type,
            request.Data?.Id ?? string.Empty,
            request.UserId);

        await _mediator.Send(command);

        return Ok();
    }

    private static string ComputeHmacSha256(string key, string message)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);

        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}

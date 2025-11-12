using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechFood.Application.Payments.Commands.ConfirmPayment;
using TechFood.Application.Payments.Commands.CreatePayment;
using TechFood.Contracts.Payments;

namespace TechFood.Api.Controllers;

[ApiController()]
[Route("v1/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreatePaymentRequest request)
    {
        var command = new CreatePaymentCommand(
            request.OrderId,
            request.Type);

        var result = await _mediator.Send(command);

        return result != null
            ? Ok(result)
            : NotFound();
    }

    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> ConfirmAsync(Guid id)
    {
        var command = new ConfirmPaymentCommand(id);

        await _mediator.Send(command);

        return Ok();
    }
}

using CoWorkingAccess.Api.Dtos;
using CoWorkingAccess.Domain.Interfaces;
using CoWorkingAccess.Services.Payment;
using LiqPay.SDK.Dto;
using Microsoft.AspNetCore.Mvc;
using LiqPayResponse = CoWorkingAccess.Services.Payment.LiqPayResponse;

namespace CoWorkingAccess.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly LiqPayHelper _liqPayHelper;

    private readonly IOrderService _orderService;

    public PaymentController(IConfiguration configuration, IOrderService orderService)
    {
        var publicKey = configuration["LiqPay:PublicKey"];
        var privateKey = configuration["LiqPay:PrivateKey"];
        _liqPayHelper = new LiqPayHelper(publicKey, privateKey);
        _orderService = orderService;
    }

    [HttpPost("create")]
    public IActionResult CreatePayment([FromBody] CreatePaymentRequest request)
    {
        try
        {
            var paymentData = _liqPayHelper.GeneratePaymentData(
                request.Amount,
                request.Currency ?? "UAH",
                request.Description,
                request.OrderId
            );

            return Ok(new
            {
                data = paymentData.Data,
                signature = paymentData.Signature
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error creating payment");
        }
    }

    [HttpPost("callback")]
    public async Task<IActionResult> PaymentCallback([FromForm] LiqPayCallback callback)
    {
        try
        {
            if (!_liqPayHelper.VerifyCallback(callback.data, callback.signature))
            {
                return BadRequest("Invalid signature");
            }

            var response = _liqPayHelper.DecodeData<LiqPayResponse>(callback.data);
            var parseResult = int.TryParse(response.order_id, out var orderId);
            if (parseResult)
            {
                switch (response.status)
                {
                    case "success":
                        await _orderService.UpdateOrderStatus(orderId, "Paid");
                        break;
                    case "failure":
                    case "error":
                        await _orderService.UpdateOrderStatus(orderId, "Failed");
                        break;
                    case "sandbox":
                        await _orderService.UpdateOrderStatus(orderId, "TestPaid");
                        break;
                }
            }
            else
            {
                return StatusCode(500);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
}

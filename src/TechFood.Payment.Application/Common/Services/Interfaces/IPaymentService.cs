using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Dto.QrCode;

namespace TechFood.Payment.Application.Common.Services.Interfaces;

public interface IPaymentService
{
    Task<QrCodePaymentResult> GenerateQrCodePaymentAsync(QrCodePaymentRequest request);
}

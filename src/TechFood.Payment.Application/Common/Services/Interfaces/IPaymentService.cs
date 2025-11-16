using System.Threading.Tasks;
using TechFood.Payment.Application.Common.Data.QrCode;

namespace TechFood.Payment.Application.Common.Services.Interfaces;

public interface IPaymentService
{
    Task<QrCodePaymentResult> GenerateQrCodePaymentAsync(QrCodePaymentRequest request);
}

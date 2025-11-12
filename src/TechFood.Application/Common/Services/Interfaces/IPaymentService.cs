using System.Threading.Tasks;
using TechFood.Application.Common.Data.QrCode;

namespace TechFood.Application.Common.Services.Interfaces;

public interface IPaymentService
{
    Task<QrCodePaymentResult> GenerateQrCodePaymentAsync(QrCodePaymentRequest request);
}

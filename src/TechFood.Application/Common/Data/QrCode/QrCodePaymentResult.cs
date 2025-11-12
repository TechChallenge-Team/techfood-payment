namespace TechFood.Application.Common.Data.QrCode;

public class QrCodePaymentResult
{
    public string QrCodeId { get; set; }

    public string QrCodeData { get; set; }

    public QrCodePaymentResult(string qrCodeId, string qrCodeData)
    {
        QrCodeId = qrCodeId;
        QrCodeData = qrCodeData;
    }
}

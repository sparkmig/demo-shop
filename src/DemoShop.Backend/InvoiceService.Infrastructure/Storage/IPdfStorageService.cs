namespace InvoiceService.Infrastructure.Storage;

public interface IPdfStorageService
{
    Task<string> UploadPdfAsync(byte[] pdfContent, string fileName, CancellationToken cancellationToken = default);
    Task<byte[]> GetPdfAsync(string fileName, CancellationToken cancellationToken = default);
    Task<bool> DeletePdfAsync(string fileName, CancellationToken cancellationToken = default);
}

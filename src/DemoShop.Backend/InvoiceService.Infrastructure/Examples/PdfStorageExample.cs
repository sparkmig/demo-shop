using InvoiceService.Infrastructure.Storage;

namespace InvoiceService.Examples;

/// <summary>
/// Example showing how to use the PDF storage service
/// </summary>
public class PdfStorageExample
{
    private readonly IPdfStorageService _pdfStorageService;

    public PdfStorageExample(IPdfStorageService pdfStorageService)
    {
        _pdfStorageService = pdfStorageService;
    }

    public async Task UploadExample()
    {
        // Generate your PDF content (example using byte array)
        byte[] pdfContent = GetPdfContent();
        
        // Upload to Cloudflare R2
        string fileName = $"invoice-{Guid.NewGuid()}.pdf";
        string url = await _pdfStorageService.UploadPdfAsync(pdfContent, fileName);
        
        Console.WriteLine($"PDF uploaded successfully! URL: {url}");
    }

    public async Task DownloadExample()
    {
        // Download a PDF by filename
        string fileName = "invoice-12345.pdf";
        byte[] pdfContent = await _pdfStorageService.GetPdfAsync(fileName);
        
        // Save to local file or process the content
        await File.WriteAllBytesAsync($"downloaded-{fileName}", pdfContent);
        Console.WriteLine($"PDF downloaded successfully!");
    }

    public async Task DeleteExample()
    {
        // Delete a PDF by filename
        string fileName = "invoice-12345.pdf";
        bool success = await _pdfStorageService.DeletePdfAsync(fileName);
        
        Console.WriteLine(success ? "PDF deleted successfully!" : "Failed to delete PDF");
    }

    private byte[] GetPdfContent()
    {
        // This is just a placeholder
        // In real usage, this would be your generated PDF content
        return Array.Empty<byte>();
    }
}

# Cloudflare R2 PDF Storage

This infrastructure layer implements PDF storage using Cloudflare R2 buckets (S3-compatible).

## Quick Start - Local Development

For local development, we use **MinIO** (S3-compatible storage) running in Docker.

### 1. Start Local Infrastructure
```bash
# From the project root
docker-compose up -d
```

### 2. Create the Bucket
1. Open MinIO Console: http://localhost:9001
2. Login: `minioadmin` / `minioadmin`
3. Create bucket named: `invoices`

### 3. Run Your Application
The application is already configured for local development in `appsettings.Development.json`.
Just run the InvoiceService and it will automatically use MinIO!

**That's it!** Your PDFs will be stored locally in MinIO. ??

---

## Production Setup (Cloudflare R2)

### 1. Install Dependencies
The AWS S3 SDK is already added to the project dependencies.

### 2. Configure Cloudflare R2

Add the following configuration to your `appsettings.json` or User Secrets:

```json
{
  "CloudflareR2": {
    "AccountId": "your-cloudflare-account-id",
    "AccessKeyId": "your-r2-access-key-id",
    "SecretAccessKey": "your-r2-secret-access-key",
    "BucketName": "your-bucket-name",
    "PublicUrl": "https://your-custom-domain.com" // Optional: Use if you have a custom domain configured
  }
}
```

### 3. Get Cloudflare R2 Credentials

1. Log in to your Cloudflare dashboard
2. Go to R2 Object Storage
3. Create a bucket if you haven't already
4. Go to "Manage R2 API Tokens"
5. Create an API token with read/write permissions
6. Copy the Access Key ID, Secret Access Key, and Account ID

## Usage

### Inject the service into your handlers:

```csharp
public class CreateInvoicePDFHandler : ICommandHandler<CreateInvoicePDFCommand>
{
    private readonly IPdfStorageService _pdfStorageService;

    public CreateInvoicePDFHandler(IPdfStorageService pdfStorageService)
    {
        _pdfStorageService = pdfStorageService;
    }

    public async Task HandleAsync(CreateInvoicePDFCommand command)
    {
        // Generate PDF
        byte[] pdfContent = Document.Create(...).GeneratePdf();
        
        // Upload to Cloudflare R2
        string fileName = $"invoice-{command.OrderId}.pdf";
        string url = await _pdfStorageService.UploadPdfAsync(pdfContent, fileName);
        
        // The URL can be stored in your database or returned to the caller
        Console.WriteLine($"PDF uploaded to: {url}");
    }
}
```

### Available Methods

- **UploadPdfAsync**: Upload a PDF and get back the URL
- **GetPdfAsync**: Download a PDF by filename
- **DeletePdfAsync**: Delete a PDF by filename

## Security Note

**Never commit your API credentials to source control!** Use one of these methods:

1. **User Secrets** (Development):
   ```bash
   dotnet user-secrets set "CloudflareR2:AccountId" "your-account-id"
   dotnet user-secrets set "CloudflareR2:AccessKeyId" "your-access-key"
   dotnet user-secrets set "CloudflareR2:SecretAccessKey" "your-secret-key"
   dotnet user-secrets set "CloudflareR2:BucketName" "your-bucket-name"
   ```

2. **Environment Variables** (Production)
3. **Azure Key Vault** or other secret management solutions

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Options;

namespace InvoiceService.Infrastructure.Storage;

public class CloudflarePdfStorageService : IPdfStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly CloudflareR2Options _options;

    public CloudflarePdfStorageService(IOptions<CloudflareR2Options> options)
    {
        _options = options.Value;

        var credentials = new BasicAWSCredentials(_options.AccessKeyId, _options.SecretAccessKey);
        
        var serviceUrl = _options.AccountId == "local" || _options.PublicUrl?.StartsWith("http://localhost") == true
            ? "http://localhost:9000"
            : $"https://{_options.AccountId}.r2.cloudflarestorage.com";
        
        var config = new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<string> UploadPdfAsync(byte[] pdfContent, string fileName, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(pdfContent);

        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = fileName,
            InputStream = memoryStream,
            ContentType = "application/pdf"
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        return $"{_options.PublicUrl}/{fileName}";
    }

    public async Task<byte[]> GetPdfAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var request = new GetObjectRequest
        {
            BucketName = _options.BucketName,
            Key = fileName
        };

        using var response = await _s3Client.GetObjectAsync(request, cancellationToken);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);

        return memoryStream.ToArray();
    }

    public async Task<bool> DeletePdfAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(request, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

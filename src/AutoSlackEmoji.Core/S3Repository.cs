using System.Net;
using Amazon.S3;
using Amazon.S3.Model;

namespace AutoSlackEmoji.Core;

public interface IFileRepository
{
    Task<string> AddFileAsync(Stream dataStream, string fileName, string backetName);
}

public class S3Repository : IFileRepository
{
    private readonly IAmazonS3 _s3Client;

    public S3Repository(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<string> AddFileAsync(Stream dataStream, string fileName, string backetName)
    {
        var request = new PutObjectRequest()
        {
            BucketName = backetName,
            Key = fileName,
            InputStream = dataStream
        };

        var response = await _s3Client.PutObjectAsync(request);
        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            var presignedUrl = GenerateShareObjectPreSignedUrl(backetName, fileName);
            return presignedUrl;
        }

        return string.Empty;
    }

    private string GenerateShareObjectPreSignedUrl(string backetName, string fileName)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = backetName,
            Key = fileName,
            Expires = DateTime.Now.AddMinutes(5)
        };

        var result = _s3Client.GetPreSignedURL(request);
        return result;
    }
}
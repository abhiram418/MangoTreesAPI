using Amazon.S3;
using Amazon.S3.Model;
using MangoTreesAPI.Models;
using Microsoft.Extensions.Options;

namespace MangoTreesAPI.Services
{
    public class AwsS3BucketService
    {
        private readonly IAmazonS3 amazonS3;
        private readonly AwsDataOptions awsDataOptions;
        public AwsS3BucketService(IAmazonS3 _amazonS3, IOptions<AwsDataOptions> _awsOptions)
        {
            amazonS3 = _amazonS3;
            awsDataOptions = _awsOptions.Value;
        }

        public async Task<string[]> PostTheImagesAndGetUrls(string folderName, IFormFile[] images)
        {
            List<string> imageUrls = new List<string>();

            for (int index = 0; index < images.Length; index++)
            {
                string key = $"{folderName}/{index}";
                string imageUrl = await PostImagesToS3WithRetry(key, images[index]);
                imageUrls.Add(imageUrl);
            }

            return imageUrls.ToArray();
        }

        private async Task<string> PostImagesToS3WithRetry(string key, IFormFile image)
        {
            var retryCount = 3;

            PostImage:
            try
            {
                return await PostImagesToS3(key, image);
            }
            catch (Exception) {
                if (retryCount == 1)
                {
                    throw new Exception("Communication error: Failed to save session after multiple attempts.");
                }
                else
                {
                    retryCount--;
                    await Task.Delay(1000);
                    goto PostImage;
                }
            }
            
        }

        private async Task<string> PostImagesToS3(string key, IFormFile image)
        {
            var objectRequest = new PutObjectRequest()
            {
                BucketName = awsDataOptions.BucketName.ToString(),
                Key = key,
                InputStream = image.OpenReadStream(),
                ContentType = image.ContentType
            };
            var responce = await amazonS3.PutObjectAsync(objectRequest);
            var imageUrl = FormatAwsS3ImageUrl(key);

            return imageUrl;
        }

        private string FormatAwsS3ImageUrl(string key)
        {
            return $"https://{awsDataOptions.BucketName}.s3.{awsDataOptions.Region}.amazonaws.com/{key}";
        }
    }
}

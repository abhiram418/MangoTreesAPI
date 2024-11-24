using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.S3;
using MangoTreesAPI.Models;

namespace MangoTreesAPI.Middleware
{
    public static class AWSMiddleware
    {
        public static IServiceCollection ConfigureAWS(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AwsDataOptions>(options =>
            {
                options.BucketName = Environment.GetEnvironmentVariable("AWS_S3_BucketName") ?? configuration["AWS:BucketName"] ?? "";
                options.Region = configuration["AWS:Region"] ?? "";
            });

            //Adding Aws Lambda hosting
            services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

            //Adding Aws DynamoDb (V2)
            var awsOptions = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();

            //Adding Aws S3 bucket 
            services.AddAWSService<IAmazonS3>();

            return services;
        }
    }
}

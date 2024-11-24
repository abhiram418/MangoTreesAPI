using MangoTreesAPI.Services;

namespace MangoTreesAPI.Middleware
{
    public static class ServiceMiddleware
    {
        public static IServiceCollection ConfigureCustomServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<AwsS3BucketService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<GeneralService>();
            services.AddScoped<OtpService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ManagementService>();

            return services;
        }
    }
}

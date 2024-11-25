namespace MangoTreesAPI.Middleware
{
    public static class DeveloperMiddleware
    {
        public static IServiceCollection ConfigureDeveloperTools(this IServiceCollection services)
        {
            // Detailed return when problem comes
            services.AddProblemDetails();

            // To access the data from the HttpContext/Tockens
            services.AddHttpContextAccessor();

            // Adds the data mapper
            services.AddAutoMapper(typeof(Program));

            // Adds Cors access to the bellow sites
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMangoTreesUI", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://www.mangotrees.in"); // Allow all origins (you can restrict to specific domains)
                    policy.AllowAnyMethod(); // Allow any HTTP methods (GET, POST, PUT, DELETE, etc.)
                    policy.AllowAnyHeader(); // Allow any headers
                });
            });

            return services;
        }
    }
}

namespace UniCore.API.Extensions
{
    public static class CorsExtension
    {
        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(
                                "http://172.29.50.21:5174",
                                "https://172.29.50.21:5174",
                                "http://172.29.50.28:5173",
                                "https://172.29.50.28:5173",
                                "http://172.29.50.21:5173",
                                "https://172.29.50.21:5173",
                                "http://localhost:5173",
                                "https://localhost:5173",
                                "http://localhost:5174",
                                "https://localhost:5174",
                                "http://localhost:5290",
                                "https://localhost:5290",
                                "http://0.0.0.0:5290",
                                "https://0.0.0.0:5290",
                                "http://172.29.50.31:5290",
                                "https://172.29.50.31:5290")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .SetPreflightMaxAge(TimeSpan.FromDays(1.0));
                });
            });
        }

        public static void ConfigureCors(this WebApplication app, IWebHostEnvironment env)
        {
            app.UseCors();
        }
    }
}

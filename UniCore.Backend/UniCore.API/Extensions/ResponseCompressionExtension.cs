namespace UniCore.API.Extensions
{
    public static class ResponseCompressionExtension
    {
        public static void AddResponseCompressionService(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
        }

        public static void ConfigureResponseCompression(this WebApplication app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
        }
    }
}

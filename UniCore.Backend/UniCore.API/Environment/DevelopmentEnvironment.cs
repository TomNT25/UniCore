using UniCore.API.Middlewares.CustomMiddlewares;

namespace UniCore.API.Environment
{
    public static class DevelopmentEnvironment
    {
        public static void UseDevelopmentSwagger(this WebApplication app)
        {
            // app.UseMiddleware<DebugContextMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
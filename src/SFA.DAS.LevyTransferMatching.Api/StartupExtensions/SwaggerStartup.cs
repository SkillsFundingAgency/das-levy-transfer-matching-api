using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.LevyTransferMatching.Api.StartupExtensions
{
    public static class SwaggerStartup
    {
        public static IApplicationBuilder AddSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Levy Transfers Matching API V1");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}

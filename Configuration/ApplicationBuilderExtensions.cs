namespace TerracoDaCida.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder app)
            => app.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
}

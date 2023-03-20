using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder
                    .ConfigureMvc()
                    .ConfigureNLog()
                    .ConfigureServices()
                    .ConfigureMapster()
                    .ConfigureFluentValidation();

            };

            var app = builder.Build();
            {
                app.UseRequestPipeline();
                app.UseBlogRoutes();
                app.UseDataSeeder();
            }

            app.Run();
        }
    }
}
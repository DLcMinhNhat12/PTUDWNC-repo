using Mapster;
using MapsterMapper;
using TatBlog.WebApp.Extensions;

namespace TatBlog.WebApp.Mapsters;
// Định nghĩa phương thức
// ConfigureMapster để thêm các dịch vụ cần thiết của Mapster vào DI Container.
public static class MapsterDependencyInjection
{
    public static WebApplicationBuilder ConfigureMapster(
        this WebApplicationBuilder builder)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MapsterConfiguration).Assembly);

        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IMapper, ServiceMapper>();

        return builder;
    }
}

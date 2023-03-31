namespace TatBlog.WebApp.Middlewares
{
    // Lưu lại quá trình di chuyển giữa các trang của người dùng
    public class UserActivityMiddleware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger<UserActivityMiddleware> _Logger;

        public UserActivityMiddleware(
            RequestDelegate next,
            ILogger<UserActivityMiddleware> logger)
        {
            _next = next;
            _Logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _Logger.LogInformation(
                "{Time:yyy-MM-dd MM:mm:ss} - IP{IpAdress} - Path: {Url}",
                DateTime.Now,
                context.Connection.RemoteIpAddress?.ToString(),
                context.Request.Path);

            await _next(context);
        }
    }
}
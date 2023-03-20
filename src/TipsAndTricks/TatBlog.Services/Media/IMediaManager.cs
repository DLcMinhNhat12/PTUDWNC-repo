
// Tính năng upload hình ảnh
namespace TatBlog.Services.Media;

public interface IMediaManager
{
    Task<string> SaveFileAsync(
        Stream buffer,
        string originalFineName,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(
        string filePath,
        CancellationToken cancellationToken = default);
}

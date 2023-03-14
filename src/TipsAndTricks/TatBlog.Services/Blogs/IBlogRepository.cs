using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;


namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        // tìm bài viết có tên định danh là slug
        // và đăng vào tháng, năm?

        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

        Task<bool> IsPostSlugExitsedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);

        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool ShowOnMenu = false,
            CancellationToken cancellationToken = default);

        // Lấy danh sách từ khóa/ thẻ và phân theo thamso
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 	// Phần C
        // Lấy định danh (slug) từ 1 thể tag
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tag> GetTagsAsync(
            string slug,
            CancellationToken cancellationToken = default);

        // Lấy danh sách tất cả các tag + số bài viết chứa bài đó
        Task<IList<TagItem>> GetAllTagsList(
            CancellationToken cancellationToken = default);

        // Xóa 1 tag theo mã
        Task<Tag> RemoveTagsByIdAsync(int removeTag, CancellationToken cancellation = default);

        // Method tìm kiếm phân trang theo các bài viết
        Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery condition,
            int pageNumber = 1, int pageSize = 10,
            CancellationToken cancellationToken = default);
    }
}






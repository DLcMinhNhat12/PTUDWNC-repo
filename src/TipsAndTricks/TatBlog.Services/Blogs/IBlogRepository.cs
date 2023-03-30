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

        Task<IList<Author>> GetPopularAuthorsAsync(int numAuthor,
           CancellationToken cancellationToken = default);


        Task<bool> IsPostSlugExistedAsync(
        int postId, string slug,
        CancellationToken cancellationToken = default);

        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool ShowOnMenu = false,
            CancellationToken cancellationToken = default);

        Task<IList<AuthorItem>> GetAuthorsAsync(
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

        Task<IList<Post>> GetRandomArticlesAsync(
             int numPosts, CancellationToken cancellationToken = default);

        // Lấy danh sách tất cả các tag + số bài viết chứa bài đó
        Task<IList<TagItem>> GetAllTagsList(
            CancellationToken cancellationToken = default);

        // Xóa 1 tag theo mã
        Task<Tag> RemoveTagsByIdAsync(int removeTag, CancellationToken cancellation = default);
        // Xóa 1 author theo mã
        Task<Author> RemoveAuthorsByIdAsync(int authorId, CancellationToken cancellation = default);


        //Task DeletePostByIdAsync(
        //       int id, CancellationToken cancellationToken = default);

        Task<Post> RemovePostsByIdAsync(
          int posstId, CancellationToken cancellationToken = default);
        
        Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(int numMonths,
        CancellationToken cancellationToken = default);

        // Method tìm kiếm phân trang theo các bài viết
        Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery condition,
            int pageNumber = 1, int pageSize = 10,
            CancellationToken cancellationToken = default);

        // Tìm kiếm phân trang theo tác giả
        Task<IPagedList<Author>> GetPagedAuthorsAsync(
           int pageNumber = 1, int pageSize = 10,
           CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(int postId, bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<Author> GetAuthorByIdAsync(int postId,
            CancellationToken cancellationToken = default);

        // Chức năng tìm Tìm kiếm
        Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<bool> TogglePublishedFlagAsync(
        int postId, CancellationToken cancellationToken = default);

        Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default);

        Task<Author> CreateOrUpdateAuthorAsync(
        Author author, CancellationToken cancellationToken = default);
    }
}






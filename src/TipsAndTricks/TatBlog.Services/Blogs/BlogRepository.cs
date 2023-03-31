using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        // Lấy ds chuyên mục và số lượng bài viết
        public async Task<IList<CategoryItem>> GetCategoriesAsync(
            bool ShowOnMenu = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (ShowOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == month);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == slug);
            }

            return await postQuery.FirstOrDefaultAsync(cancellationToken);
        }

        // Tìm bài N bài vết nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        // TOP tác giả có nhiều bài viết nhất
        public async Task<IList<Author>> GetPopularAuthorsAsync(int numAuthor,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .OrderByDescending(p => p.Posts)
                .Take(numAuthor)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        public async Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p =>
                p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);
        }

        //Lấy ds tags và phân trang theo tham số
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<Tag> GetTagsAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(t => t.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // Hiển thị TOP 5 bài viết ngẫu nhiên. Khi người dùng click "Xem chi tiết".
        public async Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }


        public async Task<IList<TagItem>> GetAllTagsList(CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToListAsync(cancellationToken);
        }

        public async Task<Tag> RemoveTagsByIdAsync(
            int removeTag, CancellationToken cancellationToken = default)
        {
            var tag = await _context.Set<Tag>()
                .FirstOrDefaultAsync(t => t.Id == removeTag, cancellationToken);

            if (tag != null)
            {
                _context.Set<Tag>().Remove(tag);
                await _context.SaveChangesAsync(cancellationToken);
                Console.WriteLine("Đã xóa tag!");
            }
            else
            {
                Console.WriteLine("ID không tồn tại!");
            }
            return tag;
        }

        public async Task<bool> DeletePostByIdAsync(
             int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
             .Where(x => x.Id == postId)
             .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<Author> RemoveAuthorsByIdAsync(
            int authorId, CancellationToken cancellationToken = default)
        {
            var author = await _context.Set<Author>()
                .FirstOrDefaultAsync(t => t.Id == authorId, cancellationToken);

            if (author != null)
            {
                _context.Set<Author>().Remove(author);
                await _context.SaveChangesAsync(cancellationToken);
                Console.WriteLine("Đã xóa tác giả!");
            }
            else
            {
                Console.WriteLine("Tác giả không tồn tại!");
            }
            return author;
        }

        public async Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(int numMonths,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(x => x.Published)
                .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
                .Select(g => new MonthlyPostCountItem()
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    PostCount = g.Count(x => x.Published)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync(cancellationToken);
        }

        // Câu 1s
        public async Task<IPagedList<Post>> GetPagedPostsAsync(
           PostQuery condition,
           int pageNumber = 1,
           int pageSize = 10,
           CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(PostQuery condition, IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper)
        {
            var posts = FilterPosts(condition);

            return await mapper(posts)
                .ToPagedListAsync(pagingParams);
        }    


        // Thay đổi trạng thái của bài viết
        public async Task<bool> TogglePublishedFlagAsync(
        int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(postId);

            if (post is null) return false;

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);

            return post.Published;
        }

        private IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly == true)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished == false)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;

            //// Compact version
            //return _context.Set<Post>()
            //	.Include(x => x.Category)
            //	.Include(x => x.Author)
            //	.Include(x => x.Tags)
            //	.WhereIf(condition.PublishedOnly, x => x.Published)
            //	.WhereIf(condition.NotPublished, x => !x.Published)
            //	.WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug), x => x.Category.UrlSlug == condition.CategorySlug)
            //	.WhereIf(condition.AuthorId > 0, x => x.AuthorId == condition.AuthorId)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), x => x.Author.UrlSlug == condition.AuthorSlug)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug), x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug))
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), x => x.Title.Contains(condition.Keyword) ||
            //	                                                             x.ShortDescription.Contains(condition.Keyword) ||
            //	                                                             x.Description.Contains(condition.Keyword) ||
            //	                                                             x.Category.Name.Contains(condition.Keyword) ||
            //	                                                             x.Tags.Any(t => t.Name.Contains(condition.Keyword)))
            //	.WhereIf(condition.Year > 0, x => x.PostedDate.Year == condition.Year)
            //	.WhereIf(condition.Month > 0, x => x.PostedDate.Month == condition.Month)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TitleSlug), x => x.UrlSlug == condition.TitleSlug);
        }

        // Tìm kiếm chuyên mục
        public async Task<Category> FindCategoryBySlugAsync(string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FirstOrDefaultAsync(c => c.UrlSlug == slug, cancellationToken);
        }


        // Tạo mới hoặc update bài viết
        public async Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagsAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }

        // Tạo mới hoặc update tác giả
        public async Task<Author> CreateOrUpdateAuthorAsync(Author author,
            CancellationToken cancellationToken = default)
        {
            if (author.Id > 0)
            {
                _context.Set<Author>().Update(author);
            }
            else
            {
                _context.Set<Author>().Add(author);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return author;
        }
    }
}

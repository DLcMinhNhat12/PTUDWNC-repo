using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Core.Contracts;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        //public IActionResult Index()
        //{
        //	ViewBag.Currentime = DateTime.Now.ToString("HH:mm:ss");
        //	return View();
        //}

        // Action này xử lý HTTP request đến trang chủ của
        // ứng dụng web hoặc tìm kiếm bài viết theo từ khóa
        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 4)
        {
            //Tạo oject chứa điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                //Chỉ lấy những bài viết Published
                PublishedOnly = true,
                Keyword = keyword
            };

            // Truy vấn các bài viết theo điều kiện đã tạo
            var postList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            // Lưu lại đk truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;

            return View(postList);
        }

        public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> Category(
        string slug,
        [FromQuery(Name = "p")] int pageNumber = 1,
        [FromQuery(Name = "ps")] int pageSize = 4)
        {

            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true,
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            var categorys = await _blogRepository
                            .FindCategoryBySlugAsync(slug);

            ViewBag.NameCat = categorys.Name;
            ViewBag.PostQuery = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Author(
           string slug,
           [FromQuery(Name = "p")] int pageNumber = 1,
           [FromQuery(Name = "ps")] int pageSize = 3)
        {

            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                PublishedOnly = true,
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;

            var author = await _blogRepository.FindAuthorBySlugAsync(slug);

            ViewBag.Author = author.FullName;

            return View(posts);
        }

        public async Task<IActionResult> Tag(
           string slug,
           [FromQuery(Name = "p")] int pageNumber = 1,
           [FromQuery(Name = "ps")] int pageSize = 3)
        {

            var postQuery = new PostQuery()
            {
                TagSlug = slug,
                PublishedOnly = true,
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;

            var tag = await _blogRepository.GetTagsAsync(slug);

            ViewBag.Tag = tag.Name;

            return View(posts);
        }

        public async Task<IActionResult> Post(
                string slug,
                int year,
                int month,
                int day)
        {

            var post = await _blogRepository
                .GetPostAsync(year, month, slug);
            //await _blogRepository.IncreaseViewCountAsync(post.Id);

            return View(post);
        }

    }
}

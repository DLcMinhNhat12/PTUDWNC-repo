using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Core.Contracts;
using Microsoft.VisualBasic;

namespace TatBlog.WebApp.Controllers
{
	public class BlogController : Controller
	{
		private readonly IBlogRepository _blogRepository;

		public BlogController(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		// Action này xử lý HTTP request đến trang chủ của
		// ứng dụng web hoặc tìm kiếm bài viết theo từ khóa
		public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 5)
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

		public async Task<IActionResult> Category(string slug)
		{
			var postQuery = new PostQuery()
			{
				CategorySlug = slug
			};

			var categoriesList = await _blogRepository.GetPagedPostsAsync(postQuery);
			ViewBag.PostQuery = postQuery;
			return View("Index", categoriesList);
		}

        public async Task<IActionResult> Author(string slug)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug
            };

            var authors = await _blogRepository.GetPagedPostsAsync(postQuery);
            ViewBag.PostQuery = postQuery;
            return View("Index", authors);
        }

        public async Task<IActionResult> Tag(string slug)
        {
            var postQuery = new PostQuery()
            {
                TagSlug = slug
            };

            var tags = await _blogRepository.GetPagedPostsAsync(postQuery);
            ViewBag.PostQuery = postQuery;
            return View("Index", tags);
        }

		// Hiển thị chi tiết 1 bài viết khi click button "Xem chi tiết"
        public async Task<IActionResult> Post(int year, int month, 
			int day,string slug)
        {
			var postQuery = new PostQuery()
			{
				Year = year,
				Month = month,
				Day = day,
                TagSlug = slug
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery);
            ViewBag.PostQuery = postQuery;
            return View("Index", posts);
        }

		// Hiển thị danh sách bài viết được đăng theo năm và quý do người dùng chọn
        public async Task<IActionResult> Archives(int year, int month,
			[FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name ="ps")] int pageSize = 5)
        {
            var postQuery = new PostQuery()
            {
                Year = year,
                Month = month
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery);
			ViewData["PostQuery"] = postQuery;
            return View("Index", posts);
        }

		// Hiển thị trang giới thiệu Blog content tĩnh
        public IActionResult About()
			=> View();
		// Hiển thị thông tin liên hệ, bản đồ, form gửi thông tin ý kiến
		public IActionResult Contact()
			=> View();
		
		public IActionResult Rss()
			=> Content("Nội dung sẽ được cập nhật");

		public IActionResult Error()
		{
			return View();
		}
    }
}

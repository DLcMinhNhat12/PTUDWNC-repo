using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    //TOP 4 tác giả có nhiều bài viết nhất
    public class BestAuthors : ViewComponent
    {

        private readonly IBlogRepository _blogRepository;

        public BestAuthors(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authors = await _blogRepository.GetPopularAuthorsAsync(4);

            return View(authors);
        }
    }
}

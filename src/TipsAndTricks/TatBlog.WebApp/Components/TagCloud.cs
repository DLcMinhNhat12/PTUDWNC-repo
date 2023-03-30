using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    // Hiển thị danh sách thẻ tag
    public class TagCloud : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public TagCloud(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _blogRepository.GetAllTagsList();

            return View(tags);
        }
    }
}

using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models
{
    public class PostDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string UrlSlug { get; set; }
        public string ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public bool Published { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CategoryDTO CategoryName { get; set; }
        // Tác giả của bài viết
        public AuthorDTO AuthorName { get; set; }
        //Danh sách từ khóa bài viết
        public IList<TagDTO> Tags { get; set; }
    }
}

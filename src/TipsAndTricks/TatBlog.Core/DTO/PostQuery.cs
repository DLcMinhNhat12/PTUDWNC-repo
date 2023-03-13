using TatBlog.Core.Contracts;

namespace TatBlog.Services.Blogs
{
    // PostQuery lưu trữ các điều kiện tìm kiếm bài viết
    public class PostQuery
    {
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; } 
        public string TitleSlug { get; set; } 
        public int? Month { get; set; }
        public int? Year { get; set; }
        public bool? PublishedOnly { get; set; }
        public bool? NotPublished { get; set; }


        public string Keyword { get; set; } 
    }
}

using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
    public class PostItem
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
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }
        public int TagCount { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public IList<string> Tags { get; set; }
    }
}
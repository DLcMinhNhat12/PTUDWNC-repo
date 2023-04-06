using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        public PostValidator(IBlogRepository _blogRepository)
        {
            RuleFor(a => a.ShortDescription)
              .NotEmpty()
              .WithMessage("Phần giới thiệu không được để trống")
              .MaximumLength(2000)
              .WithMessage("Mô tả tối đa 2000 ký tự");

            RuleFor(a => a.Description)
              .NotEmpty()
              .WithMessage("Phần nội dung không được để trống")
              .MaximumLength(5000)
              .WithMessage("Nội dung tối đa 5000 ký tự");

            RuleFor(a => a.UrlSlug)
              .MustAsync(async (postModel, slug, cancellationToken) =>
                  !await _blogRepository.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
              .WithMessage("Slug '{PropertyValue}' đã được sử dụng");
        }
    }
}

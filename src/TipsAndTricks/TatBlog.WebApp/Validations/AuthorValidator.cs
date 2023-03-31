using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class AuthorValidator : AbstractValidator<AuthorEditModel>
    {
        private readonly IBlogRepository _blogRepository;
        public AuthorValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Tên tác giả không được để trống")
                .MaximumLength(200)
                .WithMessage("Tên tác giả tối đa 200 ký tự");

            RuleFor(p => p.UrlSlug)
               .NotEmpty()
               .WithMessage("Slug không được bỏ trống")
               .MaximumLength(1000)
               .WithMessage("Slug không được nhiều hơn 1000 ký tự");

            RuleFor(p => p.UrlSlug)
               .MustAsync(async (postModel, slug, cancellationToken) =>
                   !await _blogRepository.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
               .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email của tác giả không được để trống");
        }
    }
}

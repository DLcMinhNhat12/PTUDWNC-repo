using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class AuthorValidatior : AbstractValidator<AuthorEditModel>
    {
        public AuthorValidatior(IAuthorRepository _authorRepository)
        {
            RuleFor(a => a.FullName)
               .NotEmpty()
               .WithMessage("Tên tác giả không được để trống")
               .MaximumLength(200)
               .WithMessage("Tên tác giả tối đa 200 ký tự");

            RuleFor(a => a.UrlSlug)
               .NotEmpty()
               .WithMessage("Slug không được bỏ trống")
               .MaximumLength(1000)
               .WithMessage("UrlSlug tối đa 1000 ký tự");

            RuleFor(a => a.UrlSlug)
               .MustAsync(async (postModel, slug, cancellationToken) =>
                   !await _authorRepository.IsAuthorSlugExistedAsync(postModel.Id, slug, cancellationToken))
               .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(a => a.JoinedDate)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Ngày tham gia không hợp lệ");


            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email của không được để trống")
                .MaximumLength(100)
                .WithMessage("Email tối đa 100 ký tự");

            RuleFor(a => a.Notes)
                .MaximumLength(500)
                .WithMessage("Ghi chú tối đa 500 ký tự");
        }
    }
}

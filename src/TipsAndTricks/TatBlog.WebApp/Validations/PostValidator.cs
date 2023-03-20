using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        private readonly IBlogRepository _blogRepo;

        public PostValidator(IBlogRepository blogRepo)
        {
            _blogRepo = blogRepo;

            RuleFor(post => post.Title)
                .NotEmpty().WithMessage("Chủ đề không được bỏ trống")
                .MaximumLength(500).WithMessage("Chủ đề không được nhiều hơn 500 ký tự");

            RuleFor(p => p.ShortDescription)
                .NotEmpty()
                .WithMessage("Giới thiệu không được bỏ trống"); ;

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Nội dung không được bỏ trống"); ;

            RuleFor(p => p.Meta)
                .NotEmpty()
                .WithMessage("Metadata không được bỏ trống")
                .MaximumLength(1000)
                .WithMessage("Metadata không được nhiều hơn 1000 ký tự");

            RuleFor(p => p.UrlSlug)
                .NotEmpty()
                .WithMessage("Slug không được bỏ trống")
                .MaximumLength(1000)
                .WithMessage("Slug không được nhiều hơn 1000 ký tự");

            RuleFor(p => p.UrlSlug)
                .MustAsync(async (postModel, slug, cancellationToken) =>
                    !await _blogRepo.IsPostSlugExitsedAsync(postModel.Id, slug, cancellationToken))
                .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(p => p.CategoryId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(p => p.AuthorId)
                .NotEmpty()
                .WithMessage("Bạn phải chọn tác giả bài viết");

            RuleFor(p => p.SelectedTags)
                .Must(HasAtLeastOneTag)
                .WithMessage("Bạn phải nhập ít nhất một thẻ");

            When(p => p.Id <= 0, () =>
            {
                RuleFor(p => p.ImageFile)
                    .Must(p => p is { Length: > 0 })
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            }).Otherwise(() =>
            {
                RuleFor(p => p.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            });
        }

        private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags)
        {
            return postModel.GetSelectedTags().Any();
        }

        private async Task<bool> SetImageIfNotExist(
            PostEditModel postModel,
            IFormFile imageFile,
            CancellationToken cancellationToken)
        {
            var post = await _blogRepo.GetPostByIdAsync(postModel.Id, false, cancellationToken);
            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
            {
                return true;
            }

            return imageFile is { Length: > 0 };
        }

    }
}

using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class AuthorsController : Controller
{
    private readonly IBlogRepository _blogRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    private readonly IMediaManager _mediaManager;

    public AuthorsController(
       IBlogRepository blogRepository,
       IAuthorRepository authorRepository,
       IMediaManager mediaManager,
       IMapper mapper)
    {
        _blogRepository = blogRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
        _mediaManager = mediaManager;
    }

    public async Task<IActionResult> Index(AuthorFilterModel model,
       [FromQuery(Name = "p")] int pageNum = 1,
       [FromQuery(Name = "ps")] int pageSize = 5)
    {
        var pagingParams = new PagingParams()
        {
            PageNumber = pageNum,
            PageSize = pageSize
        };
        var authorQuery  = _mapper.Map<AuthorQuery>(model);

        ViewBag.AuthorsList = await _authorRepository
            .GetPagedAuthorsAsync(pagingParams, authorQuery.Keyword);

        //await PopulatePostFilterModelAsync(model);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id = 0)
    {
        var author = id > 0
            ? await _authorRepository.GetAuthorByIdAsync(id)
            : null;

        var model = author == null
            ? new AuthorEditModel()
            : _mapper.Map<AuthorEditModel>(author);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
        [FromServices] IValidator<AuthorEditModel> authorValidator,
        AuthorEditModel model)
    {
        var validationResult = await authorValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var author = model.Id > 0
            ? await _authorRepository.GetAuthorByIdAsync(model.Id) : null;


        if (author == null)
        {
            author = _mapper.Map<Author>(model);
            author.Id = 0;
            author.JoinedDate = DateTime.Now;
        }
        else
        {
            _mapper.Map(model, author);
        }

        // Nếu người dùng có upload hình ảnh minh họa cho bài viết
        if (model.ImageFile?.Length > 0)
        {
            // Thì thực hiện lưu vào thư mục uploads
            var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                model.ImageFile.FileName, model.ImageFile.ContentType);

            // Nếu thành công, xóa hình ảnh cũ nếu có
            if (!string.IsNullOrEmpty(newImagePath))
            {
                await _mediaManager.DeleteFileAsync(author.ImageUrl);
                author.ImageUrl = newImagePath;
            }
        }

        await _blogRepository.CreateOrUpdateAuthorAsync(author);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteAuthor(int id)
    {
        await _blogRepository.RemoveAuthorsByIdAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

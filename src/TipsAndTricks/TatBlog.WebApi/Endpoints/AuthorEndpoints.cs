﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<PaginationResult<AuthorItem>>();

            // Lấy thông tin 1 tác giả
            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
               .WithName("GetAuthorById")
               .Produces<AuthorItem>()
               .Produces(404);

            // Lấy danh sách bài viết được viết bởi tác giả có ID cho trước
            routeGroupBuilder.MapGet("/{id:int}/posts", GetPostByAuthorId)
              .WithName("GetPostByAuthorId")
              .Produces<PaginationResult<PostItem>>();

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostByAuthorSlug)
              .WithName("GetPostByAuthorSlug")
              .Produces<PaginationResult<PostDTO>>();

            // Tạo thêm tác giả mới
            routeGroupBuilder.MapPost("/{id:int}", AddNewAuthor)
                .WithName("AddNewAuthor")
                .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
               .WithName("SetAuthorPicture")
               .Accepts<IFormFile>("multipart/form-data")
               .Produces<string>()
               .Produces(400);

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
              .WithName("UpdateAuthor")
              .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
              .Produces(204)
              .Produces(400)
              .Produces(409);

            routeGroupBuilder.MapDelete("/", DeleteAuthor)
              .WithName("DeleteAuthor")
              .Produces(204)
              .Produces(404);

            return app;
        }

        //Method xử lý yêu cầu tìm danh sách tác giả
        private static async Task<IResult> GetAuthors(
            [AsParameters] AuthorFilterModel model,
            IAuthorRepository authorRepository)
        {
            var authorsList = await authorRepository.GetPagedAuthorsAsync(model, model.Name);

            var paginationResult = new PaginationResult<AuthorItem>(authorsList);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetAuthorDetails(int id, IAuthorRepository authorRepository, IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);
            return author == null
                ? Results.NotFound($"không tìm thấy tác giả có mã số {id}")
                : Results.Ok(mapper.Map<AuthorItem>(author));
        }

        private static async Task<IResult> GetPostByAuthorId(int id, [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorId = id,
                PublishedOnly = true
            };

            var postList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
                posts => posts.ProjectToType<PostDTO>());

            var paginationResult = new PaginationResult<PostDTO>(postList);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetPostByAuthorSlug([FromRoute] string slug, 
            [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                PublishedOnly = true
            };

            var postList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel,
                posts => posts.ProjectToType<PostDTO>());

            var paginationResult = new PaginationResult<PostDTO>(postList);

            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddNewAuthor(AuthorEditModel model,
            IValidator<AuthorEditModel> validator, IAuthorRepository authorRepository, IMapper mapper)
        {
            if (await authorRepository.IsAuthorSlugExistedAsync(0,model.UrlSlug))
            {
                return Results.Conflict($"slug '{model.UrlSlug}' đã được sử dụng");
            }

            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAsync(author);

            return Results.CreatedAtRoute("GetAuthorById", new { author.Id },
                mapper.Map<AuthorItem>(author));
        }

        private static async Task<IResult> SetAuthorPicture(int id, IFormFile imageFile,
            IAuthorRepository authorRepository, IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(),
                imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.BadRequest("Không lưu được tập tin");
            }

            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(imageUrl);
        }

        private static async Task<IResult> UpdateAuthor(int id, AuthorEditModel model,
            IValidator<AuthorEditModel> validator, IAuthorRepository authorRepository, IMapper mapper)
        {
            if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Conflict($"slug '{model.UrlSlug}' đã được sử dụng");
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;
            return await authorRepository.AddOrUpdateAsync(author)
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteAuthor(int id, IAuthorRepository authorRepository, IMapper mapper)
        {
            return await authorRepository.DeleteAuthorAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"Không tìm thấy tác giả có id = '{id}'");
        }



    }
}
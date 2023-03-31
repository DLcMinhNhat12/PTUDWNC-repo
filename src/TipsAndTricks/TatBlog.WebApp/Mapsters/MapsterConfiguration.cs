﻿using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Mapsters;
// Thiết lập việc copy data từ các đối tượng
public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Post, PostItem>()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.Tags, src => src.Tags.Select(x => x.Name));

        config.NewConfig<PostFilterModel, PostQuery>()
            .Map(dest => dest.PublishedOnly, src => false);

        config.NewConfig<PostEditModel, Post>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.ImageUrl);

        config.NewConfig<AuthorEditModel, Author>()
             .Ignore(dest => dest.Id)
             .Ignore(dest => dest.ImageUrl);

        config.NewConfig<Author, AuthorEditModel>()
              .Ignore(dest => dest.ImageFile);

        config.NewConfig<Post, PostEditModel>()
            .Map(dest => dest.SelectedTags, src =>
                string.Join("\r\n", src.Tags.Select(x => x.Name)))
            .Ignore(dest => dest.CategoryList)
            .Ignore(dest => dest.AuthorList)
            .Ignore(dest => dest.ImageFile);
    }
}

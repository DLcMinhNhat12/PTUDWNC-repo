using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders;

public interface IDataSeeder
{
	void Initialize();
}

public class DataSeeder : IDataSeeder
{
	private readonly BlogDbContext _dbContext;
    public DataSeeder(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Posts.Any()) return;

        var authors = AddAuthors();
        var categories = AddCategories();
        var tags = AddTags();
        var posts = AddPosts(authors, categories, tags);
    }

    private IList<Author> AddAuthors() 
    {
		var authors = new List<Author>()
		{
			new()
			{
				FullName = "Tran Dinh Minh Nhat",
				UrlSlug = "tdm-nhat",
				Email = "tdmnhat@gmail.com",
				JoinedDate = new DateTime(2023, 2, 25)
			},

			new()
			{
				FullName = "Do Quoc Sang",
				UrlSlug = "dq-sang",
				Email = "dqsang@gmail.com",
				JoinedDate = new DateTime(2021, 4, 30)
			},

			new()
			{
				FullName = "Mark J.Price",
				UrlSlug = "mj-price",
				Email = "mjprice@gmail.com",
				JoinedDate = new DateTime(2022, 10, 30)
			},

			new()
			{
				FullName = "L.R Svekis & R. Pervival & Mvan Putten",
				UrlSlug = "javascipt-beginer",
				Email = "js@beginertopro@gmail.com",
				JoinedDate = new DateTime(2022, 10, 30)
			},

			new()
			{
				FullName = "Nguyễn Hoàng Nguyên",
				UrlSlug = "rosie-nguyen",
				Email = "rosienguyen@gmail.com",
				JoinedDate = new DateTime(2016, 2, 22)
			},

			new()
			{
				FullName = "Đậu Thị Quyên",
				UrlSlug = "de-thoi-thanh-xuan-khong-nhat",
				Email = "dauthiquyen@gmail.com",
				JoinedDate = new DateTime(2022, 1, 30)
			}
		};

        _dbContext.Authors.AddRange(authors);
        _dbContext.SaveChanges();

        return authors;
    }

    private IList<Category> AddCategories() 
    {
        var categories = new List<Category>()
        {
            new() {Name = ".Net Core", Description = ".Net Core", UrlSlug = ""},
			new() {Name = "Architecture", Description = "Architecture", UrlSlug = ""},
			new() {Name = "Messaging", Description = "Messaging", UrlSlug = ""},
			new() {Name = "OOP", Description = "Object-Oriented Program", UrlSlug = ""},
			new() {Name = "Design Patterns", Description = "Design Patterns", UrlSlug = ""}
		};

		_dbContext.AddRange(categories);
		_dbContext.SaveChanges();

		return categories;
    }

    private IList<Tag> AddTags() 
    {
        var tags = new List<Tag>()
        {
            new() {Name = "Google", Description = "Google applications", UrlSlug = "google-application"},
			new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "aspdotnet-mvc"},
			new() {Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page"},
			new() {Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
			new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning"},
			new() {Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-networking"},
			new() {Name = "Tuổi trẻ đáng giá bao nhiêu?", Description = "Tuổi trẻ đáng giá bao nhiêu?", UrlSlug = "tuoi-tre-dang-gia-bao-nhieu"},
			new() {Name = "Để thời sinh viên không nhạt", Description = "Để thời sinh viên không nhạt", UrlSlug = "de-thoi-sinh vien-khong-nhat"},
			new() {Name = "Clean Code", Description = "cleanCode", UrlSlug = "clean-code"},
		};

		_dbContext.AddRange(tags);
		_dbContext.SaveChanges();

		return tags;
	}

    private IList<Post> AddPosts(
        IList<Author> authors,
        IList<Category> categories, 
        IList<Tag> tags) 
    {
        var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friends has a great repository filled",
                Description = "Here's a few DON'T and DO examples",
                Meta = "David and friends has a great repository",
                UrlSlug = "aspdotnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            },

			new()
			{
				Title = "C# 10 and .NET 6 – Modern\r\nCross-Platform Development\r\nSixth Edition",
				ShortDescription = "Build apps, websites, and services with ASP.NET Core 6",
				Description = "Blazor, and EF Core 6 using Visual Studio 2022 and\r\nVisual Studio Code",
				Meta = "Copyright © 2021 Packt Publishing",
				UrlSlug = "csharp-dotnet6-moderncross-platform-development",
				Published = true,
				PostedDate = new DateTime(2022, 10, 22, 10, 40, 0),
				ModifiedDate = null,
				ViewCount = 101,
				Author = authors[0],
				Category = categories[0],
				Tags = new List<Tag>()
				{
					tags[0]
				}
			},

			new()
			{
				Title = "JavaScript from Beginner to Professional",
				ShortDescription = "Learn JavaScript quickly",
				Description = "Learn JavaScript quickly by building fun, interactive, and dynamic web apps, games, and pages",
				Meta = "Learn JavaScript quickly",
				UrlSlug = "javascript-from-beginer-to-professional",
				Published = true,
				PostedDate = new DateTime(2022, 9, 9, 10, 10, 0),
				ModifiedDate = null,
				ViewCount = 101,
				Author = authors[0],
				Category = categories[0],
				Tags = new List<Tag>()
				{
					tags[0]
				}
			},

			new()
			{
				Title = "Tuổi trẻ đáng giá bao nhiêu?",
				ShortDescription = "Hành trang để tuổi trẻ tươi đẹp hơn",
				Description = "Tuổi trẻ gắn liền học - làm - đi ra sao?",
				Meta = "Hành trang để tuổi trẻ tươi đẹp hơn",
				UrlSlug = "tuoi-tre-dang-gia-bao-nhieu",
				Published = true,
				PostedDate = new DateTime(2017, 7, 9, 8, 10, 0),
				ModifiedDate = null,
				ViewCount = 10199,
				Author = authors[0],
				Category = categories[0],
				Tags = new List<Tag>()
				{
					tags[0]
				}
			}
		};

		_dbContext.AddRange(posts);
		_dbContext.SaveChanges();

		return posts;
	}
}

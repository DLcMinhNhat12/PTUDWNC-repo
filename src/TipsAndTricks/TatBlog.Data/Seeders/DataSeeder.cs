using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders;

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
            FullName = "Robert C. Martin",
            UrlSlug = "csharp-dotnet",
            Email = "robermartin@gmail.com",
            JoinedDate = new DateTime(2023, 2, 25)
        },

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
            JoinedDate = new DateTime(2020, 4, 30)
        },

        new()
        {
            FullName = "Mark J.Price",
            UrlSlug = "mj-price",
            Email = "mjprice@gmail.com",
            JoinedDate = new DateTime(2021, 10, 30)
        },

        new()
        {
            FullName = "L.R Svekis & R. Pervival & Mvan Putten",
            UrlSlug = "javascipt-beginer",
            Email = "js@beginertopro@gmail.com",
            JoinedDate = new DateTime(2022, 3, 13)
        },

        new()
        {
            FullName = "Nguyễn Hoàng Nguyên",
            UrlSlug = "rosie-nguyen",
            Email = "rosienguyen@gmail.com",
            JoinedDate = new DateTime(2016, 8, 22)
        },

        new()
        {
            FullName = "Đậu Thị Quyên",
            UrlSlug = "de-thoi-thanh-xuan-khong-nhat",
            Email = "dauthiquyen@gmail.com",
            JoinedDate = new DateTime(2022, 8, 16)
        },
        new()
        {
            FullName = "Robin Sharma",
            UrlSlug = "doi-ngan-dung-ngu-dai",
            Email = "robinsharama@author.com",
            JoinedDate = new DateTime(2022, 6, 20)
        },
        new()
        {
            FullName = "John Vũ",
            UrlSlug = "muon-kiep-nhan-sinh",
            Email = "johnvu@author.com",
            JoinedDate = new DateTime(2022, 2, 15)
        },
         new()
        {
            FullName = "Dale Carnegie",
            UrlSlug = "how-to-win-friends-and-influence-people",
            Email = "dalecarnegie@author.com",
            JoinedDate = new DateTime(2010, 1, 22)
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
        new() {Name = ".Net Core", Description = ".Net Core and ASP.NET", UrlSlug = ""},
        new() {Name = "Architecture", Description = "Architecture Book", UrlSlug = ""},
        new() {Name = "Messaging", Description = "Messaging", UrlSlug = ""},
        new() {Name = "OOP", Description = "Object-Oriented Program", UrlSlug = "lap-trinh-huong-doi-tuong"},
        new() {Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "thiet-ke-mau"},
        new() {Name = "Bookself", Description = "Những cuổn sách nên đọc ít nhất một lần", UrlSlug = "thiet-ke-mau"},
        new() {Name = "Muôn kiếp nhân sinh", Description = "Luật nhân quả", UrlSlug = "muon-kiep-nhan-sinh"},
        new() {Name = "Đời ngắn đừng ngủ dài", Description = "Thời gian là sự quý báu", UrlSlug = "doi-ngan-dung-ngu-dai"},
        new() {Name = "Đắc nhân tâm", Description = "Sách gối đầu giường", UrlSlug = "how-to-win-friends-and-influence-people"}
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
        new() {Name = "Blazor", Description = "Data Structures", UrlSlug = "blazor-data-structure"},
        new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning"},
        new() {Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-networking"},
        new() {Name = "Tuổi trẻ đáng giá bao nhiêu?", Description = "Tuổi trẻ đáng giá bao nhiêu?", UrlSlug = "tuoi-tre-dang-gia-bao-nhieu"},
        new() {Name = "Để thời sinh viên không nhạt", Description = "Để thời sinh viên không nhạt", UrlSlug = "de-thoi-sinh vien-khong-nhat"},
        new() {Name = "Clean Code", Description = "cleanCode", UrlSlug = "clean-code"},
        new() {Name = "Muôn kiếp nhân sinh", Description = "Muôn kiếp nhân Sinh bức tranh về cuộc đời...", UrlSlug = "muon-kiep-nhan-sinh-john-vu"},
        new() {Name = "Đời ngắn đừng ngủ dài", Description = "Đời ngắn đừng ngủ dài", UrlSlug = "doi-ngan-dung-ngu-dai"}
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
            ImageUrl = "/images/aspdotnet.jpeg",
            Published = true,
            PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
            ModifiedDate = null,
            ViewCount = 5031,
            Author = authors[0],
            Category = categories[0],
            Tags = new List<Tag>()
            {
                tags[0],
                tags[1]
            }
        },

        new()
        {
            Title = "Clean Code",
            ShortDescription = "Code Clean and performance",
            Description = "Code clean is effective",
            Meta = "code-clean",
            UrlSlug = "code-clean",
            ImageUrl = "/images/book-cleancode.jpg",
            Published = true,
            PostedDate = new DateTime(2019, 8, 15, 9, 05, 0),
            ModifiedDate = null,
            ViewCount = 5031,
            Author = authors[0],
            Category = categories[2],
            Tags = new List<Tag>()
            {
                tags[0],
                tags[8]
            }
        },

        new()
        {
            Title = "C# 10 and .NET 6 – Modern\r\nCross-Platform Development\r\nSixth Edition",
            ShortDescription = "Build apps, websites, and services with ASP.NET Core 6",
            Description = "Blazor, and EF Core 6 using Visual Studio 2022 and\r\nVisual Studio Code",
            Meta = "Copyright © 2021 Packt Publishing",
            UrlSlug = "csharp-dotnet6-moderncross-platform-development",
            ImageUrl = "/images/aspdotnet.jpeg",
            Published = true,
            PostedDate = new DateTime(2022, 10, 22, 10, 40, 0),
            ModifiedDate = null,
            ViewCount = 2210,
            Author = authors[0],
            Category = categories[0],
            Tags = new List<Tag>()
            {
                tags[1]
            }
        },

        new()
        {
            Title = "JavaScript from Beginner to Professional",
            ShortDescription = "Learn JavaScript quickly",
            Description = "Learn JavaScript quickly by building fun, interactive, and dynamic web apps, games, and pages",
            Meta = "Learn JavaScript quickly",
            UrlSlug = "javascript-from-beginer-to-professional",
            ImageUrl = "/images/aspdotnet.jpeg",
            Published = true,
            PostedDate = new DateTime(2022, 9, 9, 10, 10, 0),
            ModifiedDate = null,
            ViewCount = 1010,
            Author = authors[4],
            Category = categories[3],
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
            ImageUrl = "/images/book-ttdgbn.jfif",
            Published = true,
            PostedDate = new DateTime(2017, 7, 9, 8, 10, 0),
            ModifiedDate = null,
            ViewCount = 100202,
            Author = authors[5],
            Category = categories[5],
            Tags = new List<Tag>()
            {
                tags[7]
            }
        },

        new()
        {
            Title = "Để thời sinh viên không nhạt",
            ShortDescription = "Sinh viên là khoảng thời gian đẹp và quý giá nhất",
            Description = "Hành trang để sinh viên tự tin, trân trọng thời gian, học tập, rèn luyện... ",
            Meta = "Làm sao để thời Sinh viên không nhạt",
            UrlSlug = "de-thoi-sinh-vien-khong-nhat",
            ImageUrl = "/images/book-dtsvkn.jpg",
            Published = true,
            PostedDate = new DateTime(2016, 5, 12, 9, 10, 0),
            ModifiedDate = null,
            ViewCount = 2002,
            Author = authors[6],
            Category = categories[5],
            Tags = new List<Tag>()
            {
                tags[0]
            }
        },

        new()
        {
            Title = "Đời ngắn đừng ngủ dài",
            ShortDescription = "Tác giả của những cuốn sách nổi tiếng",
            Description = "Thời gian là thước đo của thành công...",
            Meta = "Làm sao để thời gian trôi qua không hối tiếc?",
            UrlSlug = "doi-ngan-dung-ngu-dai",
            ImageUrl = "/images/book-dndnd.jpg",
            Published = true,
            PostedDate = new DateTime(2018, 11, 5, 8, 00, 0),
            ModifiedDate = null,
            ViewCount = 201999,
            Author = authors[7],
            Category = categories[7],
            Tags = new List<Tag>()
            {
                tags[10]
            }
        },

        new()
        {
            Title = "Muôn kiếp nhân sinh",
            ShortDescription = "Đầu sách hay về Luật Nhân - Quả",
            Description = "Bất cứ một hành động nào cũng tạo ra một nhân, và đã có nhân thì ắt phải có quả. Đó là quy luật của Vũ trụ!",
            Meta = "Luật nhân quả",
            UrlSlug = "muon-kiep-nhan-sinh",
            ImageUrl = "/images/book-mkns.jpg",
            Published = true,
            PostedDate = new DateTime(2016, 5, 12, 9, 10, 0),
            ModifiedDate = null,
            ViewCount = 303567,
            Author = authors[8],
            Category = categories[6],
            Tags = new List<Tag>()
            {
                tags[9]
            }
        },

        new()
        {
            Title = "Đắc nhân tâm",
            ShortDescription = "Cuổn sách gối đầu giường",
            Description = "Cuốn sách giúp bạn thấu hiểu được bản, rèn luyện nội tâm...",
            Meta = "Đắc nhân tâm",
            UrlSlug = "dac-nhan-tam-how-to-win-friends-and-influence-people",
            ImageUrl = "/images/book-dacnhantam.jpg",
            Published = true,
            PostedDate = new DateTime(1936, 7, 19, 5, 27, 0),
            ModifiedDate = null,
            ViewCount = 1249990,
            Author = authors[9],
            Category = categories[8],
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

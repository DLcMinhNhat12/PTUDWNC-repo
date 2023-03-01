﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
	// tìm bài viết có tên định danh là slug
	// và đăng vào tháng, năm?

	Task<Post> GetPostAsync(
		int year,
		int month,
		string slug,
		CancellationToken cancellationToken = default);

	Task<IList<Post>> GetPopularArticlesAsync(
		int numPosts,
		CancellationToken cancellationToken = default);

	Task<bool> IsPostSlugExitsedAsync(
		int postId, string slug,
		CancellationToken cancellationToken = default);

	Task IncreaseViewCountAsync(
		int postId,
		CancellationToken cancellationToken = default);

	Task<IList<CategoryItem>> GetCategoriesAsync(
		bool ShowOnMenu = false,
		CancellationToken cancellationToken = default);

	// Lấy danh sách từ khóa/ thẻ và phân theo thamso
	Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default);
}

public class BloggRepository : IBlogRepository
{
	private readonly BlogDbContext _context;

    public BloggRepository(BlogDbContext context)
    {
		_context = context;
    }

	// Lấy ds chuyên mục và số lượng bài viết
	public async Task<IList<CategoryItem>> GetCategoriesAsync(
		bool ShowOnMenu = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Category> categories = _context.Set<Category>();

		if (ShowOnMenu)
		{
			categories = categories.Where(x => x.ShowOnMenu);
		}

		return await categories
			.OrderBy(x => x.Name)
			.Select(x => new CategoryItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				ShowOnMenu = x.ShowOnMenu,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<Post> GetPostAsync(
		int year,
		int month,
		string slug,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Post> postQuery = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author);

		if (year > 0)
		{
			postQuery = postQuery.Where(x => x.PostedDate.Year == year);
		}

		if (month > 0)
		{
			postQuery = postQuery.Where(x => x.PostedDate.Month == month);
		}

		if (!string.IsNullOrWhiteSpace(slug))
		{
			postQuery = postQuery.Where(x => x.UrlSlug == slug);
		}

		return await postQuery.FirstOrDefaultAsync(cancellationToken);
	}
	
	// Tìm bài N bài vết nhiều người xem nhất
	public async Task<IList<Post>> GetPopularArticlesAsync(
		int numPosts, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Include(x => x.Author)
			.Include(x => x.Category)
			.OrderByDescending(p => p.ViewCount)
			.Take(numPosts)
			.ToListAsync(cancellationToken);

	}

	public async Task<bool> IsPostSlugExitsedAsync(
		int postId, string slug,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
	}

	public async Task IncreaseViewCountAsync(
		int postId,
		CancellationToken cancellationToken = default)
	{
		await _context.Set<Post>()
			.Where(x => x.Id == postId)
			.ExecuteUpdateAsync(p =>
			p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
			cancellationToken);
	}

	//Lấy ds tags và phân trang theo tham số
	public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default)
	{
		var tagQuery = _context.Set<Tag>()
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			});

		return await tagQuery
			.ToPagedListAsync(pagingParams, cancellationToken);
	}
}
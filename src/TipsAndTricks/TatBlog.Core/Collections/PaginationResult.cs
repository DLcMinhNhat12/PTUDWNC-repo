using TatBlog.Core.Contracts;

namespace TatBlog.Core.Collections;

public class PaginationResult<T>
{
	//Danh sách mẫu tin cần nhận
	public IEnumerable<T> Items { get; set; }

	// Thông tin điều hướng
	public PagingMetadata Metadata { get; set; }
	
	public PaginationResult(IPagedList<T> pagedList)
	{
		Items = pagedList;
		Metadata = new PagingMetadata(pagedList);
	}
}
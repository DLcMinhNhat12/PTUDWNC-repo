using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts;

// Chứa thông tin cần thiết cho việc phân trang
public interface IPagingParams
{
	int PageSize { get; set; }

	int PageNumber { get; set; }

	// Tên cột muốn sort
	string SortColumn { get; set; }

	string SortOrder { get; set; }
}

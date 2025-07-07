using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Core.Models
{
    public class PageResult<T> : PageResultBase where T : class
    {
        public List<T> Results { get; set; } = new List<T>();
        public PageResult()
        {
            Results = new List<T>();
        }
        //public PageResult(int currentPage, int pageSize, int rowCount, List<T> items)
        //{
        //    CurrentPage = currentPage;
        //    PageSize = pageSize;
        //    RowCount = rowCount;
        //    Items = items ?? new List<T>();
        //}
    }
    {
    }
}

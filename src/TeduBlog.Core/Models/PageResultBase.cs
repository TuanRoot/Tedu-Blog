using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Core.Models
{
    public abstract class PageResultBase
    {
        public int CurrentPage { get; set; }
        private int _pageCount;
        public int PageCount
        {
            get
            {
                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                //PageCount = value;
                _pageCount = value;
            }
        }

        public int PageSize { get; set; }
        public int RowCount { get; set; }

        //Đây là hàng đầu tiên của trang hiện tại.
        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        //Tính hàng cuối cùng của trang hiện tại.
        //Nếu dữ liệu không đầy đủ (VD: trang cuối có 7 bản ghi thay vì 10), dùng Math.Min để đảm bảo không vượt quá RowCount.
        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
        public string? AdditionalData { get; set; }
    }
}

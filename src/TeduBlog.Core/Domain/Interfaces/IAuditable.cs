using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Core.Domain.Interfaces
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}

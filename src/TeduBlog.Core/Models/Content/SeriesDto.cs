using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduBlog.Core.Domain.Content;

namespace TeduBlog.Core.Models.Content
{
    public class SeriesDto : SeriesInListDto
    {
        public string? Content { get; set; }
        public string? SeoDescription { get; set; }
        public string? Thumbnail { get; set; }
        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {
                CreateMap<Series, SeriesDto>();
            }
        }
    }
}

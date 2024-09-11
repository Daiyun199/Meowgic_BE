using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Question
{
    public class QueryPagedQuestion
    {
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "sortColumn")]
        public string SortColumn { get; set; } = "id";

        [FromQuery(Name = "orderByDesc")]
        public bool OrderByDesc { get; set; } = true;

        [FromQuery(Name = "categoryId")]
        public int? CategoryId { get; set; }

        [FromQuery(Name = "description")]
        public string? Desciption { get; set; }
    }
}

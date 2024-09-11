using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Card
{
    public class QueryPagedCard
    {
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "sortColumn")]
        public string SortColumn { get; set; } = "name";

        [FromQuery(Name = "orderByDesc")]
        public bool OrderByDesc { get; set; } = true;

        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
}

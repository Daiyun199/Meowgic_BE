using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Request.Question
{
    public class FilterQuestionByCategoryId
    {
        [Required]
        public int CategoryId { get; set; }
    }
}

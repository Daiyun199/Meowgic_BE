using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Question
{
    public class QuestionResponse
    {
        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? CategoryId { get; set; }
    }
}

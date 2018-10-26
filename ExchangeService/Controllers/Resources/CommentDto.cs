using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Resources
{
    public class CommentDto
    {
        public DateTime CommentDate { get; set; }
        [Required]
        [StringLength(512)]
        public string Text { get; set; }
        [Required]
        [Range(0, 5)]
        public double Mark { get; set; }
    }
}

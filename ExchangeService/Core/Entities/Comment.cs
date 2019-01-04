using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeService.Core.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int ReceivingUserId { get; set; }
        public int LeavingUserId { get; set; }
        public DateTime CommentDate { get; set; }
        [Required]
        [StringLength(512)]
        public string Text { get; set; } 
        [Required]
        [Range(0,5)]
        public double Mark { get; set; }
        public bool IsVisible { get; set; }
    }
}

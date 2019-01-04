using System;

namespace ExchangeService.Shared.Resources
{
    public class CommentDto
    {
        public int LeavingUserId { get; set; }
        public DateTime CommentDate { get; set; }
        public string Text { get; set; }
        public double Mark { get; set; }
        public bool IsVisible { get; set; }
    }
}

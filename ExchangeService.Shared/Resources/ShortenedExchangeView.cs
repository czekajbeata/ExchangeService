using ExchangeService.Shared.Enums;
using System;

namespace ExchangeService.Shared.Resources
{
    public class ShortenedExchangeView
    {
        public int ExchangeId { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
        public ExchangeState State { get; set; }
    }
}

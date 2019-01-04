﻿using ExchangeService.Shared.Enums;
using System;

namespace ExchangeService.Shared.Resources
{
    public class ExchangeView
    {
        public int ExchangeId { get; set; }
        public string UserImage { get; set; }
        public string OtherUserName { get; set; }
        public string[] MyGames { get; set; }
        public string[] OtherUserGames { get; set; }
        public Shipment Shipment { get; set; }
        public ExchangeState State { get; set; }
        public bool AmIOffering { get; set; }

    }
}
using System;
using System.Collections.Generic;

namespace ETModel
{
    
    public class BidControllerComponent : Component
    {
        public GamerReserveTulip reserveTulip;
        public Int32 Price;
        public Int64 userId;
        public Dictionary<Int64, Boolean> gamerStatus = new Dictionary<long, bool>();
    }
}
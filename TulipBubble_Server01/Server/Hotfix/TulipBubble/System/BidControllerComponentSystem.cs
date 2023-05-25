using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    public static class BidControllerComponentSystem
    {
        public static void StartBid(this BidControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            RoomTulipCardsComponent roomTulipCardsComponent = room.GetComponent<RoomTulipCardsComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();
            TulipMarketEconomicsComponent tulipMarketEconomicsComponent =
                room.GetComponent<TulipMarketEconomicsComponent>();

            if (roomTulipCardsComponent.reservedTulipCards.Count == 0)
            {
                orderControllerComponent.PrepareNextPhase();
                return;
            }

            self.gamerStatus.Clear();
            self.reserveTulip =
                roomTulipCardsComponent.reservedTulipCards[roomTulipCardsComponent.reservedTulipCards.Count - 1];

            self.userId = self.reserveTulip.UserId.Last();
            self.Price = tulipMarketEconomicsComponent.GetTulipPrice(self.reserveTulip.ReserveTulipCard) - 1;
            foreach (Int64 userId in self.reserveTulip.UserId)
            {
                self.gamerStatus.Add(userId, false);
            }

            orderControllerComponent.SetGamerBidTurn(self.userId, self.Price);
        }

        public static void ContinueBid(this BidControllerComponent self, Int64 userId, Int32 price)
        {
            Room room = self.GetParent<Room>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();
            BankComponent bankComponent = room.GetComponent<BankComponent>();

            if (price == 0)
            {
                self.gamerStatus.Remove(userId);
                self.gamerStatus.Add(userId, true);

                Int32 count = 0;
                foreach (KeyValuePair<long, bool> gamerStatus in self.gamerStatus)
                {
                    if (gamerStatus.Value == true)
                        count += 1;
                }

                if (count == self.gamerStatus.Count)
                {
                    Log.Info($"{self.userId} Buy this TulipCard");
                    bankComponent.NotifyPlayerPayWay(room.GetGamerFromUserID(self.userId));
                    return;
                }
            }

            self.gamerStatus.Clear();
            foreach (Int64 Id in self.reserveTulip.UserId)
            {
                self.gamerStatus.Add(Id, false);
            }

            self.gamerStatus.Remove(userId);
            self.gamerStatus.Add(userId, true);
            self.userId = userId;

            Int32 passCount = 0;
            foreach (KeyValuePair<long, bool> gamerStatus in self.gamerStatus)
            {
                if (gamerStatus.Value == false)
                {
                    orderControllerComponent.SetGamerBidTurn(gamerStatus.Key, price);
                    continue;
                }
                passCount += 1;
            }

            if (passCount == self.gamerStatus.Count)
            {
                Log.Info($"{self.userId} Buy this TulipCard");
                bankComponent.NotifyPlayerPayWay(room.GetGamerFromUserID(self.userId));
            }
        }
    }
}
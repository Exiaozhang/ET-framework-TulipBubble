using ETHotfix;

namespace ETModel
{
    /// <summary>
    /// 银行组件
    /// </summary>
    public static class BankComponentSystem
    {
        public static void NotifyPlayerPayWay(this BankComponent self, Gamer gamer)
        {
            ActorMessageSenderComponent actorProxyComponent =
              Game.Scene.GetComponent<ActorMessageSenderComponent>();
            ActorMessageSender actorMessageSender = actorProxyComponent.Get(gamer.CActorID);

            actorMessageSender.Send(new Actor_NotifyPlayerPayWay_Ntt { });
        }

        public static void PayMoneyToBank(this BankComponent self, Gamer gamer, int payWay)
        {
            MoneyComponent moneyComponent = gamer.GetComponent<MoneyComponent>();
            Room room = self.GetParent<Room>();
            BidControllerComponent bidControllerComponent = room.GetComponent<BidControllerComponent>();
            HandCardsComponent handCardsComponent = gamer.GetComponent<HandCardsComponent>();
            RoomTulipCardsComponent roomTulipCardsComponent = room.GetComponent<RoomTulipCardsComponent>();

            if ((PayWay)payWay == PayWay.Cash)
            {
                if (moneyComponent.money < bidControllerComponent.Price)
                {
                    Log.Info("Money not enough");
                    return;
                }

                moneyComponent.money -= bidControllerComponent.Price;
                handCardsComponent.AddCard(bidControllerComponent.reserveTulip.ReserveTulipCard);
                roomTulipCardsComponent.reservedTulipCards.Remove(bidControllerComponent.reserveTulip);
                Log.Info($"{gamer.UserID} Left Money {moneyComponent.money}");
                bidControllerComponent.StartBid();
                return;

            }

            if ((PayWay)payWay == PayWay.Loans)
            {
                handCardsComponent.AddLoanCard(bidControllerComponent.reserveTulip.ReserveTulipCard, bidControllerComponent.Price);
                bidControllerComponent.StartBid();
                return;
            }
        }
    }
}
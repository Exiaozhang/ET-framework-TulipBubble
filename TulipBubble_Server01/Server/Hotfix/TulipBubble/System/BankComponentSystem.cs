using System;
using System.Reflection.Metadata;
using ETHotfix;
using Google.Protobuf.Collections;

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
            ReserveSignComponent reserveSignComponent = gamer.GetComponent<ReserveSignComponent>();
            Room room = self.GetParent<Room>();
            BidControllerComponent bidControllerComponent = room.GetComponent<BidControllerComponent>();
            HandCardsComponent handCardsComponent = gamer.GetComponent<HandCardsComponent>();
            RoomTulipCardsComponent roomTulipCardsComponent = room.GetComponent<RoomTulipCardsComponent>();
            ActorMessageSenderComponent actorMessageSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            ActorMessageSender actorMessageSender = actorMessageSenderComponent.Get(gamer.CActorID);

            if ((PayWay)payWay == PayWay.Cash)
            {
                if (moneyComponent.money < bidControllerComponent.Price)
                {
                    Log.Info("Money not enough");
                    return;
                }

                moneyComponent.money -= bidControllerComponent.Price;
                roomTulipCardsComponent.PopCard(bidControllerComponent.reserveTulip.ReserveTulipCard);
                handCardsComponent.AddCard(bidControllerComponent.reserveTulip.ReserveTulipCard);
                roomTulipCardsComponent.reservedTulipCards.Remove(bidControllerComponent.reserveTulip);

                Log.Info($"{gamer.UserID} Left Money {moneyComponent.money}");

                RepeatedField<TulipCard> loanCards = new RepeatedField<TulipCard>();
                RepeatedField<int> cardsPrice = new RepeatedField<Int32>();

                foreach (var reservedTulipCards in handCardsComponent.loanTulipLibrary)
                {
                    loanCards.Add(reservedTulipCards.Key);
                    cardsPrice.Add(reservedTulipCards.Value);
                }

                actorMessageSender.Send(new Actor_GetHandCard_Ntt()
                {
                    HandCard = MapHelper.To.RepeatedField(handCardsComponent.tulipLibrary),
                    LoanCard = loanCards,
                    CardPrice = cardsPrice
                });

                actorMessageSender.Send(new Actor_GetMoney_Ntt()
                {
                    Money = moneyComponent.money
                });

                bidControllerComponent.StartBid();
                return;

            }

            if ((PayWay)payWay == PayWay.Loans)
            {
          
                roomTulipCardsComponent.PopCard(bidControllerComponent.reserveTulip.ReserveTulipCard);
                handCardsComponent.AddLoanCard(bidControllerComponent.reserveTulip.ReserveTulipCard, bidControllerComponent.Price);
                roomTulipCardsComponent.reservedTulipCards.Remove(bidControllerComponent.reserveTulip);
                reserveSignComponent.RemoveOneSign();
                RepeatedField<TulipCard> loanCards = new RepeatedField<TulipCard>();
                RepeatedField<int> cardsPrice = new RepeatedField<Int32>();

                foreach (var reservedTulipCards in handCardsComponent.loanTulipLibrary)
                {
                    loanCards.Add(reservedTulipCards.Key);
                    cardsPrice.Add(reservedTulipCards.Value);
                }

                actorMessageSender.Send(new Actor_GetHandCard_Ntt()
                {
                    HandCard = MapHelper.To.RepeatedField(handCardsComponent.tulipLibrary),
                    LoanCard = loanCards,
                    CardPrice = cardsPrice
                });

                bidControllerComponent.StartBid();
                return;
            }


        }

        public static void GiveMoneyToBank(this BankComponent self, Gamer gamer, int money)
        {
            Room room = self.GetParent<Room>();
            MoneyComponent moneyComponent = gamer.GetComponent<MoneyComponent>();

            moneyComponent.money -= money;
        }

        public static void GetMoneyFormBank(this BankComponent self, Gamer gamer, int money)
        {
            Room room = self.GetParent<Room>();
            MoneyComponent moneyComponent = gamer.GetComponent<MoneyComponent>();

            moneyComponent.money += money;
        }
    }
}
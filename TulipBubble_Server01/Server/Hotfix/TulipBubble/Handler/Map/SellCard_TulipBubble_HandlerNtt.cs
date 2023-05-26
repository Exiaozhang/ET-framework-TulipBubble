using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class SellCard_TulipBubble_Handler : AMActorHandler<Gamer, Actor_SellCard_Ntt>
    {
        protected override async ETTask Run(Gamer gamer, Actor_SellCard_Ntt message)
        {
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            Room room = tulipMatchComponent.GetGamingRoom(gamer);
            BankComponent bankComponent = room.GetComponent<BankComponent>();
            TulipMarketEconomicsComponent tulipMarketEconomicsComponent = room.GetComponent<TulipMarketEconomicsComponent>();
            HandCardsComponent handCardsComponent = gamer.GetComponent<HandCardsComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();

            foreach (var card in message.SelledTulipCards)
            {
                int price = tulipMarketEconomicsComponent.GetTulipPrice(card);
                bankComponent.GetMoneyFormBank(gamer, price);
                handCardsComponent.PopCard(card);
            }

            foreach (var card in message.SelledLoanCards)
            {
                int originPrice = tulipMarketEconomicsComponent.GetTulipPrice(card.LoanTulipcard);
                bankComponent.GetMoneyFormBank(gamer, originPrice);
                bankComponent.GiveMoneyToBank(gamer, card.LoanPrice);
                handCardsComponent.PopLoanCard(card.LoanTulipcard);
            }

           
        }
    }
}
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class SellCardToCollector_TulipBubble_Handler : AMActorHandler<Gamer, Actor_SellCardToCollector_Ntt>
    {
        protected override async ETTask Run(Gamer gamer, Actor_SellCardToCollector_Ntt message)
        {
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            Room room = tulipMatchComponent.GetGamingRoom(gamer);
            RoomCollectorCardsComponent roomCollectorCardsComponent = room.GetComponent<RoomCollectorCardsComponent>();
            HandCardsComponent handCardsComponent = gamer.GetComponent<HandCardsComponent>();

            roomCollectorCardsComponent.RemoveCollectorCard(message.SelledCollector);
            foreach (var card in message.SelledTulipCards)
            {
                handCardsComponent.PopCard(card);
            }
            
        }
    }

}
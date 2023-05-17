using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class ReserveTulip_TulipBubble_Handler : AMActorHandler<Gamer, Actor_ReserveTulipCard_Ntt>
    {
        protected override async ETTask Run(Gamer gamer, Actor_ReserveTulipCard_Ntt message)
        {
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            Room room = tulipMatchComponent.GetGamingRoom(gamer);
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();
            RoomTulipCardsComponent roomTulipCardsComponent = room.GetComponent<RoomTulipCardsComponent>();
            
            if (gamer.UserID != orderControllerComponent.currentAuthority.UserID)
                return;

            roomTulipCardsComponent.ReserveTulipCard(message.ReserveTulipCard);
            orderControllerComponent.NextGamerTurn();
        }
    }
}
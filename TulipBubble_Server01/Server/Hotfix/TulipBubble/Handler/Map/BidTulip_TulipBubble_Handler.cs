using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class BidTulip_TulipBubble_Handler : AMActorHandler<Gamer,Actor_NotifyRoomBid>
    {
        protected override async ETTask Run(Gamer gamer, Actor_NotifyRoomBid message)
        {
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            Room room = tulipMatchComponent.GetGamingRoom(gamer);
            BidControllerComponent bidControllerComponent = room.GetComponent<BidControllerComponent>();
            
            bidControllerComponent.ContinueBid(gamer.UserID,message.Price);
            
        }
    }
}
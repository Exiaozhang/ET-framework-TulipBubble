using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class PassSell_TulipBubble_Handler : AMActorHandler<Gamer, Actor_PassSell_Ntt>
    {
        protected override async ETTask Run(Gamer gamer, Actor_PassSell_Ntt message)
        {
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            Room room = tulipMatchComponent.GetGamingRoom(gamer);
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();

            orderControllerComponent.NextGamerTurn();
        }
    }
}
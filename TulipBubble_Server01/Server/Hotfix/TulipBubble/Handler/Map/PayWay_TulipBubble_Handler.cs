using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class PayWay_TulipBubble_Handler : AMActorHandler<Gamer, Actor_NotifyRoomPayWay_Ntt>
    {
        protected override async ETTask Run(Gamer gamer, Actor_NotifyRoomPayWay_Ntt message)
        {
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            Room room = tulipMatchComponent.GetGamingRoom(gamer);
            BankComponent bankComponent = room.GetComponent<BankComponent>();

            bankComponent.PayMoneyToBank(gamer,message.PayWay);
        }
    }
}

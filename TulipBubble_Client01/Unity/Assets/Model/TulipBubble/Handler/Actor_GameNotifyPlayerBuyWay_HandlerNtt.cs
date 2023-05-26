namespace ETModel
{
    [MessageHandler]
    public class Actor_GameNotifyPlayerBidWay_HandlerNtt : AMHandler<Actor_NotifyPlayerPayWay_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_NotifyPlayerPayWay_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();

            tulipRoomGamerPanelComponent.SetBuyWay();
        }
    }
}
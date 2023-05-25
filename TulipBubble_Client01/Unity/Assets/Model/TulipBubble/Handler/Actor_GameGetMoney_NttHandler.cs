namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetMoney_NttHandler : AMHandler<Actor_GetMoney_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetMoney_Ntt message)
        {
            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent =
                TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();
            TulipRoomGamerMoneyComponent tulipRoomGamerMoneyComponent =
                TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerMoneyComponent>();
            
            tulipRoomGamerPanelComponent.SetMoney(message.Money);
            tulipRoomGamerMoneyComponent.Money = message.Money;
        }
    }
}
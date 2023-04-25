namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetTulipPriceLevel_NttHandler : AMHandler<Actor_GetTulipPriceLevel_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetTulipPriceLevel_Ntt message)
        {
            Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom).GetComponent<TulipRoomComponent>()
                .MarketEconomicsComponent
                .SetPriceLvel(message.RedPriceLevel, message.WhitePriceLevel, message.YellowPriceLevel);
        }
    }
}
namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetCollector_NttHandler : AMHandler<Actor_GetCollector_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetCollector_Ntt message)
        {
            TulipRoomCollectorCardsComponent tulipRoomCollectorCardsComponent = Game.Scene.GetComponent<UIComponent>()
                .Get(TulipUIType.TulipRoom)
                .GetComponent<TulipRoomCollectorCardsComponent>();
            
            tulipRoomCollectorCardsComponent.SetCurrentHighPriceCollector(message.HighPriceCollector,
                message.HighPriceCollectorCount);
            tulipRoomCollectorCardsComponent.SetCurrentMiddlePriceCollector(message.MiddlePriceCollector,
                message.MiddlePriceCollectorCount);
            tulipRoomCollectorCardsComponent.SetCurrentLowPriceCollector(message.LowPriceCollector,
                message.LowPriceCollectorCount);
        }
    }
}
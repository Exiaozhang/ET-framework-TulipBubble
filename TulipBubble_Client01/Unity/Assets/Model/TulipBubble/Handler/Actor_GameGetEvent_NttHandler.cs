namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetEvent_NttHandler : AMHandler<Actor_GetEvent_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetEvent_Ntt message)
        {
            EventCard card = message.EventCard;
            TulipRoomEventCardsComponent tulipRoomEventCardsComponent = Game.Scene.GetComponent<UIComponent>()
                .Get(TulipUIType.TulipRoom).GetComponent<TulipRoomEventCardsComponent>();
            tulipRoomEventCardsComponent.SetEventDiscardPile(card);
            tulipRoomEventCardsComponent.SetEventCardPile(message.RemindEventCount);
        }
    }
}
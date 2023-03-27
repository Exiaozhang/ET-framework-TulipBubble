namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerExitRoom_NttHandler : AMHandler<Actor_GamerExitRoom_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GamerExitRoom_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent tulipRoomComponent = uiRoom.GetComponent<TulipRoomComponent>();
            tulipRoomComponent.RemoveGamer(message.UserID);

            await ETTask.CompletedTask;
        }
    }
}
using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameStart_NttHandler : AMHandler<Actor_GameStart>
    {
        protected override async ETTask Run(Session session, Actor_GameStart message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);

            TulipRoomComponent tulipRoomComponent = uiRoom.GetComponent<TulipRoomComponent>();

            tulipRoomComponent.HideCancelButton();
        }
    }

}
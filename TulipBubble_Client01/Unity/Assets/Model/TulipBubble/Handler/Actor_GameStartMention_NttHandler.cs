using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameStartMention_NttHandler : AMHandler<Actor_GameStartMention>
    {
        protected override async ETTask Run(Session session, Actor_GameStartMention message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent tulipRoomComponent = uiRoom.GetComponent<TulipRoomComponent>();
         
            
            tulipRoomComponent.ShowStartButton();
        }
    }
}
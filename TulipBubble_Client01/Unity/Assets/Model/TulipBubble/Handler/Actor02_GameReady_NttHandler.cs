using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor02_GameReady_NttHandler : AMHandler<Actor_GamerReady_TulipBubble>
    {
        protected override async ETTask Run(Session session, Actor_GamerReady_TulipBubble message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent room = uiRoom.GetComponent<TulipRoomComponent>();
            
            
       
            Gamer gamer = room.GetGamer(message.UserID);
            Log.Info(gamer.UserID.ToString());
            
            gamer.GetComponent<TulipRoomGamerOrderPanelComponent>().SetGamerReady();
            if (message.UserID == TulipRoomComponent.LocalGamer.UserID)
            {
              TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>().SetReady();
            }
   
            
            //本地玩家准备,隐藏准备按钮
            if (gamer.UserID == TulipRoomComponent.LocalGamer.UserID)
            {
                uiRoom.GameObject.Get<GameObject>("Ready").SetActive(false);
            }

            await ETTask.CompletedTask;
        }
    }
}
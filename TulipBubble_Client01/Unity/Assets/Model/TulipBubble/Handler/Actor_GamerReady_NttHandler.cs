using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameReady_NttHandler : AMHandler<Actor_GamerReady_TulipBubble>
    {
        protected override async ETTask Run(Session session, Actor_GamerReady_TulipBubble message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent room = uiRoom.GetComponent<TulipRoomComponent>();

            Gamer gamer = room.GetGamer(message.UserID);
            Log.Info(gamer.UserID.ToString());

            gamer.GetComponent<TulipRoomGamerOrderPanelComponent>().SetGamerReady();

            //判断是否为本地玩家
            if (room.IsLocalGame(message.UserID))
            {
                //玩家面板显示准备
                TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>().SetReady();

                //隐藏准备按钮
                room.HideReadyButton();
            }

            await ETTask.CompletedTask;
        }
    }
}
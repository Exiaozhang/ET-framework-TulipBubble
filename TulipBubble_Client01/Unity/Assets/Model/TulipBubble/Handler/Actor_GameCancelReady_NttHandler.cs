using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerCancelReady_NttHandler : AMHandler<Actor_GamerCancelReady_TulipBubble>
    {
        protected override async ETTask Run(Session session, Actor_GamerCancelReady_TulipBubble message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent room = uiRoom.GetComponent<TulipRoomComponent>();

            Gamer gamer = room.GetGamer(message.UserID);
            Log.Info(gamer.UserID.ToString() + "Ready");

            //取消显示准备图标
            gamer.GetComponent<TulipRoomGamerOrderPanelComponent>().CancelGamerReady();
            Log.Info("Test Ready");

            if (room.IsLocalGame(message.UserID))
            {
                //玩家面重置提示
                TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>().ResetPrompt();

                //房间隐藏取消准备按钮 显示准备按钮
                room.HideCancelButton();
                room.ShowReadyButton();
            }
        }
    }
}
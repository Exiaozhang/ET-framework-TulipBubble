using System;
using System.Collections.Generic;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameSyncFirstPlayer_NttHandler : AMHandler<Actor_SyncFirstPlayer_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_SyncFirstPlayer_Ntt message)
        {
            UI ui = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent room = ui.GetComponent<TulipRoomComponent>();
            TulipRoomGamerPanelComponent playerPanel = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();

            TulipRoomGameComponent roomGame = ui.GetComponent<TulipRoomGameComponent>();
            roomGame.currentRoundFirstUserId = message.UserID;

            //取消玩家顺序排名ui中显示当前回合的首位玩家
            List<Gamer> gamers = room.GetAllGamers();
            gamers.ForEach(gamer =>
            {
                if (gamer != null)
                    gamer.GetComponent<TulipRoomGamerOrderPanelComponent>().SetGamerFirst(false);
            });

            //在玩家顺序排名ui中显示当前回合的首位玩家
            Gamer firstGamer = room.GetGamer(message.UserID);
            TulipRoomGamerOrderPanelComponent tulipRoomGamerOrderPanelComponent = firstGamer.GetComponent<TulipRoomGamerOrderPanelComponent>();
            tulipRoomGamerOrderPanelComponent.SetGamerFirst();

        }
    }
}
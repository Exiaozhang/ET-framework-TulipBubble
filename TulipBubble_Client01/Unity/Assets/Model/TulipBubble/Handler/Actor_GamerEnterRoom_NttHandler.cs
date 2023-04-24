using System;
using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GamerEnterRoom_NttHandler : AMHandler<Actor_GamerEnterRoom_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GamerEnterRoom_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent tulipRoomComponent = uiRoom.GetComponent<TulipRoomComponent>();

            //从匹配状态中切换为准备状态
            if (tulipRoomComponent.Matching)
            {
                tulipRoomComponent.Matching = false; //从匹配状态中切换为准备状态
                GameObject matchPrompt = uiRoom.GameObject.Get<GameObject>("MatchPrompt");
                uiRoom.GameObject.Get<GameObject>("Ready").SetActive(true);
            }

            //添加进入房间的玩家，判定座位位置
            //添加未显示的玩家
            for (int i = 0; i < message.Gamers.Count; i++)
            {
                //如果服务端发来了默认空GamerInfo跳过
                GamerInfo gamerInfo = message.Gamers[i];
                if (gamerInfo.UserID == 0)
                    continue;
                //如果这个ID的玩家不在桌上
                if (tulipRoomComponent.GetGamer(gamerInfo.UserID) == null)
                {
                    Gamer gamer = ComponentFactory.Create<Gamer, long>(gamerInfo.UserID);
                    Log.Info($"第{i}个玩家加入房间");
                    tulipRoomComponent.AddGamer(gamer, i);

                    if (Convert.ToBoolean(gamerInfo.IsHoster))
                    {
                        gamer.GetComponent<TulipRoomGamerOrderPanelComponent>().SetGamerHoster();
                    }
                }

             
            }

            await ETTask.CompletedTask;
        }
    }
}
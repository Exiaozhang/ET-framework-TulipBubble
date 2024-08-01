using System;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class GamerCancelReady_tulipBubble_Handler : AMActorHandler<Gamer, Actor_GamerCancelReady_TulipBubble>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GamerCancelReady_TulipBubble message)
        {
            Log.Info("Received Player Cancel Ready");
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            //请求gamer目前所在的房间
            Room room = tulipMatchComponent.GetWaitingRoom(gamer);
            if (room != null)
            {
                //找到玩家的座位顺序
                int seatIndex = room.GetGamerSeat(gamer.UserID);
                if (seatIndex >= 0)
                {
                    //由等待状态设置为准备状态
                    room.isReadys[seatIndex] = false;

                    //如果房间内已经准备的玩家数量大于三个，通知房主关闭开始游戏的功能
                    if (room.GetReadyGamerCount() - 1 >= 2)
                    {
                        ActorMessageSenderComponent actorMessageSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                        ActorMessageSender actorMessageSender = actorMessageSenderComponent.Get(room.hoster.CActorID);

                        actorMessageSender.Send(new Actor_GameUnableStartMention());
                    }

                    //广播通知全房间玩家此player取消准备
                    room.Broadcast(new Actor_GamerCancelReady_TulipBubble()
                    {
                        UserID = gamer.UserID
                    });

                }
                else
                {
                    Log.Error("Player not in correct seat");
                }
            }
        }
    }
}

using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class GamerReady_TulipBubble_Handler : AMActorHandler<Gamer,Actor_GamerReady_TulipBubble>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GamerReady_TulipBubble message)
        {
            Log.Info("Received Player Is Ready");
            TulipMatchComponent tulipMatchComponent = Game.Scene.GetComponent<TulipMatchComponent>();
            //请求的gamer目前所在等待中的房间
            Room room = tulipMatchComponent.GetWaitingRoom(gamer);
            if (room != null)
            {
                //找到玩家的座位顺序
                int seatIndex = room.GetGamerSeat(gamer.UserID);
                if (seatIndex >= 0)
                {
                    //由等待状态设置为准备状态
                    room.isReadys[seatIndex] = true;
                    //广播通知全房间玩家此gamer已经准备好
                    room.Broadcast(new Actor_GamerReady_TulipBubble(){UserID = gamer.UserID});

                    if (room.GetReadyGamerCount() >= 3)
                    {
                        ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                        ActorMessageSender actorProxy = actorProxyComponent.Get(room.hoster.CActorID);
                        actorProxy.Send(new Actor_GameStartMention(){});
                    }
                }
                else
                {
                    Log.Error("玩家不在正确的座位上");
                }
            }

            await ETTask.CompletedTask;
        }
    }
}
﻿using ETModel;

namespace ETHotfix
{
    public static class RoomSystem
    {
        /// <summary>
        /// 检查游戏是否可以开始
        /// </summary>
        public static void CheckGameStart(this Room self)
        {
            Log.Debug("Check Can Game Start");
            bool isOK = true;

            for (int i = 0; i < self.isReadys.Length; i++)
            {
                //遍历所有准备状态 任何一个状态为false结果都是false
                if (self.isReadys[i] == false)
                {
                    isOK = false;
                }
            }

            if (isOK)
            {
                Log.Debug("满足游戏开始条件");
                self.GameStart();
            }
        }

        /// <summary>
        /// 郁金香泡沫游戏开始
        /// </summary>
        public static void GameStart(this Room self)
        {
            //更改房间状态 从空闲房间移除 添加到游戏中房间列表
            TulipMatchComponent Match = Game.Scene.GetComponent<TulipMatchComponent>();
            Match.FreeTulipBubbleRooms.Remove(self.Id);
            Match.GamingTulipBubbleRooms.Add(self.Id,self);
            
            //更改玩家状态
            for (int i = 0; i < self.gamers.Length; i++)
            {
                Gamer gamer = self.gamers[i];
                Match.Waiting.Remove(gamer.UserID);
                Match.Playing.Add(gamer.UserID,self);
            }
            
            //添加开始郁金香泡沫需要的组件
            self.AddComponent<GameControllerComponent,RoomConfig>(GateHelper.GetTulipBubbleRoomConfig(RoomLevel.Lv100));

            //开始游戏
            //self.GetComponent<GameControllerComponent>().StartGame();
        }

        public static void Add(this Room self, Gamer gamer)
        {
            int seatIndex = self.GetEmptySeat();
            //玩家需要获取一个座位坐下
            if (seatIndex >= 0)
            {
                self.gamers[seatIndex] = gamer;
                self.isReadys[seatIndex] = false;
                self.seats[gamer.UserID] = seatIndex;
            }
            else
            {
                Log.Error("房间已满无法加入");
            }
        }
        /// <summary>
        /// 获取房间中的玩家
        /// </summary>
        public static Gamer GetGamerFromUserID(this Room self, long id)
        {
            int seatindex = self.GetGamerSeat(id);
            if (seatindex >= 0)
            {
                return self.gamers[seatindex];
            }

            return null;
        }

        /// <summary>
        /// 获取玩家座位索引
        /// </summary>
        public static int GetGamerSeat(this Room self, long id)
        {
            if (self.seats.TryGetValue(id, out int seatIndex))
            {
                return seatIndex;
            }

            return -1;
        }
        
        /// <summary>
        /// 返回玩家是否已准备 玩家不在房间时返回false
        /// </summary>
        public static bool IsGamerReady(this Room self, Gamer gamer)
        {
            int seatIndex = self.GetGamerSeat(gamer.UserID);
            if (seatIndex > 0)
            {
                return self.isReadys[seatIndex];
            }
            return false;
        }


        /// <summary>
        /// 移除玩家并返回 玩家离开房间
        /// </summary>
        public static Gamer Remove(this Room self, long id)
        {
            int seatIndex = self.GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                Gamer gamer = self.gamers[seatIndex];
                self.gamers[seatIndex] = null;
                self.seats.Remove(id);
                return gamer;
            }
            return null;
        }


        /// <summary>
        /// 获取空座位
        /// </summary>
        /// <returns>返回座位索引，没有空座位时返回-1</returns>
        public static int GetEmptySeat(this Room self)
        {
            for (int i = 0; i < self.gamers.Length; i++)
            {
                if (self.gamers[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void Broadcast(this Room self, IActorMessage message)
        {
            foreach (Gamer gamer in self.gamers)
            {
                if (gamer == null || gamer.isOffline)
                {
                    continue;
                }

                ActorMessageSenderComponent actorProxyComponent= Game.Scene.GetComponent<ActorMessageSenderComponent>();
                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorID);
                actorProxy.Send(message);
            }
        }
        
        
    }
}
﻿using System;

namespace ETModel
{
    public class GameControllerComponent : Component
    {
        //房间配置
        public RoomConfig Config { get; set; }
        
        //底分
        public long BasePointPerMatch { get; set; }
        
        //全场倍率
        public int Multiples { get; set; }

        //最低入场门槛
        public long MinThreshold { get; set; }

        //郁金香数量=amerCount+2
        public int TulipMount { get; set; }
        
        //当前回合玩家
        public Int64 CurrentGamerUserId { get; set; }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            
            base.Dispose();

            this.BasePointPerMatch = 0;
            this.Multiples = 0;
            this.MinThreshold = 0;
        }

     
    }
}
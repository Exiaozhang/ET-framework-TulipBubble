﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace ETModel
{
    
    /// <summary>
    /// 房间配置
    /// </summary>
    public struct RoomConfig
    {
        /// <summary>
        /// 倍率
        /// </summary>
        public int Multiples { get; set; }

        /// <summary>
        /// 基础分
        /// </summary>
        public long BasePointPerMatch { get; set; }

        /// <summary>
        /// 房间最低门槛
        /// </summary>
        public long MinThreshold { get; set; }
    }
    /// <summary>
    /// 房间对象
    /// </summary>
    public class Room : Entity
    {
        /// <summary>
        /// 当前房间的5个座位 UserID/seatIndex
        /// </summary>
        public readonly Dictionary<long, int> seats = new Dictionary<long, int>();

        /// <summary>
        /// 当前房间的所有所有玩家 空位为null
        /// </summary>
        public readonly Gamer[] gamers = new Gamer[5];
        public readonly bool[] isReadys = new bool[5];

        public Gamer hoster;

        /// <summary>
        /// 房间中玩家的数量
        /// </summary>
        public int GamerCount => seats.Values.Count;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            
            seats.Clear();

            for (int i = 0; i < gamers.Length; i++)
            {
                if (gamers[i] != null)
                {
                    gamers[i].Dispose();
                    gamers[i] = null;
                }
            }

            for (int i = 0; i < isReadys.Length; i++)
            {
                isReadys[i] = false;
            }

        }
        
    }
}
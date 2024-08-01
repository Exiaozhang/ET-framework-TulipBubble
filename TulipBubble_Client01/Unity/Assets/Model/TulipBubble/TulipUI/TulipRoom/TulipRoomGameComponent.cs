using System;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class TulipRoomGameComponentAwakeSystem : AwakeSystem<TulipRoomGameComponent>
    {
        public override void Awake(TulipRoomGameComponent self)
        {
            self.Awake();
        }
    }


    public class TulipRoomGameComponent : Component
    {
        public Int64 currentTurnUserId;

        public GameStage stage;

        //当前轮次的起始玩家的Id
        public Int64 currentRoundFirstUserId;

        public static String localPlayerColor;

        private Dictionary<Int64, string> playerColor = new Dictionary<long, string>();

        private string[] userColor = new[] { "blue", "green", "purple", "red", "yellow" };

        public void Awake()
        {
        }

        /// <summary>
        /// 判断是否为这个玩家的回合
        /// </summary>
        /// <param name="userId">玩家的Id</param>
        /// <returns></returns>
        public bool WhetherIsGamerTurn(Int64 userId)
        {
            return userId == currentTurnUserId;
        }

        /// <summary>
        /// 是否为本地玩家的回合
        /// </summary>
        /// <returns></returns>
        public bool WhetherIsLocalGamerTurn()
        {
            return WhetherIsGamerTurn(TulipRoomComponent.LocalGamer.UserID);
        }


        /// <summary>
        /// 得到该玩家的颜色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserColor(Int64 userId)
        {
            if (playerColor.TryGetValue(userId, out String color))
                return color;
            Log.Info("Error Cant Find Color");
            return "Red";
        }

        /// <summary>
        /// 添加颜色到玩家
        /// </summary>
        /// <param name="usedId"></param>
        /// <param name="index"></param>
        public void AddColorToGamer(Int64 userId, Int32 index)
        {
            playerColor.Add(userId,userColor[index]);
        }

        /// <summary>
        /// 设置本地玩家的颜色
        /// </summary>
        /// <param name="userID"></param>
        public void SetLocalPlayerColor(Int64 userId)
        {
            playerColor.TryGetValue(userId, out String color);
            localPlayerColor = color;
        }
    }
}
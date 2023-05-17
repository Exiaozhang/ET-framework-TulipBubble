using System;
using ETModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Object = ETModel.Object;

namespace ETHotfix
{
    [ObjectSystem]
    public class OrderControllerComponetAwakeSystem : AwakeSystem<OrderControllerComponent>
    {
        public override void Awake(OrderControllerComponent self)
        {
            self.Awake();
        }
    }

    public static class OrderControllerComponentSystem
    {
        public static void Awake(this OrderControllerComponent self)
        {
        }

        /// <summary>
        /// 设定本轮的出牌顺序
        /// </summary>
        /// <param name="firstGamer">起始玩家</param>
        public static void SetTurnOrder(this OrderControllerComponent self, Gamer firstGamer)
        {
            Room room = self.GetParent<Room>();
            room.seats.TryGetValue(firstGamer.UserID, out int index);

            index -= 1;
            for (int i = 0; i < 5; i++)
            {
                if (index == 4)
                    index = -1;
                index += 1;
                if (room.gamers[index] == null)
                    continue;
                self.turnGamerOrder.Add(room.gamers[index]);
            }

            self.currentAuthority = firstGamer;
            self.currentPlayerOrder = 1;
        }

        /// <summary>
        /// 下一个玩家进行回合
        /// </summary>
        /// <param name="self"></param>
        public static void NextGamerTurn(this OrderControllerComponent self)
        {
            if (self.currentPlayerOrder == self.turnGamerOrder.Count)
            {
                self.PrepareNextRound();
                return;
            }

            self.currentPlayerOrder += 1;
            self.currentAuthority = self.turnGamerOrder[self.currentPlayerOrder - 1];
            Room room = self.GetParent<Room>();
            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                UserID = self.currentAuthority.UserID,
                Stage = (int)self.stage
            });
        }

        public static void PrepareNextBigRound(this OrderControllerComponent self)
        {
        }

        public static void PrepareNextRound(this OrderControllerComponent self)
        {
        }

        public static void PrepareNextPhase(this OrderControllerComponent self)
        {
        }
    }
}
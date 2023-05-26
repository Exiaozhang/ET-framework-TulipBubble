using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;


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
        public static void NextGamerTurn(this OrderControllerComponent self, bool ignoreNextRound = false)
        {
            if (self.currentPlayerOrder == self.turnGamerOrder.Count && !ignoreNextRound)
            {
                self.PrepareNextRound();
                return;
            }

            self.currentPlayerOrder += 1;
            if (self.currentPlayerOrder > self.turnGamerOrder.Count)
            {
                self.currentPlayerOrder = 1;
            }

            self.currentAuthority = self.turnGamerOrder[self.currentPlayerOrder - 1];

            Log.Info($"Current Player Order{self.currentPlayerOrder}");

            Room room = self.GetParent<Room>();
            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                UserID = self.currentAuthority.UserID,
                Stage = (int)self.stage
            });
        }

        /// <summary>
        /// 指定玩家进行竞价回合
        /// </summary>
        /// <param name="self"></param>
        /// <param name="userId"></param>
        public static void SetGamerBidTurn(this OrderControllerComponent self, Int64 userId, Int32 price)
        {
            Room room = self.GetParent<Room>();

            room.Broadcast(new Actor_NotifyPlayerBid()
            {
                UserId = userId,
                LowestPrice = price,
                BidingTulipCard = room.GetComponent<BidControllerComponent>().reserveTulip.ReserveTulipCard
            });
        }
        
        public static void PrepareNextBigRound(this OrderControllerComponent self)
        {
            self.currentBigRound += 1;
            self.PrepareNextPhase();
        }

        public static void PrepareNextRound(this OrderControllerComponent self)
        {
            Log.Info(self.stage.ToString());

            if (self.stage == GameStage.AuctionStage && self.currentBigRound == 1 && self.currentRound == 1)
            {
                self.currentRound += 1;
                self.NextGamerTurn(true);
                return;
            }

            if (self.stage == GameStage.AuctionStage)
            {
                self.FishThisPhase();
                return;
            }

            if (self.stage == GameStage.SellStage)
            {
                self.currentRound += 1;
                self.NextGamerTurn(true);
                return;
            }

            self.currentRound += 1;
            self.PrepareNextPhase();
        }

        public static void PrepareNextPhase(this OrderControllerComponent self)
        {
       
            Room room = self.GetParent<Room>();
            GameControllerComponent gameControllerComponent = room.GetComponent<GameControllerComponent>();

            Log.Info($"{self.stage}");

            self.currentRound = 1;

            int phase = (int)self.stage + 1;
            if (phase > 3)
                phase = 0;

            GameStage stage = (GameStage)(phase);
            self.stage = stage;

            Log.Info($"Next {self.stage}");

            if (self.stage == GameStage.EventStage)
            {
                gameControllerComponent.EnterEventStage();
                self.PrepareNextPhase();
                return;
            }

            if (self.stage == GameStage.SellStage)
            {
                gameControllerComponent.EnterSellStage();
                self.PrepareNextRound();
            }

            if (self.stage == GameStage.AuctionStage)
            {
                gameControllerComponent.EnterAuctionStage();
                self.PrepareNextRound();
            }

            if (self.stage == GameStage.FinishStage)
            {
                gameControllerComponent.EnterFishPhase();
                self.PrepareNextBigRound();
                return;
            }
        }

        public static void FishThisPhase(this OrderControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            GameControllerComponent gameControllerComponent = room.GetComponent<GameControllerComponent>();
            if (self.stage == GameStage.AuctionStage)
            {
                gameControllerComponent.FishAuctionStage();
            }
        }
    }
}
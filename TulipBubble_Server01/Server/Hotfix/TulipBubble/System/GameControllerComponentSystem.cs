using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [ObjectSystem]
    public class GamControllerComponentAwakeSystem : AwakeSystem<GameControllerComponent, RoomConfig>
    {
        public override void Awake(GameControllerComponent self, RoomConfig config)
        {
            self.Awake(config);
        }
    }

    public static class GameControllerComponentSystem
    {
        public static void Awake(this GameControllerComponent self, RoomConfig config)
        {
            self.Config = config;
            self.BasePointPerMatch = config.BasePointPerMatch;
            self.Multiples = config.Multiples;
            self.MinThreshold = config.MinThreshold;
        }

        /// <summary>
        /// 准备开始游戏
        /// </summary>
        /// <param name="self"></param>
        public static void StartGame(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            Gamer[] gamers = room.gamers;

            //初始玩家开始状态
            foreach (var gamer in gamers)
            {
                if (gamer == null)
                {
                    continue;
                }

                // 添加手牌组件
                if (gamer.GetComponent<HandCardsComponent>() == null)
                {
                    gamer.AddComponent<HandCardsComponent>();
                }

                // 添加经济组件
                if (gamer.GetComponent<MoneyComponent>() == null)
                {
                    gamer.AddComponent<MoneyComponent>();
                }

                // 添加竞价标志物组件
                if (gamer.GetComponent<ReserveSignComponent>() == null)
                {
                    gamer.AddComponent<ReserveSignComponent>();
                }
            }

            //洗牌发牌
            self.PrepareTulipCards();
            self.PrepareEventCards();
            self.PrepareCollectorCards();

            RoomCollectorCardsComponent roomCollectorCardsComponent = room.GetComponent<RoomCollectorCardsComponent>();
            RoomEventCardsComponent roomEventCardsComponent = room.GetComponent<RoomEventCardsComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();

            //向玩家发送当前回合的卡牌
            foreach (Gamer gamer in gamers)
            {
                if (gamer == null)
                    continue;

                ActorMessageSenderComponent actorProxyComponent =
                    Game.Scene.GetComponent<ActorMessageSenderComponent>();

                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorID);

                // 郁金香市场牌
                actorProxy.Send(new Actor_GameStartRoomCards_Ntt()
                {
                    FutureTulipCards =
                        MapHelper.To.RepeatedField(room.GetComponent<RoomTulipCardsComponent>().futureTulipCards),
                    CashTulipCards =
                        MapHelper.To.RepeatedField(room.GetComponent<RoomTulipCardsComponent>().cashTulipCards)
                });

                // 郁金香的嘉禾
                actorProxy.Send(new Actor_GetTulipPriceLevel_Ntt()
                {
                    RedPriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().redPrieceLevel,
                    WhitePriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().whitePrieceLevel,
                    YellowPriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().yellowPrieceLevel
                });

                // 收藏家牌
                actorProxy.Send(new Actor_GetCollector_Ntt()
                {
                    HighPriceCollector = roomCollectorCardsComponent.highPriceCollector,
                    HighPriceCollectorCount = roomCollectorCardsComponent.highPriceCollectorCount,
                    MiddlePriceCollector = roomCollectorCardsComponent.middlePriceCollector,
                    MiddlePriceCollectorCount = roomCollectorCardsComponent.middlePriceCollectorCount,
                    LowPriceCollector = roomCollectorCardsComponent.lowPriceCollector,
                    LowPriceCollectorCount = roomCollectorCardsComponent.lowPriceCollectorCount
                });

                // 事件卡
                actorProxy.Send(new Actor_GetEvent_Ntt()
                {
                    EventCard = roomEventCardsComponent.currentEvent,
                    RemindEventCount = roomEventCardsComponent.remainderEventCardCount
                });

                // 金币
                actorProxy.Send(new Actor_GetMoney_Ntt()
                {
                    Money = gamer.GetComponent<MoneyComponent>().money
                });

                // 竞价标志物
                actorProxy.Send(new Actor_GetSignCount_Ntt()
                {
                    SignCount = gamer.GetComponent<ReserveSignComponent>().count
                });

                //移除发出去的牌
                //actorProxy.Send();
            }

            //随机先手玩家
            Int64 userId = self.RandomFirstAuthority();
            self.CurrentGamerUserId = userId;

            //设置阶段、轮数、回合
            orderControllerComponent.stage = GameStage.AuctionStage;
            orderControllerComponent.currentRound = 1;
            orderControllerComponent.currentBigRound = 1;

            //通知玩家游戏游戏已经开始
            room.Broadcast(new Actor_GameStart { });

            //同步当前回合起始玩家
            room.Broadcast(new Actor_SyncFirstPlayer_Ntt()
            {
                UserID = self.CurrentGamerUserId
            });

            //同步当前回合玩家行动
            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                UserID = self.CurrentGamerUserId,
                Stage = (int)orderControllerComponent.stage
            });

            Log.Info($"Room{room.Id} Start Game");
        }

        /// <summary>
        /// 开始游戏时给房间补充郁金香
        /// </summary>
        /// <param name="self"></param>
        public static void PrepareTulipCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            //牌库洗郁金香牌
            room.GetComponent<DeckComponent>().ShuffleTulipCards();

            RoomTulipCardsComponent roomTulipCards = room.GetComponent<RoomTulipCardsComponent>();

            //清空已售
            roomTulipCards.selledTulipCards.Clear();
            //清空现货
            roomTulipCards.cashTulipCards.Clear();
            //清空进货
            roomTulipCards.futureTulipCards.Clear();

            for (int i = 0; i < self.TulipMount; i++)
            {
                DeckComponent deck = room.GetComponent<DeckComponent>();
                roomTulipCards.futureTulipCards.Add(deck.DealTulipCard());
                roomTulipCards.cashTulipCards.Add(deck.DealTulipCard());
            }
        }

        /// <summary>
        /// 开始游戏时给房间补充事件卡牌
        /// </summary>
        /// <param name="self"></param>
        public static void PrepareEventCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            //牌库洗事件牌
            DeckComponent deck = room.GetComponent<DeckComponent>();
            deck.ShuffleEventCard();
            room.GetComponent<RoomEventCardsComponent>().remainderEventCardCount = deck.EventCardsCount;
        }

        /// <summary>
        /// 开始游戏时给房间补充收藏家卡牌
        /// </summary>
        /// <param name="self"></param>
        public static void PrepareCollectorCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            DeckComponent deck = room.GetComponent<DeckComponent>();
            RoomCollectorCardsComponent roomCollectorCardsComponent = room.GetComponent<RoomCollectorCardsComponent>();

            //洗收藏家牌
            deck.ShuffleCollectorCard();

            //发牌给收藏家组件
            roomCollectorCardsComponent.highPriceCollector = deck.DealHighPriceCollectorCard();
            roomCollectorCardsComponent.highPriceCollectorCount = deck.HighPriceCollectorCount;
            roomCollectorCardsComponent.middlePriceCollector = deck.DealMiddlePriceCollectorCard();
            roomCollectorCardsComponent.middlePriceCollectorCount = deck.MiddlePriceCollectorCount;
            roomCollectorCardsComponent.lowPriceCollector = deck.DealLowPriceCollectorCard();
            roomCollectorCardsComponent.lowPriceCollectorCount = deck.LowPriceCollectorCount;
        }

        public static void EnterEventStage(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            RoomEventCardsComponent roomEventCardsComponent = room.GetComponent<RoomEventCardsComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();
            DeckComponent deckComponent = room.GetComponent<DeckComponent>();

            EventCard dealEventCard = deckComponent.DealEventCard();
            roomEventCardsComponent.SetCurrentEvent(dealEventCard);
            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                Stage = (int)orderControllerComponent.stage
            });
        }

        public static void EnterSellStage(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();

            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                Stage = (int)orderControllerComponent.stage
            });
        }

        public static void EnterAuctionStage(this GameControllerComponent self)
        {
        }

        public static void EnterFishPhase(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            RoomTulipCardsComponent roomTulipCardsComponent = room.GetComponent<RoomTulipCardsComponent>();
            TulipMarketEconomicsComponent tulipMarketEconomicsComponent =
                room.GetComponent<TulipMarketEconomicsComponent>();
            DeckComponent deckComponent = room.GetComponent<DeckComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();

            int leastTulipColor = roomTulipCardsComponent.FindLeastTulipColor();
            int mostTulipColor = roomTulipCardsComponent.FindMostTulipColor();

            tulipMarketEconomicsComponent.DropTulipPriceLevel(mostTulipColor);
            tulipMarketEconomicsComponent.RiseTulipPriceLevel(leastTulipColor);

            roomTulipCardsComponent.PutTulipCardToDiscardPile(roomTulipCardsComponent.selledTulipCards);
            roomTulipCardsComponent.selledTulipCards.AddRange(roomTulipCardsComponent.cashTulipCards);
            roomTulipCardsComponent.cashTulipCards.Clear();
            roomTulipCardsComponent.cashTulipCards.AddRange(roomTulipCardsComponent.futureTulipCards);
            roomTulipCardsComponent.futureTulipCards.Clear();

            for (int i = 0; i < self.TulipMount; i++)
            {
                TulipCard card = deckComponent.DealTulipCard();
                roomTulipCardsComponent.AddFutureCard(card);
            }

            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                Stage = (int)orderControllerComponent.stage
            });

            room.Broadcast(new Actor_GetTulipPriceLevel_Ntt()
            {
                RedPriceLevel = tulipMarketEconomicsComponent.redPrieceLevel,
                WhitePriceLevel = tulipMarketEconomicsComponent.whitePrieceLevel,
                YellowPriceLevel = tulipMarketEconomicsComponent.yellowPrieceLevel
            });

            room.Broadcast(new Actor_GetTulip_Ntt()
            {
                FutureTulipCards = MapHelper.To.RepeatedField(roomTulipCardsComponent.futureTulipCards),
                CashTulipCards = MapHelper.To.RepeatedField(roomTulipCardsComponent.cashTulipCards),
                SelledTulipCards = MapHelper.To.RepeatedField(roomTulipCardsComponent.selledTulipCards)
            });
        }

        public static void FishAuctionStage(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            BidControllerComponent bidControllerComponent = room.GetComponent<BidControllerComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();

            room.Broadcast(new Actor_AuthorityPlayCard_Ntt()
            {
                UserID = 0,
                Stage = (int)orderControllerComponent.stage
            });

            bidControllerComponent.StartBid();
        }

        /// <summary>
        /// 进行游戏时给房间补充郁金香牌
        /// </summary>
        /// <param name="self"></param>
        public static void DealTulipCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            RoomTulipCardsComponent roomTulipCards = room.GetComponent<RoomTulipCardsComponent>();

            //清空已售的卡牌
            roomTulipCards.selledTulipCards.Clear();
            //把现货移动到已售
            roomTulipCards.selledTulipCards.AddRange(roomTulipCards.cashTulipCards);
            //清空现货
            roomTulipCards.cashTulipCards.Clear();
            //把进货移动到现货
            roomTulipCards.cashTulipCards.AddRange(roomTulipCards.futureTulipCards);
            //清空进货
            roomTulipCards.futureTulipCards.Clear();

            for (int i = 0; i < self.TulipMount; i++)
            {
                TulipCard tulipCard = room.GetComponent<DeckComponent>().DealTulipCard();
                roomTulipCards.futureTulipCards.Add(tulipCard);
            }
        }

        /// <summary>
        /// 给房间补充郁金香牌房间
        /// </summary>
        /// <param name="id"></param>
        public static void DealTo(this GameControllerComponent self, long id)
        {
            Room room = self.GetParent<Room>();
            TulipCard tulipCard = room.GetComponent<DeckComponent>().DealTulipCard();

            foreach (var gamer in room.gamers)
            {
                if (id == gamer.UserID)
                {
                    gamer.GetComponent<HandCardsComponent>().AddCard(tulipCard);
                    break;
                }
            }
        }

        /// <summary>
        /// 随机起始玩家
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Int64 RandomFirstAuthority(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();

            //随机先手玩家座位
            Random random = new Random();
            Int32 startIndex = random.Next(0, 5);
            Int32 index = startIndex;

            while (room.gamers[index] == null)
            {
                if (index == 4)
                    index = 0;

                index += 1;
                if (index == startIndex)
                {
                    Log.Info("Error Room Dont Have Any Gamer");
                    return 0;
                }
            }

            //确定先手玩家
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();
            orderControllerComponent.SetTurnOrder(room.gamers[index]);

            return orderControllerComponent.currentAuthority.UserID;
        }
    }
}
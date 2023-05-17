using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETModel;

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

                if (gamer.GetComponent<HandCardsComponent>() == null)
                {
                    gamer.AddComponent<HandCardsComponent>();
                }
            }

            //洗牌发牌
            self.PrepareTulipCards();
            self.PrepareEventCards();
            self.PrepareCollectorCards();

            RoomCollectorCardsComponent roomCollectorCardsComponent = room.GetComponent<RoomCollectorCardsComponent>();
            RoomEventCardsComponent roomEventCardsComponent = room.GetComponent<RoomEventCardsComponent>();
            OrderControllerComponent orderControllerComponent = room.GetComponent<OrderControllerComponent>();
            
            foreach (Gamer gamer in gamers)
            {
                if (gamer == null)
                    continue;

                ActorMessageSenderComponent actorProxyComponent =
                    Game.Scene.GetComponent<ActorMessageSenderComponent>();
                Log.Info($"{gamer.CActorID == null}");
                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorID);

                actorProxy.Send(new Actor_GameStartRoomCards_Ntt()
                {
                    FutureTulipCards =
                        MapHelper.To.RepeatedField(room.GetComponent<RoomTulipCardsComponent>().futureTulipCards),
                    CashTulipCards =
                        MapHelper.To.RepeatedField(room.GetComponent<RoomTulipCardsComponent>().cashTulipCards)
                });

                actorProxy.Send(new Actor_GetTulipPriceLevel_Ntt()
                {
                    RedPriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().redPrieceLevel,
                    WhitePriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().whitePrieceLevel,
                    YellowPriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().yellowPrieceLevel
                });

                actorProxy.Send(new Actor_GetCollector_Ntt()
                {
                    HighPriceCollector = roomCollectorCardsComponent.highPriceCollector,
                    HighPriceCollectorCount = roomCollectorCardsComponent.highPriceCollectorCount,
                    MiddlePriceCollector = roomCollectorCardsComponent.middlePriceCollector,
                    MiddlePriceCollectorCount = roomCollectorCardsComponent.middlePriceCollectorCount,
                    LowPriceCollector = roomCollectorCardsComponent.lowPriceCollector,
                    LowPriceCollectorCount = roomCollectorCardsComponent.lowPriceCollectorCount
                });

                actorProxy.Send(new Actor_GetEvent_Ntt()
                {
                    EventCard = roomEventCardsComponent.currentEvent,
                    RemindEventCount = roomEventCardsComponent.remainderEventCardCount
                });

                //移除发出去的牌
                //actorProxy.Send();
            }

            //随机先手玩家
            Int64 userId = self.RandomFirstAuthority();
            self.CurrentGamerUserId = userId;
            orderControllerComponent.stage = GameStage.AuctionStage;

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
            Log.Info(index.ToString());

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
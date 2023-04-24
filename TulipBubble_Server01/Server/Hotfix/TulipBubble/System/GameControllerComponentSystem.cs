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
            
            foreach (Gamer gamer in gamers)
            {
                if(gamer==null)
                    continue;
                
                ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                Log.Info($"{gamer.CActorID==null}");
                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorID);
                
                actorProxy.Send(new Actor_GameStartRoomCards_Ntt()
                {
                    FutureTulipCards = MapHelper.To.RepeatedField(room.GetComponent<RoomCardsComponent>().futureTulipCards),
                    CashTulipCards = MapHelper.To.RepeatedField(room.GetComponent<RoomCardsComponent>().cashTulipCards)
                });
                
                actorProxy.Send(new Actor_GetTulipPriceLevel_Ntt()
                {
                    RedPriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().redPrieceLevel,
                    WhitePriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().whitePrieceLevel,
                    YellowPriceLevel = room.GetComponent<TulipMarketEconomicsComponent>().yellowPrieceLevel
                });
            }
            //随机先手玩家
            
            Log.Info($"Room{room.Id} Start Game");
        }

        /// <summary>
        /// 开始游戏时给房间补充郁金香
        /// </summary>
        /// <param name="self"></param>
        public static void PrepareTulipCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            //牌库洗牌
            room.GetComponent<DeckComponent>().Shuffle();

            RoomCardsComponent roomCards = room.GetComponent<RoomCardsComponent>();

            //清空已售
            roomCards.selledTulipCards.Clear();
            //清空现货
            roomCards.cashTulipCards.Clear();
            //清空进货
            roomCards.futureTulipCards.Clear();

            for (int i = 0; i < self.TulipMount; i++)
            {
                DeckComponent deck = room.GetComponent<DeckComponent>();
                roomCards.futureTulipCards.Add(deck.Deal());
                roomCards.cashTulipCards.Add(deck.Deal());
            }
        }

        /// <summary>
        /// 进行游戏时给房间补充郁金香牌
        /// </summary>
        /// <param name="self"></param>
        public static void DealTulipCards(this GameControllerComponent self)
        {
            Room room = self.GetParent<Room>();
            RoomCardsComponent roomCards = room.GetComponent<RoomCardsComponent>();

            //清空已售的卡牌
            roomCards.selledTulipCards.Clear();
            //把现货移动到已售
            roomCards.selledTulipCards.AddRange(roomCards.cashTulipCards);
            //清空现货
            roomCards.cashTulipCards.Clear();
            //把进货移动到现货
            roomCards.cashTulipCards.AddRange(roomCards.futureTulipCards);
            //清空进货
            roomCards.futureTulipCards.Clear();

            for (int i = 0; i < self.TulipMount; i++)
            {
                TulipCard tulipCard = room.GetComponent<DeckComponent>().Deal();
                roomCards.futureTulipCards.Add(tulipCard);
            }
        }

        /// <summary>
        /// 给房间补充郁金香牌房间
        /// </summary>
        /// <param name="id"></param>
        public static void DealTo(this GameControllerComponent self, long id)
        {
            Room room = self.GetParent<Room>();
            TulipCard tulipCard = room.GetComponent<DeckComponent>().Deal();

            foreach (var gamer in room.gamers)
            {
                if (id == gamer.UserID)
                {
                    gamer.GetComponent<HandCardsComponent>().AddCard(tulipCard);
                    break;
                }
            }
        }
    }
}
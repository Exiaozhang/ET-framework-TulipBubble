using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class DeckComponentAwakeSystem : AwakeSystem<DeckComponent>
    {
        public override void Awake(DeckComponent self)
        {
            self.Awake();
        }
    }

    public static class DeckComponentSystem
    {
        public static void Awake(this DeckComponent self)
        {
            self.CreateTulipDeck();
            self.CreateEventDeck();
            self.CreateCollectorDeck();
        }


        /// <summary>
        /// 郁金香牌库洗牌
        /// </summary>
        /// <param name="self"></param>
        public static void ShuffleTulipCards(this DeckComponent self)
        {
            if (self.TulipCardsCount > 0)
            {
                Random random = new Random();
                List<TulipCard> newTulipCards = new List<TulipCard>();
                foreach (TulipCard tulipCard in self.tulipLibray)
                {
                    newTulipCards.Insert(random.Next(newTulipCards.Count + 1), tulipCard);
                }

                self.tulipLibray.Clear();
                self.tulipLibray.AddRange(newTulipCards);
            }
        }

        /// <summary>
        /// 事件牌库洗牌
        /// </summary>
        /// <param name="self"></param>
        public static void ShuffleEventCard(this DeckComponent self)
        {
            if (self.EventCardsCount > 0)
            {
                Random random = new Random();
                List<EventCard> newEventCards = new List<EventCard>();
                foreach (EventCard eventCard in self.eventLibray)
                {
                    newEventCards.Insert(random.Next(newEventCards.Count + 1), eventCard);
                }

                EventCard bubbleBurstEventCard = EventCard.Create((int)MarketEvent.BubbleBurst);
                newEventCards.Insert(random.Next(3), bubbleBurstEventCard);

                self.eventLibray.Clear();
                self.eventLibray.AddRange(newEventCards);
            }
        }

        /// <summary>
        /// 郁金香牌库洗牌
        /// </summary>
        /// <param name="self"></param>
        public static void ShuffleCollectorCard(this DeckComponent self)
        {
            if (self.HighPriceCollectorCount > 1)
            {
                List<CollectorCard> newHighPriceCollectorCards = new List<CollectorCard>();
                Random random = new Random();
                foreach (CollectorCard collectorCard in self.collectorLibrayHighPrice)
                {
                    newHighPriceCollectorCards.Insert(random.Next(newHighPriceCollectorCards.Count + 1), collectorCard);
                }

                self.collectorLibrayHighPrice.Clear();
                self.collectorLibrayHighPrice.AddRange(newHighPriceCollectorCards);
            }

            if (self.MiddlePriceCollectorCount > 1)
            {
                List<CollectorCard> newMiddlePriceCollectorCards = new List<CollectorCard>();
                Random random = new Random();

                foreach (CollectorCard collectorCard in self.collectorLibrayMiddlePrice)
                {
                    newMiddlePriceCollectorCards.Insert(random.Next(newMiddlePriceCollectorCards.Count + 1),
                        collectorCard);
                }

                self.collectorLibrayMiddlePrice.Clear();
                self.collectorLibrayMiddlePrice.AddRange(newMiddlePriceCollectorCards);
            }

            if (self.LowPriceCollectorCount > 1)
            {
                List<CollectorCard> newLowPriceCollectorCards = new List<CollectorCard>();
                Random random = new Random();
                foreach (CollectorCard collectorCard in self.collectorLibrayLowPrice)
                {
                    newLowPriceCollectorCards.Insert(random.Next(newLowPriceCollectorCards.Count + 1), collectorCard);
                }

                self.collectorLibrayLowPrice.Clear();
                self.collectorLibrayLowPrice.AddRange(newLowPriceCollectorCards);
            }
        }

        /// <summary>
        /// 牌库发一张郁金香牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static TulipCard DealTulipCard(this DeckComponent self)
        {
            TulipCard tulipCard = self.tulipLibray[self.TulipCardsCount - 1];
            self.tulipLibray.Remove(tulipCard);
            //Log.Info(tulipCard.GetName());
            return tulipCard;
        }

        /// <summary>
        /// 牌库发一张事件牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static EventCard DealEventCard(this DeckComponent self)
        {
            EventCard eventCard = self.eventLibray[self.EventCardsCount - 1];
            self.eventLibray.Remove(eventCard);
            return eventCard;
        }

        /// <summary>
        /// 发一张高价位收藏家牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static CollectorCard DealHighPriceCollectorCard(this DeckComponent self)
        {
            CollectorCard collectorCard = self.collectorLibrayHighPrice[self.HighPriceCollectorCount - 1];
            self.collectorLibrayHighPrice.Remove(collectorCard);
            return collectorCard;
        }

        /// <summary>
        /// 发一张中价位收藏家牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static CollectorCard DealMiddlePriceCollectorCard(this DeckComponent self)
        {
            CollectorCard collectorCard = self.collectorLibrayMiddlePrice[self.MiddlePriceCollectorCount - 1];
            self.collectorLibrayMiddlePrice.Remove(collectorCard);
            return collectorCard;
        }

        /// <summary>
        /// 发一张低价位收藏家牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static CollectorCard DealLowPriceCollectorCard(this DeckComponent self)
        {
            CollectorCard collectorCard = self.collectorLibrayLowPrice[self.LowPriceCollectorCount - 1];
            self.collectorLibrayLowPrice.Remove(collectorCard);
            return collectorCard;
        }


        /// <summary>
        /// 向郁金香牌库中添加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddTulipCard(this DeckComponent self, TulipCard tulipCard)
        {
            self.tulipLibray.Add(tulipCard);
        }

        /// <summary>
        /// 向事件牌库中添加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eventCard"></param>
        public static void AddEventCard(this DeckComponent self, EventCard eventCard)
        {
            self.eventLibray.Add(eventCard);
        }

        /// <summary>
        /// 创建一副郁金香牌
        /// </summary>
        private static void CreateTulipDeck(this DeckComponent self)
        {
            //创建普通郁金香
            for (int color = 0; color < 3; color++) //创建每种郁金香花色的牌
            {
                for (int level = 0; level < 3; level++) //每种花色创建3种等级的郁金香拍
                {
                    for (int weight = 0; weight < level + 1; weight++) //每种等级创建对应的种类 a->1 b->2 c->3
                    {
                        for (int i = 0; i < 3; i++) //创建3个相同的郁金香牌
                        {
                            TulipCard tulipCard = TulipCard.Create(color, level, weight);
                            self.tulipLibray.Add(tulipCard);
                        }
                    }
                }
            }

            //创建黑色郁金香
            //self.tulipLibray.Add(TulipCard.Create((int)Color.Black, (int)Level.None, (int)Weight.None));
        }

        /// <summary>
        /// 创建一副事件牌
        /// </summary>
        /// <param name="self"></param>
        private static void CreateEventDeck(this DeckComponent self)
        {
            //事件牌堆每个颜色都有2个上涨和下跌
            //3个暴跌1个暴涨
            //1个泡沫破裂

            for (int color = 0; color < 3; color++)
            {
                for (int type = 0; type < 2; type++)
                {
                    for (int i = 0; i < 2; i++)
                        self.eventLibray.Add(EventCard.Create(type, color));
                }
            }

            self.eventLibray.Add(EventCard.Create((int)MarketEvent.RiseSuddenly));

            for (int i = 0; i < 3; i++)
                self.eventLibray.Add(EventCard.Create((int)MarketEvent.DropSuddenly));
        }

        /// <summary>
        /// 创建一副收藏家牌
        /// </summary>
        /// <param name="self"></param>
        private static void CreateCollectorDeck(this DeckComponent self)
        {
            self.collectorLibrayHighPrice.Add(CollectorCard.Create((int)RequestColor.PureColor,
                new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.A,
                    TulipCardWeight = (int)Weight.one
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.one
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.two
                }, 20, "Nobleman"));

            self.collectorLibrayMiddlePrice.Add(CollectorCard.Create((int)RequestColor.PureColor,
                new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.Anyone
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.Anyone
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.Anyone
                }, 15, "Clergyman"));

            self.collectorLibrayMiddlePrice.Add(CollectorCard.Create((int)RequestColor.PureColor,
                new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.one
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.two
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.Anyone
                }, 15, "FairLady"));

            self.collectorLibrayMiddlePrice.Add(CollectorCard.Create((int)RequestColor.DiverseColor,
                new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.Anyone
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.Anyone
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.B,
                    TulipCardWeight = (int)Weight.Anyone
                }, 15, "Madame"));

            self.collectorLibrayLowPrice.Add(CollectorCard.Create((int)RequestColor.PureColor,
                new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.one
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.two
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.three
                }, 10, "Servant"));

            self.collectorLibrayLowPrice.Add(CollectorCard.Create((int)RequestColor.PureColor,
                new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.Anyone
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.Anyone
                }, new CollectorTulipCard()
                {
                    TulipCardLevel = (int)Level.C,
                    TulipCardWeight = (int)Weight.Anyone
                }, 10, "YoungMan"));

            self.collectorLibrayLowPrice.Add(
                CollectorCard.Create((int)RequestColor.DiverseColor,
                    new CollectorTulipCard()
                    {
                        TulipCardLevel = (int)Level.C,
                        TulipCardWeight = (int)Weight.Anyone
                    }, new CollectorTulipCard()
                    {
                        TulipCardLevel = (int)Level.C,
                        TulipCardWeight = (int)Weight.Anyone
                    }, new CollectorTulipCard()
                    {
                        TulipCardLevel = (int)Level.C,
                        TulipCardWeight = (int)Weight.Anyone
                    }, 10, "TavernOwner"));

            self.collectorLibrayLowPrice.Add(
                CollectorCard.Create((int)RequestColor.DiverseColor,
                    new CollectorTulipCard()
                    {
                        TulipCardLevel = (int)Level.X,
                        TulipCardWeight = (int)Weight.Y
                    }, new CollectorTulipCard()
                    {
                        TulipCardLevel = (int)Level.X,
                        TulipCardWeight = (int)Weight.Y
                    }, new CollectorTulipCard()
                    {
                        TulipCardLevel = (int)Level.X,
                        TulipCardWeight = (int)Weight.Y
                    }, 10,"Scholar")
            );
        }
    }
}
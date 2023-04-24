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
        }


        /// <summary>
        /// 郁金香牌库洗牌
        /// </summary>
        /// <param name="self"></param>
        public static void Shuffle(this DeckComponent self)
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
        /// 牌库发一张郁金香牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static TulipCard Deal(this DeckComponent self)
        {
            TulipCard tulipCard = self.tulipLibray[self.TulipCardsCount - 1];
            self.tulipLibray.Remove(tulipCard);
            //Log.Info(tulipCard.GetName());
            return tulipCard;
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
    }
}
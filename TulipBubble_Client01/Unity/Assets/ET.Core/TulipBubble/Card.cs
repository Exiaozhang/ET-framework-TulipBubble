using System;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 参考Unit类
    /// </summary>
    public partial class TulipCard
    {
        public CardBelongType BelongType;

        public GameObject CardObj;

        public bool isLoanCard;

        public Int32 loanMoney; 

        public static TulipCard Create(int color, int level, int weight)
        {
            TulipCard tulipCard = new TulipCard();
            tulipCard.TulipCardColor = color;
            tulipCard.TulipCardLevel = level;
            tulipCard.TulipCardWeight = weight;

            return tulipCard;
        }

        public bool Equals(TulipCard other)
        {
            return this.TulipCardLevel == other.TulipCardLevel && this.TulipCardColor == other.TulipCardColor &&
                   this.TulipCardWeight == other.TulipCardWeight;
        }

        /// <summary>
        /// 获取郁金香卡牌名
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.TulipCardColor == (int)Color.Black
                ? $"{((Color)this.TulipCardColor).ToString()}"
                : $"{((Color)this.TulipCardColor).ToString()}{((Level)this.TulipCardLevel).ToString()}{((Weight)this.TulipCardWeight).ToString()}";
        }
    }


    /// <summary>
    /// 收藏家卡牌
    /// </summary>
    public partial class CollectorCard
    {
        public CardBelongType BelongType;

        /// <summary>
        /// 创建一个收藏家牌
        /// </summary>
        /// <param name="reqColor">要求的颜色类型</param>
        /// <param name="reqFirstType"></param>
        /// <param name="reqSecondType"></param>
        /// <param name="reqThirdType"></param>
        /// <param name="price">收购价格</param>
        /// <returns></returns>
        public static CollectorCard Create(int reqColor, CollectorTulipCard reqFirstType,
            CollectorTulipCard reqSecondType, CollectorTulipCard reqThirdType, int price)
        {
            CollectorCard collectorCard = new CollectorCard();
            collectorCard.RequestColor = reqColor;

            collectorCard.CollectorTulipCard.Add(reqFirstType);
            collectorCard.CollectorTulipCard.Add(reqSecondType);
            collectorCard.CollectorTulipCard.Add(reqThirdType);

            collectorCard.Price = price;
            return collectorCard;
        }


        /// <summary>
        /// 得到卡牌的名字
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.Name;
        }
    }

    /// <summary>
    /// 事件卡牌
    /// </summary>
    public partial class EventCard
    {
        public static EventCard Create(int eventType, int color)
        {
            EventCard eventCard = new EventCard();
            eventCard.EventType = eventType;
            eventCard.TulipColor = color;
            return eventCard;
        }

        /// <summary>
        /// 得到卡牌的名字
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            string marketEvent = ((MarketEvent)this.EventType).ToString();
            string tulipColor = ((Color)this.TulipColor).ToString();
            Log.Info(marketEvent + tulipColor);
            return marketEvent + tulipColor;
        }
    }
}
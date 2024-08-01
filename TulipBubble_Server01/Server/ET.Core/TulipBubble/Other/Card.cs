using System;
using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 参考Unit类
    /// </summary>
    public partial class TulipCard
    {
        public static TulipCard Create(int color, int level, int weight, int Id)
        {
            TulipCard tulipCard = new TulipCard();
            tulipCard.TulipCardColor = color;
            tulipCard.TulipCardLevel = level;
            tulipCard.TulipCardWeight = weight;
            tulipCard.Id = Id;
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
            return this.TulipCardColor == (int)TulipColor.Black
                ? $"{((TulipColor)this.TulipCardColor).ToString()}"
                : $"{((TulipColor)this.TulipCardColor).ToString()}{((Level)this.TulipCardLevel).ToString()}{((Weight)this.TulipCardWeight).ToString()}";
        }
    }

    public partial class CollectorCard
    {
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
            CollectorTulipCard reqSecondType, CollectorTulipCard reqThirdType, int price, string name)
        {
            CollectorCard collectorCard = new CollectorCard();
            collectorCard.RequestColor = reqColor;
            collectorCard.Name = name;
            collectorCard.CollectorTulipCard.Add(reqFirstType);
            collectorCard.CollectorTulipCard.Add(reqSecondType);
            collectorCard.CollectorTulipCard.Add(reqThirdType);
            collectorCard.Price = price;
            collectorCard.Id = DateTime.Now.Ticks;
            return collectorCard;
        }
    }


    public partial class EventCard
    {
        public static EventCard Create(int type, int color)
        {
            //type 0,1 郁金香 涨/跌 2,3 暴涨/跌 4泡沫破裂

            EventCard eventCard = new EventCard();
            eventCard.EventType = type;
            eventCard.TulipColor = color;
            eventCard.Id = DateTime.Now.Ticks;
            return eventCard;
        }

        public static EventCard Create(int type)
        {
            EventCard eventCard = new EventCard();
            eventCard.EventType = type;
            eventCard.Id = DateTime.Now.Ticks;
            return eventCard;
        }
    }
}
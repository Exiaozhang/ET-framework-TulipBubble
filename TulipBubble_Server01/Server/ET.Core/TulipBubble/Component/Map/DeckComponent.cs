using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 牌库组件
    /// </summary>
    public class DeckComponent : Component
    {
        //牌库中的郁金香牌
        public readonly List<TulipCard> tulipLibray = new List<TulipCard>();

        //牌堆中的事件牌
        public readonly List<EventCard> eventLibray = new List<EventCard>();

        //牌堆中的收藏家牌
        public readonly List<CollectorCard> collectorLibrayHighPrice = new List<CollectorCard>();
        public readonly List<CollectorCard> collectorLibrayMiddlePrice = new List<CollectorCard>();
        public readonly List<CollectorCard> collectorLibrayLowPrice = new List<CollectorCard>();
        


        //牌库中的郁金香总牌数
        public int TulipCardsCount => this.tulipLibray.Count;
        
        //牌库中事件牌总数
        public int EventCardsCount => this.eventLibray.Count;

        //高/中/低价位收藏家数量
        public int HighPriceCollectorCount => this.collectorLibrayHighPrice.Count;
        public int MiddlePriceCollectorCount => this.collectorLibrayMiddlePrice.Count;
        public int LowPriceCollectorCount => this.collectorLibrayLowPrice.Count;
        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            tulipLibray.Clear();
        }
    }
}
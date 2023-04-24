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
        
        //牌库中的郁金香总牌数
        public int TulipCardsCount => this.tulipLibray.Count;

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
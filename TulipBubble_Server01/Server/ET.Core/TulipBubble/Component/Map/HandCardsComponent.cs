using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 玩家的手牌组件
    /// </summary>
    public class HandCardsComponent : Component
    {
        //所有手牌
        public readonly List<TulipCard> tulipLibrary = new List<TulipCard>();
        
        //手牌数
        public int tulipCardsCount => tulipLibrary.Count;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            
            this.tulipLibrary.Clear();
        }
    }
}
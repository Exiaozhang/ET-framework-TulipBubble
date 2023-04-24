using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 房间市场卡牌组件
    /// </summary>
    public class RoomCardsComponent : Component
    {
        //进货
        public readonly List<TulipCard> futureTulipCards = new List<TulipCard>();
        public int futureTulipCardsCount => futureTulipCards.Count;
        
        //现货
        public readonly List<TulipCard> cashTulipCards = new List<TulipCard>();
        public int cashTulipCardsCount => cashTulipCards.Count;
        
        //出售
        public readonly List<TulipCard> selledTulipCards = new List<TulipCard>();
        public int selledTulipCardsCount => selledTulipCards.Count;

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
    }
}
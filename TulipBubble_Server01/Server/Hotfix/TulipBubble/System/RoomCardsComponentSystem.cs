using ETModel;

namespace ETHotfix
{
    public static class RoomCardsComponentSystem
    {
        
        /// <summary>
        /// 向进货牌种加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddFutureCard(this RoomCardsComponent self, TulipCard tulipCard)
        {
            self.futureTulipCards.Add(tulipCard);
        }

        /// <summary>
        /// 向现货牌中加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddCashCard(this RoomCardsComponent self, TulipCard tulipCard)
        {
            self.cashTulipCards.Add(tulipCard);
        }

        /// <summary>
        /// 向售出牌中加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddSelledCard(this RoomCardsComponent self, TulipCard tulipCard)
        {
            self.selledTulipCards.Add(tulipCard);
        }

        /// <summary>
        /// 购买移除牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void PopCashCard(this RoomCardsComponent self, TulipCard tulipCard)
        {
            self.cashTulipCards.Remove(tulipCard);
        }

        /// <summary>
        /// 购买移除牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void PopSelledCard(this RoomCardsComponent self, TulipCard tulipCard)
        {
            self.selledTulipCards.Remove(tulipCard);
        }
    }
}
using ETModel;

namespace ETHotfix
{
    public static class RoomTulipCardsComponentSystem
    {
        /// <summary>
        /// 向进货牌种加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddFutureCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            self.futureTulipCards.Add(tulipCard);
        }

        /// <summary>
        /// 向现货牌中加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddCashCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            self.cashTulipCards.Add(tulipCard);
        }

        /// <summary>
        /// 向售出牌中加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddSelledCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            self.selledTulipCards.Add(tulipCard);
        }

        /// <summary>
        /// 购买移除牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void PopCashCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            self.cashTulipCards.Remove(tulipCard);
        }

        /// <summary>
        /// 购买移除牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void PopSelledCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            self.selledTulipCards.Remove(tulipCard);
        }

        /// <summary>
        /// 在房间郁金香牌堆中查找郁金香牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static TulipCard SearchRoomTulipCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            TulipCard findCashCard = self.cashTulipCards.Find((cashTulipCard) =>
            {
                if (cashTulipCard.Id == tulipCard.Id)
                    return true;
                return false;
            });

            if (findCashCard != null)
                return findCashCard;

            TulipCard findSelledTulipCard = self.selledTulipCards.Find((cashTulipCard) =>
            {
                if (cashTulipCard.Id == tulipCard.Id)
                    return true;
                return false;
            });

            if (findSelledTulipCard != null)
                return findSelledTulipCard;

            TulipCard findFutureCard = self.futureTulipCards.Find((futureTulipCard) =>
            {
                if (futureTulipCard.Id == tulipCard.Id)
                    return true;
                return false;
            });

            if (findFutureCard != null)
                return findFutureCard;

            return null;
        }


        public static void ReserveTulipCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            TulipCard searchRoomTulipCard = self.SearchRoomTulipCard(tulipCard);
            if (searchRoomTulipCard == null)
                return;
            self.reservedTulipCards.Add(tulipCard);
        }
    }
}
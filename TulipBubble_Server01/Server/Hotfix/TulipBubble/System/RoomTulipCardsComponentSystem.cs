using System;
using System.Collections.Generic;
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


        public static void PopCard(this RoomTulipCardsComponent self, TulipCard tulipCard)
        {
            if (!self.selledTulipCards.Remove(tulipCard))
                self.cashTulipCards.Remove(tulipCard);
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

        public static void ReserveTulipCard(this RoomTulipCardsComponent self, TulipCard tulipCard, Gamer gamer)
        {
            TulipCard searchRoomTulipCard = self.SearchRoomTulipCard(tulipCard);
            if (searchRoomTulipCard == null)
                return;

            GamerReserveTulip gamerReserveTulip = self.reservedTulipCards.Find(tulip =>
            {
                if (tulip.ReserveTulipCard.Id == tulipCard.Id)
                    return true;
                return false;
            });

            if (gamerReserveTulip != null)
            {
                gamerReserveTulip.UserId.Add(gamer.UserID);
                return;
            }

            self.reservedTulipCards.Add(new GamerReserveTulip()
            {
                UserId = MapHelper.To.RepeatedField(new List<long>() { gamer.UserID }),
                ReserveTulipCard = tulipCard
            });
        }

        public static void PutTulipCardToDiscardPile(this RoomTulipCardsComponent self, List<TulipCard> tulipCards)
        {
            self.discardTulipCards.AddRange(tulipCards);
            tulipCards.Clear();
        }

        public static int FindMostTulipColor(this RoomTulipCardsComponent self)
        {
            int redCount = 0;
            int whiteCount = 0;
            int yellowCount = 0;

            self.futureTulipCards.ForEach((tulipCard) =>
            {
                if (tulipCard.TulipCardColor == (int)TulipColor.Red)
                    redCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.White)
                    whiteCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.Yellow)
                    yellowCount += 1;
            });

            self.cashTulipCards.ForEach((tulipCard) =>
            {
                if (tulipCard.TulipCardColor == (int)TulipColor.Red)
                    redCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.White)
                    whiteCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.Yellow)
                    yellowCount += 1;
            });

            self.selledTulipCards.ForEach((tulipCard) =>
            {
                if (tulipCard.TulipCardColor == (int)TulipColor.Red)
                    redCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.White)
                    whiteCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.Yellow)
                    yellowCount += 1;
            });

            if (redCount >= whiteCount || redCount >= yellowCount)
                return (int)TulipColor.Red;

            if (whiteCount >= redCount || whiteCount >= yellowCount)
                return (int)TulipColor.White;

            return (int)TulipColor.Yellow;
        }

        public static int FindLeastTulipColor(this RoomTulipCardsComponent self)
        {
            int redCount = 0;
            int whiteCount = 0;
            int yellowCount = 0;

            self.futureTulipCards.ForEach((tulipCard) =>
            {
                if (tulipCard.TulipCardColor == (int)TulipColor.Red)
                    redCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.White)
                    whiteCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.Yellow)
                    yellowCount += 1;
            });

            self.cashTulipCards.ForEach((tulipCard) =>
            {
                if (tulipCard.TulipCardColor == (int)TulipColor.Red)
                    redCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.White)
                    whiteCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.Yellow)
                    yellowCount += 1;
            });

            self.selledTulipCards.ForEach((tulipCard) =>
            {
                if (tulipCard.TulipCardColor == (int)TulipColor.Red)
                    redCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.White)
                    whiteCount += 1;
                else if (tulipCard.TulipCardColor == (int)TulipColor.Yellow)
                    yellowCount += 1;
            });

            if (redCount <= whiteCount || redCount <= yellowCount)
                return (int)TulipColor.Red;

            if (whiteCount <= redCount || whiteCount <= yellowCount)
                return (int)TulipColor.White;

            return (int)TulipColor.Yellow;
        }
    }
}
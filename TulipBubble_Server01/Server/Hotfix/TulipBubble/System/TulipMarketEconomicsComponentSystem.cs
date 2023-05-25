using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using ETModel;
using Object = System.Object;

namespace System
{
}

namespace ETHotfix
{
    [ObjectSystem]
    public class TulipMarketEconomicsComponentAwakeSystem : AwakeSystem<TulipMarketEconomicsComponent>
    {
        public override void Awake(TulipMarketEconomicsComponent self)
        {
            self.Awake();
        }
    }


    public static class TulipMarketEconomicsComponentSystem
    {
        public static void Awake(this TulipMarketEconomicsComponent self)
        {
            RandomInitialPrice(self);
        }

        /// <summary>
        /// 随机起始郁金香价格
        /// </summary>
        /// <param name="self"></param>
        public static void RandomInitialPrice(this TulipMarketEconomicsComponent self)
        {
            List<int> InitialPriceRange = new List<int>() { 2, 3, 4 };
            List<int> InitialTulipPrice = new List<int>();
            Random random = new Random();
            while (InitialPriceRange.Count > 0)
            {
                int index = random.Next(InitialPriceRange.Count);
                InitialTulipPrice.Add(InitialPriceRange[index]);
                InitialPriceRange.RemoveAt(index);
            }

            self.redPrieceLevel = InitialTulipPrice[0];
            self.whitePrieceLevel = InitialTulipPrice[1];
            self.yellowPrieceLevel = InitialTulipPrice[2];
        }

        public static int GetTulipPrice(this TulipMarketEconomicsComponent self, TulipCard card)
        {
            //white yellow red

            int level = GetTulipPriceLevel(self, card.TulipCardColor);
            switch (card.TulipCardLevel)
            {
                case 0:
                    if (level <= 3)
                        return 15;
                    if (level <= 4)
                        return 20;
                    if (level <= 5)
                        return 26;
                    if (level <= 6)
                        return 33;
                    if (level <= 7)
                        return 40;
                    break;
                case 1:
                    if (level <= 3)
                        return 1 + 2 * level;
                    if (level <= 6)
                        return 7 + (level - 3) * 3;
                    if (level <= 7)
                        return 20;
                    break;
                case 2:
                    if (level <= 7)
                        return 1 + (level - 1) * 2;
                    break;
            }

            Log.Info("Error No Price Match");
            return 0;
        }

        public static int GetTulipPriceLevel(this TulipMarketEconomicsComponent self, int color)
        {
            switch (color)
            {
                case 0:
                    return self.whitePrieceLevel;
                case 1:
                    return self.yellowPrieceLevel;
                case 2:
                    return self.redPrieceLevel;
            }

            Log.Info("Error No Price Level Match");
            return 0;
        }

        public static int RiseTulipPriceLevel(this TulipMarketEconomicsComponent self, int color)
        {
            int riseTulipLevel;
            if ((TulipColor)color == TulipColor.Red)
            {
                riseTulipLevel = self.redPrieceLevel;
                do
                {
                    riseTulipLevel += 1;
                    if (riseTulipLevel > 7)
                    {
                        return self.redPrieceLevel;
                    }
                } while (!(riseTulipLevel != self.whitePrieceLevel && riseTulipLevel != self.yellowPrieceLevel));

                self.redPrieceLevel = riseTulipLevel;
                return self.redPrieceLevel;
            }
            else if ((TulipColor)color == TulipColor.White)
            {
                riseTulipLevel = self.whitePrieceLevel;
                do
                {
                    riseTulipLevel += 1;
                    if (riseTulipLevel > 7)
                    {
                        return self.whitePrieceLevel;
                    }
                } while (!(riseTulipLevel != self.redPrieceLevel && riseTulipLevel != self.yellowPrieceLevel));

                self.whitePrieceLevel = riseTulipLevel;
                return self.whitePrieceLevel;
            }
            else if ((TulipColor)color == TulipColor.Yellow)
            {
                riseTulipLevel = self.yellowPrieceLevel;
                do
                {
                    riseTulipLevel += 1;
                    if (riseTulipLevel > 7)
                    {
                        return self.yellowPrieceLevel;
                    }
                } while (!(riseTulipLevel != self.whitePrieceLevel && riseTulipLevel != self.redPrieceLevel));

                self.yellowPrieceLevel = riseTulipLevel;
                return self.yellowPrieceLevel;
            }

            return self.GetTulipPriceLevel(color);
        }

        public static int DropTulipPriceLevel(this TulipMarketEconomicsComponent self, int color)
        {
            int dropTulipLevel;
            Log.Info(((TulipColor)color).ToString());
            if ((TulipColor)color == TulipColor.Red)
            {
                dropTulipLevel = self.redPrieceLevel;
                do
                {
                    dropTulipLevel -= 1;
                    if (dropTulipLevel < 1)
                    {
                        return self.redPrieceLevel;
                    }
                } while (!(dropTulipLevel != self.whitePrieceLevel && dropTulipLevel != self.yellowPrieceLevel));

                self.redPrieceLevel = dropTulipLevel;
                return self.redPrieceLevel;
            }
            else if ((TulipColor)color == TulipColor.White)
            {
                dropTulipLevel = self.whitePrieceLevel;
                do
                {
                    dropTulipLevel -= 1;
                    if (dropTulipLevel < 1)
                    {
                        return self.whitePrieceLevel;
                    }
                } while (!(dropTulipLevel != self.redPrieceLevel && dropTulipLevel != self.yellowPrieceLevel));

                self.whitePrieceLevel = dropTulipLevel;
                return self.whitePrieceLevel;
            }
            else if ((TulipColor)color == TulipColor.Yellow)
            {
                dropTulipLevel = self.yellowPrieceLevel;
                do
                {
                    dropTulipLevel -= 1;
                    if (dropTulipLevel < 1)
                    {
                        return self.yellowPrieceLevel;
                    }
                } while (!(dropTulipLevel != self.whitePrieceLevel && dropTulipLevel != self.redPrieceLevel));

                self.yellowPrieceLevel = dropTulipLevel;
                return self.yellowPrieceLevel;
            }

            return self.GetTulipPriceLevel(color);
        }

        public static int RiseSuddenlyPriceLevel(this TulipMarketEconomicsComponent self)
        {
            int color = self.GetPriceLevelLowestTulipColor();
            for (int i = 0; i < 2; i++)
            {
                self.RiseTulipPriceLevel(color);
            }

            return self.GetTulipPriceLevel(color);
        }

        public static int DropSuddenlyPriceLevel(this TulipMarketEconomicsComponent self)
        {
            int color = self.GetPriceLevelHighestTulipColor();
            for (int i = 0; i < 2; i++)
            {
                self.DropTulipPriceLevel(color);
            }

            return self.GetTulipPriceLevel(color);
        }

        private static int GetPriceLevelLowestTulipColor(this TulipMarketEconomicsComponent self)
        {
            List<int> priceLevel = new List<int>()
                { self.redPrieceLevel, self.yellowPrieceLevel, self.whitePrieceLevel };
            int min = priceLevel.Min();
            if (min == self.redPrieceLevel)
                return (int)TulipColor.Red;
            if (min == self.whitePrieceLevel)
                return (int)TulipColor.White;
            if (min == self.yellowPrieceLevel)
                return (int)TulipColor.Yellow;
            return 0;
        }

        private static int GetPriceLevelHighestTulipColor(this TulipMarketEconomicsComponent self)
        {
            List<int> priceLevel = new List<int>()
                { self.redPrieceLevel, self.yellowPrieceLevel, self.whitePrieceLevel };
            int max = priceLevel.Max();
            if (max == self.redPrieceLevel)
                return (int)TulipColor.Red;
            if (max == self.whitePrieceLevel)
                return (int)TulipColor.White;
            if (max == self.yellowPrieceLevel)
                return (int)TulipColor.Yellow;
            return 0;
        }
    }
}
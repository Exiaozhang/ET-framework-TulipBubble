using System.Collections.Generic;
using System;
using System.Net.NetworkInformation;
using ETModel;

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
    }
}
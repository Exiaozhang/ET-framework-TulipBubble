using ETModel;

namespace ETHotfix
{
    public static class HandCardsComponentSystem
    {
        /// <summary>
        /// 获取所有手牌
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static TulipCard[] GetAll(this HandCardsComponent self)
        {
            return self.tulipLibrary.ToArray();
        }

        /// <summary>
        /// 向手牌中添加牌
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void AddCard(this HandCardsComponent self, TulipCard tulipCard)
        {
            self.tulipLibrary.Add(tulipCard);
        }


        /// <summary>
        /// 出售后将手牌移除
        /// </summary>
        /// <param name="self"></param>
        /// <param name="tulipCard"></param>
        public static void PopCard(this HandCardsComponent self, TulipCard tulipCard)
        {
            self.tulipLibrary.Remove(tulipCard);
        }
        
    }
}
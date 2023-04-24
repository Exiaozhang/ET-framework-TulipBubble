using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 参考Unit类
    /// </summary>
    public partial class TulipCard
    {
        public static TulipCard Create(int color, int level, int weight)
        {
            TulipCard tulipCard = new TulipCard();
            tulipCard.TulipCardColor = color;
            tulipCard.TulipCardLevel = level;
            tulipCard.TulipCardWeight = weight;

            return tulipCard;
        }

        public bool Equals(TulipCard other)
        {
            return this.TulipCardLevel == other.TulipCardLevel && this.TulipCardColor == other.TulipCardColor &&
                   this.TulipCardWeight == other.TulipCardWeight;
        }

        /// <summary>
        /// 获取郁金香卡牌名
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.TulipCardColor == (int)Color.Black
                ? $"{((Color)this.TulipCardColor).ToString()}"
                : $"{((Color)this.TulipCardColor).ToString()}{((Level)this.TulipCardLevel).ToString()}{((Weight)this.TulipCardWeight).ToString()}";
        }
        
        
   
    }
}
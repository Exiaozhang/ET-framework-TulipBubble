using UnityEngine;

namespace ETModel
{
    
    public static class CardHelper
    {
        //卡牌图集预设名称
        public const string TULIPATLAS = "TulipAtlas";

        public const string TULIPCARD = "TulipCard";
        /// <summary>
        /// 获取卡牌精灵
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cardName"></param>
        /// <returns></returns>
        public static Sprite GetCardSprite(string cardName)
        {
            GameObject atlas = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset($"{TULIPATLAS}.unity3d", TULIPATLAS);
            return atlas.Get<Sprite>(cardName);
        }
    }
}
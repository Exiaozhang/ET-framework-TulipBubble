using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public static class CardHelper
    {
        //卡牌图集预设名称

        public const string TULIPATLAS = "TulipAtlas";
        public const string TULIPCARD = "TulipCard";

        public const string COLLECTORATLAS = "CollectorAtlas";
        public const string COLLECTORCARD = "CollectorCard";

        public const string EVENTATLAS = "EventAtlas";
        public const string EVENTCARD = "EventCard";

        public const string CARDDETIALWINDOW = "CardDetialWindow";

        /// <summary>
        /// 获取卡牌Sprite
        /// </summary>
        /// <param name="cardName"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static Sprite GetCardSprite(string cardName, CardType cardType)
        {
            GameObject atlas;
            switch (cardType)
            {
                case CardType.TulipCard:
                    atlas = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>()
                        .GetAsset($"{TULIPATLAS}.unity3d", TULIPATLAS);
                    return atlas.Get<Sprite>(cardName);
                case CardType.CollectorCard:
                    atlas = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>()
                        .GetAsset($"{COLLECTORATLAS}.unity3d", COLLECTORATLAS);
                    return atlas.Get<Sprite>(cardName);
                case CardType.EventCard:
                    atlas = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>()
                        .GetAsset($"{EVENTATLAS}.unity3d", EVENTATLAS);
                    if (cardName.Contains("DropSuddenly"))
                        return atlas.Get<Sprite>("DropSuddenlyWhite");
                    return atlas.Get<Sprite>(cardName);
            }

            Log.Error("Could Not Find Suit CardType");
            return null;
        }

        /// <summary>
        /// 创建卡牌
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="cardName"></param>
        /// <param name="parent"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static GameObject CreateCardObj(string prefabName, string cardName, Transform parent,
            CardType cardType)
        {
            GameObject cardSprite = null;
            GameObject cardSpritePrefab;
            switch (cardType)
            {
                case CardType.TulipCard:
                    cardSpritePrefab = (GameObject)Game.Scene.GetComponent<ResourcesComponent>()
                        .GetAsset($"{prefabName}.unity3d", prefabName);
                    cardSprite = UnityEngine.Object.Instantiate(cardSpritePrefab);
                    break;
                case CardType.CollectorCard:
                    cardSpritePrefab = (GameObject)Game.Scene.GetComponent<ResourcesComponent>()
                        .GetAsset($"{prefabName}.unity3d", prefabName);
                    cardSprite = UnityEngine.Object.Instantiate(cardSpritePrefab);
                    break;
                case CardType.EventCard:
                    cardSpritePrefab = (GameObject)Game.Scene.GetComponent<ResourcesComponent>()
                        .GetAsset($"{prefabName}.unity3d", prefabName);
                    cardSprite = UnityEngine.Object.Instantiate(cardSpritePrefab);
                    break;
            }

            if (cardSprite != null)
            {
                cardSprite.name = cardName;
                cardSprite.layer = LayerMask.NameToLayer("UI");
                cardSprite.transform.SetParent(parent.transform, false);

                cardSprite.GetComponent<Image>().sprite = CardHelper.GetCardSprite(cardName, cardType);
            }

            return cardSprite;
        }

   
    }
}
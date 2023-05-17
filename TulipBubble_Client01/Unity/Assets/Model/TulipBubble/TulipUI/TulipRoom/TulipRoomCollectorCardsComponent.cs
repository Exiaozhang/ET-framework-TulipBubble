using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class TulipRoomCollectorCardsComponentAwakeSystem : AwakeSystem<TulipRoomCollectorCardsComponent>
    {
        public override void Awake(TulipRoomCollectorCardsComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 此组件控制用户对房间中收藏家卡牌的操作
    /// </summary>
    public class TulipRoomCollectorCardsComponent : Component
    {
        //高/中/低价位的当前收藏家牌和其对应的数量
        private CollectorCard highPriceCollector = new CollectorCard();
        private int highPriceCollectorCount;
        private CollectorCard middlePriceCollector = new CollectorCard();
        private int middlePriceCollectorCount;
        private CollectorCard lowPriceCollector = new CollectorCard();
        private int lowPriceCollectorCount;

        //UI对应的GameObject
        private GameObject highPriceObj;
        private GameObject highPriceCard;
        private GameObject middlePriceObj;
        private GameObject middlePriceCard;
        private GameObject lowPriceObj;
        private GameObject lowPriceCard;

        public void Awake()
        {
            GameObject panel = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>()
                .Get<GameObject>("Panel");
            ReferenceCollector referenceCollector = panel.GetComponent<ReferenceCollector>();

            highPriceObj = referenceCollector.Get<GameObject>("Collector_20");
            middlePriceObj = referenceCollector.Get<GameObject>("Collector_15");
            lowPriceObj = referenceCollector.Get<GameObject>("Collector_10");

            //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{CardHelper.COLLECTORATLAS}.unity3d");
            resourcesComponent.LoadBundle($"{CardHelper.COLLECTORCARD}.unity3d");
        }

        /// <summary>
        /// 设置高价位收藏家
        /// </summary>
        /// <param name="collectorCard"></param>
        /// <param name="count"></param>
        public void SetCurrentHighPriceCollector(CollectorCard collectorCard, int count)
        {
            collectorCard.BelongType = CardBelongType.Market;
            highPriceCollector = collectorCard;
            highPriceCollectorCount = count;

            if (highPriceCard == null)
            {
                GameObject card = CardHelper.CreateCardObj(CardHelper.COLLECTORCARD, collectorCard.GetName(),
                    highPriceObj.transform,
                    CardType.CollectorCard);
                CollectorCardSprite collectorCardSprite = card.AddComponent<CollectorCardSprite>();
                collectorCardSprite.card = collectorCard;
                collectorCardSprite.deckCount = count;
                return;
            }

            highPriceCard.GetComponent<Image>().sprite =
                CardHelper.GetCardSprite(collectorCard.GetName(), CardType.CollectorCard);
            CollectorCardSprite cardSprite = highPriceCard.GetComponent<CollectorCardSprite>();
            cardSprite.card = collectorCard;
            cardSprite.deckCount = count;
        }

        /// <summary>
        /// 设置中价位收藏家
        /// </summary>
        /// <param name="collectorCard"></param>
        /// <param name="count"></param>
        public void SetCurrentMiddlePriceCollector(CollectorCard collectorCard, int count)
        {
            collectorCard.BelongType = CardBelongType.Market;
            middlePriceCollector = collectorCard;
            middlePriceCollectorCount = count;

            if (middlePriceCard == null)
            {
                GameObject card = CardHelper.CreateCardObj(CardHelper.COLLECTORCARD, collectorCard.GetName(),
                    middlePriceObj.transform,
                    CardType.CollectorCard);
                CollectorCardSprite collectorCardSprite = card.AddComponent<CollectorCardSprite>();
                collectorCardSprite.card = collectorCard;
                collectorCardSprite.deckCount = count;
                return;
            }

            highPriceCard.GetComponent<Image>().sprite =
                CardHelper.GetCardSprite(collectorCard.GetName(), CardType.CollectorCard);
            CollectorCardSprite cardSprite = highPriceCard.GetComponent<CollectorCardSprite>();
            cardSprite.card = collectorCard;
            cardSprite.deckCount = count;
        }

        /// <summary>
        /// 设置低价位收藏家
        /// </summary>
        /// <param name="collectorCard"></param>
        /// <param name="count"></param>
        public void SetCurrentLowPriceCollector(CollectorCard collectorCard, int count)
        {
            collectorCard.BelongType = CardBelongType.Market;
            lowPriceCollector = collectorCard;
            lowPriceCollectorCount = count;

            if (highPriceCard == null)
            {
                GameObject card = CardHelper.CreateCardObj(CardHelper.COLLECTORCARD, collectorCard.GetName(),
                    lowPriceObj.transform,
                    CardType.CollectorCard);
                CollectorCardSprite collectorCardSprite = card.AddComponent<CollectorCardSprite>();
                collectorCardSprite.card = collectorCard;
                collectorCardSprite.deckCount = count;
                return;
            }

            highPriceCard.GetComponent<Image>().sprite =
                CardHelper.GetCardSprite(collectorCard.GetName(), CardType.CollectorCard);
            CollectorCardSprite cardSprite = highPriceCard.GetComponent<CollectorCardSprite>();
            cardSprite.card = collectorCard;
            cardSprite.deckCount = count;
        }
    }
}
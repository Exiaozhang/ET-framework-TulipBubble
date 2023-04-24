using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class RoomCardsComponentAwakeSystem : AwakeSystem<RoomCardsComponent>
    {
        public override void Awake(RoomCardsComponent self)
        {
            self.Awake();
        }
    }

    public class RoomCardsComponent : Component
    {
        public const string TulipCard_Name = "TulipCard";

        private readonly Dictionary<string, GameObject> TulipCardsSprite = new Dictionary<string, GameObject>();
        private readonly List<TulipCard> futureTulipCards = new List<TulipCard>();
        private readonly List<TulipCard> cashTulipCards = new List<TulipCard>();
        private readonly List<TulipCard> selledTulipCards = new List<TulipCard>();

        private GameObject _future;
        private GameObject _cash;
        private GameObject _selled;

        public GameObject Panel { get; set; }

        public void Awake()
        {
            this.Panel = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("Panel");
            _future = this.Panel.Get<GameObject>("Future");
            _cash = this.Panel.Get<GameObject>("Cash");
            _selled = this.Panel.Get<GameObject>("Sell");

            //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{TulipCard_Name}.unity3d");
            resourcesComponent.LoadBundle($"{CardHelper.TULIPATLAS}.unity3d");
            resourcesComponent.LoadBundle($"{CardHelper.TULIPCARD}.unity3d");
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            ClearFutureCards();
            ClearCashCards();
            ClearSelledCards();
        }

        /// <summary>
        /// 获取卡牌Sprite
        /// </summary>
        /// <param name="tulipCard"></param>
        /// <returns></returns>
        public GameObject GetSpire(TulipCard tulipCard)
        {
            if (TulipCardsSprite.TryGetValue(tulipCard.GetName(), out var tulipCardSprite))
            {
                return tulipCardSprite;
            }

            Log.Debug("Error,Cant Find This Card");
            return tulipCardSprite;
        }

        /// <summary>
        /// 添加牌
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="type"></param>
        public void AddTulipCards(TulipCard[] cards, TulipMarkType type)
        {
            switch (type)
            {
                case TulipMarkType.future:
                    for (int i = 0; i < cards.Length; i++)
                    {
                        AddCard(cards[i], type);
                    }

                    break;
                case TulipMarkType.cash:
                    for (int i = 0; i < cards.Length; i++)
                    {
                        AddCard(cards[i], type);
                    }

                    break;
                case TulipMarkType.selled:
                    for (int i = 0; i < cards.Length; i++)
                    {
                        AddCard(cards[i], type);
                    }

                    break;
            }
        }

        /// <summary>
        /// 购买郁金香后更新市场区域
        /// </summary>
        /// <param name="card"></param>
        public void PopTulipCard(TulipCard card)
        {
        }


        private void AddCard(TulipCard card, TulipMarkType type)
        {
            GameObject tulipCard;
            switch (type)
            {
                case TulipMarkType.future:
                    tulipCard = CreateCardSprite(CardHelper.TULIPCARD, card.GetName(), _future.transform);
                    MarketCardSprite marketCardSprite = tulipCard.AddComponent<MarketCardSprite>();
                    marketCardSprite.card=card;
                    marketCardSprite.isFurtueCard = true;
                    break;
                case TulipMarkType.cash:
                    tulipCard = CreateCardSprite(CardHelper.TULIPCARD, card.GetName(), _cash.transform);
                    tulipCard.AddComponent<MarketCardSprite>().card = card;
                    break;
                case TulipMarkType.selled:
                    tulipCard = CreateCardSprite(CardHelper.TULIPCARD, card.GetName(), _selled.transform);
                    tulipCard.AddComponent<MarketCardSprite>().card = card;
                    break;
            }
        }

        private void ClearSelledCards()
        {
        }

        private void ClearCashCards()
        {
        }

        private void ClearFutureCards()
        {
        }

        /// <summary>
        /// 创建卡牌精灵
        /// </summary>
        private GameObject CreateCardSprite(string prefabName, string cardName, Transform parent)
        {
            GameObject cardSpritePrefab = (GameObject)Game.Scene.GetComponent<ResourcesComponent>()
                .GetAsset($"{prefabName}.unity3d", prefabName);
            GameObject cardSprite = UnityEngine.Object.Instantiate(cardSpritePrefab);

            cardSprite.name = cardName;
            cardSprite.layer = LayerMask.NameToLayer("UI");
            cardSprite.transform.SetParent(parent.transform, false);

            Sprite sprite = CardHelper.GetCardSprite(cardName);
            cardSprite.GetComponent<Image>().sprite = sprite;

            return cardSprite;
        }
    }
}
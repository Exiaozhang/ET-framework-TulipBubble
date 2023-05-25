using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class RoomCardsComponentAwakeSystem : AwakeSystem<TulipRoomTulipCardsComponent>
    {
        public override void Awake(TulipRoomTulipCardsComponent self)
        {
            self.Awake();
        }
    }

    public class TulipRoomTulipCardsComponent : Component
    {
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
            resourcesComponent.LoadBundle($"{CardHelper.TULIPATLAS}.unity3d");
            resourcesComponent.LoadBundle($"{CardHelper.TULIPCARD}.unity3d");
            resourcesComponent.LoadBundle($"SignObject.unity3d");
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

        public CardBelongType GetBelongType(TulipCard card)
        {
            if (futureTulipCards.Exists((tulipCard) => System.Object.ReferenceEquals(tulipCard, card)))
                return CardBelongType.Market;

            return CardBelongType.Error;
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


        public void ClearTulipCards(TulipMarkType type)
        {
            switch (type)
            {
                case TulipMarkType.future:
                    futureTulipCards.ForEach(_ => { UnityEngine.Object.Destroy(_.CardObj); });
                    futureTulipCards.Clear();
                    break;
                case TulipMarkType.cash:
                    cashTulipCards.ForEach(_ => { UnityEngine.Object.Destroy(_.CardObj); });
                    cashTulipCards.Clear();
                    break;
                case TulipMarkType.selled:
                    selledTulipCards.ForEach(_ => { UnityEngine.Object.Destroy(_.CardObj); });
                    selledTulipCards.Clear();
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

        public void ClearSignObj()
        {
            cashTulipCards.ForEach(tulip =>
            {
                if (tulip.CardObj.transform.Find("SignObject").transform.childCount == 0)
                    return;
                foreach (Transform transform in tulip.CardObj.transform.Find("SignObject").transform)
                {
                    UnityEngine.Object.Destroy(transform.gameObject);
                }
            });
            selledTulipCards.ForEach(tulip =>
            {
                if (tulip.CardObj.transform.Find("SignObject").transform.childCount == 0)
                    return;
                foreach (Transform transform in tulip.CardObj.transform.Find("SignObject").transform)
                {
                    UnityEngine.Object.Destroy(transform.gameObject);
                }
            });
        }

        public void UpdateSignObj(List<GamerReserveTulip> gamerReserveTulips)
        {
            ClearSignObj();

            gamerReserveTulips.ForEach(reserveTulips =>
            {
                TulipCard findCardWithId = FindCardWithId(reserveTulips.ReserveTulipCard.Id);

                if (findCardWithId == null)
                    return;

                foreach (long id in reserveTulips.UserId)
                {
                    PutSignObjToTulipCard(id, findCardWithId);
                }
            });
        }

        public void PutSignObjToTulipCard(Int64 userId, TulipCard tulipCard)
        {
            TulipRoomGameComponent tulipRoomGameComponent = this.GetParent<UI>().GetComponent<TulipRoomGameComponent>();
            string userColor = tulipRoomGameComponent.GetUserColor(userId);

            Sprite sprite = Resources.Load<Sprite>($"Player_{userColor}");
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            GameObject preSignObj = (GameObject)resourcesComponent.GetAsset($"SignObject.unity3d", "SignObject");
            GameObject signObj =
                UnityEngine.Object.Instantiate(preSignObj, tulipCard.CardObj.transform.Find("SignObject"), false);
            signObj.transform.localScale = new Vector3(1, 1, 1);
            signObj.transform.localPosition = Vector3.zero;
            signObj.GetComponent<Image>().sprite = sprite;
        }

        private TulipCard FindCardWithId(Int64 Id)
        {
            var tulipCard = cashTulipCards.Find(card =>
            {
                if (card.Id == Id)
                    return true;
                return false;
            });
            if (tulipCard != null)
                return tulipCard;

            tulipCard = selledTulipCards.Find(card =>
            {
                if (card.Id == Id)
                    return true;
                return false;
            });
            if (tulipCard != null)
                return tulipCard;

            tulipCard = futureTulipCards.Find(card =>
            {
                if (card.Id == Id)
                    return true;
                return false;
            });
            if (tulipCard != null)
                return tulipCard;

            return null;
        }

        private void AddCard(TulipCard card, TulipMarkType type)
        {
            GameObject tulipCard;
            card.BelongType = CardBelongType.Market;
            switch (type)
            {
                case TulipMarkType.future:
                    tulipCard = CardHelper.CreateCardObj(CardHelper.TULIPCARD, card.GetName(), _future.transform,
                        CardType.TulipCard);
                    MarketCardSprite marketCardSprite = tulipCard.AddComponent<MarketCardSprite>();
                    marketCardSprite.card = card;
                    marketCardSprite.isFurtueCard = true;
                    card.CardObj = tulipCard;
                    futureTulipCards.Add(card);
                    break;
                case TulipMarkType.cash:
                    tulipCard = CardHelper.CreateCardObj(CardHelper.TULIPCARD, card.GetName(), _cash.transform,
                        CardType.TulipCard);
                    tulipCard.AddComponent<MarketCardSprite>().card = card;
                    card.CardObj = tulipCard;
                    cashTulipCards.Add(card);
                    break;
                case TulipMarkType.selled:
                    tulipCard = CardHelper.CreateCardObj(CardHelper.TULIPCARD, card.GetName(), _selled.transform,
                        CardType.TulipCard);
                    tulipCard.AddComponent<MarketCardSprite>().card = card;
                    card.CardObj = tulipCard;
                    selledTulipCards.Add(card);
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
    }
}
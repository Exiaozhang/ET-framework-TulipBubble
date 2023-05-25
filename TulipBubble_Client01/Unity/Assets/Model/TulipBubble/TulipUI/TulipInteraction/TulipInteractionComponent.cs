using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class LandInteractionComponentAwakeSystem : AwakeSystem<TulipInteractionComponent>
    {
        public override void Awake(TulipInteractionComponent self)
        {
            self.Awake();
        }
    }

    public class TulipInteractionComponent : Component
    {
        //当前选择的牌和物体
        private System.Object selectedCard;
        private GameObject selectedCardObj;

        //出售/预定/跳过 按钮
        private GameObject sell;
        private GameObject reserve;
        private GameObject pass;

        //卡牌细节窗口
        private GameObject currentCardDetailWindow;

        public void Awake()
        {
            //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{CardHelper.CARDDETIALWINDOW}.unity3d");

            ReferenceCollector referenceCollector = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            sell = referenceCollector.Get<GameObject>("Sell");
            reserve = referenceCollector.Get<GameObject>("Reserve");
            pass = referenceCollector.Get<GameObject>("Pass");
            InitReserveButton();
        }

        private void InitSellButton()
        {
            if (sell == null)
                return;
            Button button = sell.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_SellTulipCard_Ntt
                {
                });
            });
        }

        private void InitReserveButton()
        {
            if (reserve == null)
                return;
            Button button = reserve.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                TulipCard card = this.selectedCard as TulipCard;
                if (card == null || card.BelongType != CardBelongType.Market)
                    return;

                TulipRoomTulipCardsComponent tulipRoomTulipCardsComponent =
                    Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom)
                        .GetComponent<TulipRoomTulipCardsComponent>();

                tulipRoomTulipCardsComponent.PutSignObjToTulipCard(TulipRoomComponent.LocalGamer.UserID, card);

                reserve.SetActive(false);

                SessionComponent.Instance.Session.Send(new Actor_ReserveTulipCard_Ntt()
                {
                    ReserveTulipCard = card
                });
            });
        }

        private void InitPassButton()
        {
        }

        /// <summary>
        /// 选择或者取消卡牌
        /// </summary>
        /// <param name="card"></param>
        /// <param name="cardObj"></param>
        public void SetSelectedCard(System.Object card, GameObject cardObj)
        {
            if (SelectCardAnimation(cardObj))
            {
                SelectedCard(card, cardObj);
                return;
            }

            CancelSelectedCard();
        }

        private void SelectedCard(System.Object card, GameObject cardObj)
        {
            selectedCard = card;
            selectedCardObj = cardObj;

            TulipRoomGameComponent tulipRoomGameComponent = Game.Scene.GetComponent<UIComponent>()
                .Get(TulipUIType.TulipRoom).GetComponent<TulipRoomGameComponent>();
            bool isLocalTurn = tulipRoomGameComponent.WhetherIsLocalGamerTurn();
            GameStage stage = tulipRoomGameComponent.stage;

            if (!isLocalTurn)
                return;

            if (card is TulipCard tulipCard)
            {
                if (tulipCard.BelongType == CardBelongType.Market && stage == GameStage.AuctionStage)
                {
                    sell.SetActive(false);
                    reserve.SetActive(false);

                    reserve.SetActive(true);
                }
                else if (tulipCard.BelongType == CardBelongType.Player && stage == GameStage.SellStage)
                {
                    sell.SetActive(false);
                    reserve.SetActive(false);

                    sell.SetActive(true);
                }
            }
            else if (card is CollectorCard collectorCard)
            {
                if (collectorCard.BelongType == CardBelongType.Market)
                {
                    sell.SetActive(false);
                    reserve.SetActive(false);

                    sell.SetActive(true);
                }
            }
        }

        private void CancelSelectedCard()
        {
            selectedCard = null;
            selectedCardObj = null;

            sell.SetActive(false);
            reserve.SetActive(false);
        }

        /// <summary>
        /// 选择卡牌时的动画效果
        /// </summary>
        /// <returns>如果选择返回true,取消返回false</returns>
        private bool SelectCardAnimation(GameObject card)
        {
            if (selectedCardObj != null)
            {
                selectedCardObj.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (selectedCardObj == card)
            {
                selectedCardObj = null;
                return false;
            }

            selectedCardObj = card;
            selectedCardObj.transform.GetChild(0).gameObject.SetActive(true);
            return true;
        }

        /// <summary>
        /// 设置悬浮在卡牌上的效果 
        /// </summary>
        /// <param name="card"></param>
        /// <param name="message"></param>
        public void SetHoverCard(System.Object card, String message, Transform transform = null)
        {
            if (card is CollectorCard)
            {
                ShowMessage(message, transform);
            }
            else if (card is EventCard)
            {
                ShowMessage(message, transform);
                Log.Info("Card is EventCard");
            }
        }

        public void CancelHoverCard(System.Object card)
        {
            if (card is CollectorCard)
            {
                CancelMessage();
            }
            else if (card is EventCard)
            {
                CancelMessage();
            }
        }

        private void ShowMessage(String message, Transform transform = null)
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            GameObject preDetailWindow =
                (GameObject)resourcesComponent.GetAsset($"{CardHelper.CARDDETIALWINDOW}.unity3d",
                    $"{CardHelper.CARDDETIALWINDOW}");
            GameObject detailWindow = UnityEngine.Object.Instantiate(preDetailWindow);
            currentCardDetailWindow = detailWindow;
            detailWindow.transform.SetParent(Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom)
                .GameObject.transform, true);
            detailWindow.transform.localScale = new Vector3(1, 1, 1);

            if (transform != null)
                detailWindow.transform.position = transform.position;
            detailWindow.GetComponentInChildren<Text>().text = message;
        }

        private void CancelMessage()
        {
            if (currentCardDetailWindow != null)
                UnityEngine.Object.Destroy(currentCardDetailWindow);
        }

        /// <summary>
        /// 设置跳过按钮
        /// </summary>
        public void SetPassButton(bool active)
        {
            pass.SetActive(active);
        }
    }
}
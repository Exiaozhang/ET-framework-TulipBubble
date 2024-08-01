using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{

    [ObjectSystem]
    public class TulipRoomGamerPanelComponentAwakeSystem : AwakeSystem<TulipRoomGamerPanelComponent>
    {
        public override void Awake(TulipRoomGamerPanelComponent self)
        {

        }
    }

    /// <summary>
    /// 玩家UI组件
    /// </summary>
    public class TulipRoomGamerPanelComponent : Component
    {
        //UI面板
        public GameObject Panel;

        //玩家昵称
        public string NickName => name.text;

        private Image headPhoto;
        private Text prompt;
        private Text name;
        private Text money;
        private Text signObj;
        private GameObject bidPanel;
        private GameObject payWayPanel;
        private GameObject handCardsPanel;

        //BidPanel的组件
        private GameObject passButton;
        private GameObject bidButton;
        private Text highestPrice;
        private Text playerPrice;
        private GameObject increasePriceButton;
        private GameObject decreasePriceButton;
        private GameObject bidTulipCardObj;

        //PayWayPanel的组件
        private GameObject cashPriceButton;
        private GameObject loanPriceButton;

        /// <summary>
        /// 设置玩家UI面板
        /// </summary>
        /// <param name="panel"></param>
        public void SetPanel(GameObject panel)
        {
            this.Panel = panel;

            //绑定关联
            this.prompt = this.Panel.Get<GameObject>("Prompt").GetComponent<Text>();
            this.name = this.Panel.Get<GameObject>("Name").GetComponent<Text>();
            this.money = this.Panel.Get<GameObject>("Money").GetComponent<Text>();
            this.signObj = this.Panel.Get<GameObject>("SignObj").GetComponent<Text>();
            this.bidPanel = this.Panel.Get<GameObject>("BidPanel");
            this.payWayPanel = this.Panel.Get<GameObject>("BuyWayPanel");
            this.handCardsPanel = this.Panel.Get<GameObject>("HandCard");

            this.passButton = this.bidPanel.Get<GameObject>("Pass");
            this.bidButton = this.bidPanel.Get<GameObject>("Bid");
            this.highestPrice = this.bidPanel.Get<GameObject>("HighestPrice").GetComponent<Text>();
            this.playerPrice = this.bidPanel.Get<GameObject>("PlayerPrice").GetComponent<Text>();
            this.increasePriceButton = this.bidPanel.Get<GameObject>("IncreasePrice");
            this.decreasePriceButton = this.bidPanel.Get<GameObject>("DecreasePrice");
            this.bidTulipCardObj = this.bidPanel.Get<GameObject>("TulipCard");

            increasePriceButton.GetComponent<Button>().onClick.Add(() =>
            {
                playerPrice.text = (Convert.ToInt32(playerPrice.text) + 1).ToString();
            });
            decreasePriceButton.GetComponent<Button>().onClick.Add(() =>
            {
                int gamerPrice = Convert.ToInt32(this.playerPrice.text);
                int gamerHighestPrice = Convert.ToInt32(this.highestPrice.text);
                if ((gamerPrice - 1) <= gamerHighestPrice)
                {
                    return;
                }
                this.playerPrice.text = (gamerPrice - 1).ToString();
            });
            passButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_NotifyRoomBid()
                {
                    Price = 0
                });
                bidPanel.SetActive(false);
            });
            bidButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_NotifyRoomBid()
                {
                    Price = Convert.ToInt32(playerPrice.text)
                });
                bidPanel.SetActive(false);
            });

            this.cashPriceButton = this.payWayPanel.Get<GameObject>("Cash");
            this.loanPriceButton = this.payWayPanel.Get<GameObject>("Loans");

            cashPriceButton.GetComponent<Button>().onClick.Add(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_NotifyRoomPayWay_Ntt()
                {
                    PayWay = (Int32)PayWay.Cash
                });
                payWayPanel.SetActive(false);
            });
            loanPriceButton.GetComponent<Button>().onClick.Add(() =>
            {
                SessionComponent.Instance.Session.Send(new Actor_NotifyRoomPayWay_Ntt()
                {
                    PayWay = (Int32)PayWay.Loans
                });
                payWayPanel.SetActive(false);
            });

            UpdatePanel();
        }

        /// <summary>
        /// 更新面板
        /// </summary>
        private void UpdatePanel()
        {
            if (this.Panel != null)
            {
                SetUserInfoRoom();
                //TODO:更改玩家头像
            }
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        private async void SetUserInfoRoom()
        {
            G2C_GetUserInfoInRoom_Back g2C_GetUserInfo_Ack =
                (G2C_GetUserInfoInRoom_Back)await SessionComponent.Instance.Session.Call(new C2G_GetUserInfoInRoom_Req()
                {
                    UserID = this.GetParent<Gamer>().UserID
                });

            if (this.Panel != null)
            {
                name.text = g2C_GetUserInfo_Ack.NickName;
            }
        }

        /// <summary>
        /// 设置金钱
        /// </summary>
        /// <param name="moeny"></param>
        public void SetMoney(int money)
        {
            this.money.text = money.ToString();
        }

        /// <summary>
        /// 重置面板
        /// </summary>
        public void RestPanel()
        {
            ResetPrompt();

            //this.headPhoto.gameObject.SetActive(false);

            this.name.text = "空位";
            this.money.text = "";

            this.Panel = null;
            this.prompt = null;
            this.name = null;
            this.money = null;
        }

        /// <summary>
        /// 重置提示
        /// </summary>
        public void ResetPrompt()
        {
            prompt.text = "";
        }

        public void SetReady()
        {
            prompt.text = "Ready";
        }

        public void SetMyTurn()
        {
            prompt.text = "This is your turn";

        }

        public void SetOtherTurn()
        {
            prompt.text = "Waiting for other turn";
        }

        public void SetGameStart()
        {
            prompt.text = "Game Start";
        }

        /// <summary>
        /// 设置拍卖回合
        /// </summary>
        /// <param name="price"></param>
        public void SetMyBidTurn(int price, TulipCard tulipCard)
        {
            this.bidPanel.SetActive(true);
            this.playerPrice.text = (price + 1).ToString();
            this.highestPrice.text = price.ToString();
            this.bidTulipCardObj.GetComponent<Image>().sprite = CardHelper.GetCardSprite(tulipCard.GetName(), CardType.TulipCard);
        }

        /// <summary>
        /// 提示玩家购买方式
        /// </summary>
        public void SetBuyWay()
        {
            payWayPanel.SetActive(true);
        }

        public void SetSignObj(int count)
        {
            signObj.text = $"当前标志物剩余:{count}";
        }

        public void SetHandCards()
        {
            Gamer gamer = this.GetParent<Gamer>();
            TulipRoomGamerHandCardComponent tulipRoomGamerHandCardComponent = gamer.GetComponent<TulipRoomGamerHandCardComponent>();
            foreach (Transform i in handCardsPanel.transform)
            {
                UnityEngine.Object.Destroy(i.gameObject);
            }

            foreach (TulipCard i in tulipRoomGamerHandCardComponent.tulipCardsLibrary)
            {
                i.CardObj = CardHelper.CreateCardObj(CardHelper.TULIPCARD, i.GetName(), handCardsPanel.transform, CardType.TulipCard);
                i.BelongType = CardBelongType.Player;
                i.CardObj.AddComponent<HandCardSprite>().card = i;
            }

            foreach (KeyValuePair<TulipCard, Int32> i in tulipRoomGamerHandCardComponent.loanCardsLibrary)
            {
                Log.Info("Hello World");
                i.Key.CardObj = CardHelper.CreateCardObj(CardHelper.TULIPCARD, i.Key.GetName(), handCardsPanel.transform, CardType.TulipCard);
                i.Key.BelongType = CardBelongType.Player;
                i.Key.CardObj.transform.Find("Text").GetComponent<Text>().text = $"欠款{i.Value}";
                i.Key.CardObj.AddComponent<HandCardSprite>().card = i.Key;
                i.Key.isLoanCard = true;
                i.Key.loanMoney = i.Value;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            //重置玩家UI
            RestPanel();
        }
    }
}
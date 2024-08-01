using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class TulipRoomComponentAwakeSystem : AwakeSystem<TulipRoomComponent>
    {
        public override void Awake(TulipRoomComponent self)
        {
            self.Awake();

        }
    }

    /// <summary>
    /// 房间组件界面
    /// </summary>
    public class TulipRoomComponent : Component
    {
        private readonly Dictionary<long, int> seats = new Dictionary<long, int>();
        public bool Matching { get; set; }
        private readonly Gamer[] gamers = new Gamer[5];
        public static Gamer LocalGamer { get; private set; }

        private GameObject LocalGamerPanel;
        private GameObject AllGamer;
        private GameObject StartButton;
        private GameObject ReadyButton;
        private GameObject CancelReadyButton;

        private Text promt;

        private TulipInteractionComponent _interaction;

        public TulipInteractionComponent Interaction
        {
            get
            {
                if (_interaction == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    UI uiInteraction = TulipInteractionFactory.Create(TulipUIType.TulipInteraction, uiRoom);
                    _interaction = uiInteraction.GetComponent<TulipInteractionComponent>();
                }

                return _interaction;
            }
        }

        private TulipMarketEconomicsComponent _marketEconomicsComponent;

        public TulipMarketEconomicsComponent MarketEconomicsComponent
        {
            get
            {
                if (_marketEconomicsComponent == null)
                {
                    UI uiRoom = this.GetParent<UI>();
                    UI uiMarketEconomics = TulipMarketEconomicsFactory.Create(TulipUIType.TulipMarketEconomics, uiRoom);
                    _marketEconomicsComponent = uiMarketEconomics.GetComponent<TulipMarketEconomicsComponent>();
                }

                return _marketEconomicsComponent;
            }
        }

        public void Awake()
        {
            ReferenceCollector referenceCollector = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            GameObject quitButton = referenceCollector.Get<GameObject>("Quit");
            ReadyButton = referenceCollector.Get<GameObject>("Ready");
            CancelReadyButton = referenceCollector.Get<GameObject>("CancelReady");
            StartButton = referenceCollector.Get<GameObject>("StartGame");
            StartButton.SetActive(false);

            promt = referenceCollector.Get<GameObject>("MatchPrompt").GetComponent<Text>();
            //添加玩家面板
            LocalGamerPanel = referenceCollector.Get<GameObject>("LocalGamer");
            AllGamer = referenceCollector.Get<GameObject>("AllGamers");

            //readyButton.SetActive(false); //默认匹配
            Matching = true; //进入房间后取消匹配状态

            //绑定事件
            quitButton.GetComponent<Button>().onClick.Add(OnQuit);
            ReadyButton.GetComponent<Button>().onClick.Add(OnReady);
            CancelReadyButton.GetComponent<Button>().onClick.Add(OnCancelReady);
            StartButton.GetComponent<Button>().onClick.Add(OnStart);

            //添加本地玩家
            Gamer gamer = ComponentFactory.Create<Gamer, long>(GamerComponent.Instance.MyUser.UserID);

            LocalGamer = gamer;
            gamer.AddComponent<TulipRoomGamerPanelComponent>().SetPanel(LocalGamerPanel);
            gamer.AddComponent<TulipRoomGamerMoneyComponent>();
            gamer.AddComponent<TulipRoomGamerReserveSignObjComponent>();
            gamer.AddComponent<TulipRoomGamerHandCardComponent>();
        }

        public void AddGamer(Gamer gamer, int index)
        {
            seats.Add(gamer.UserID, index);
            gamers[index] = gamer;
            gamer.AddComponent<TulipRoomGamerOrderPanelComponent, GameObject, int>(AllGamer, index);
            TulipRoomGameComponent tulipRoomGameComponent = this.GetParent<UI>().GetComponent<TulipRoomGameComponent>();
            tulipRoomGameComponent.AddColorToGamer(gamer.UserID, index);
            if (gamer.UserID == LocalGamer.UserID)
            {
                LocalGamerPanel.GetComponent<ReferenceCollector>()
                    .Get<GameObject>(TulipRoomGamerOrderPanelComponent.userColor[index]).SetActive(true);
                tulipRoomGameComponent.SetLocalPlayerColor(gamer.UserID);
            }

            promt.text = $"One player join room,total {seats.Count} players";
        }

        /// <summary>
        /// ui界面显示开始游戏按钮
        /// </summary>
        public void ShowStartButton()
        {
            StartButton.SetActive(true);
        }
        /// <summary>
        /// ui界面显示开始游戏按钮
        /// </summary>
        public void HideStartButton()
        {
            StartButton.SetActive(false);
        }

        /// <summary>
        /// ui界面显示准备按钮
        /// </summary>
        public void ShowReadyButton()
        {
            ReadyButton.SetActive(true);
        }

        /// <summary>
        /// ui界面隐藏准备按钮
        /// </summary>
        public void HideReadyButton()
        {
            ReadyButton.SetActive(false);
        }

        /// <summary>
        /// ui界面显示取消准备按钮
        /// </summary>
        public void ShowCancelReadyButton()
        {
            CancelReadyButton.SetActive(true);
        }

        /// <summary>
        /// ui界面隐藏取消准备按钮
        /// </summary>
        public void HideCancelButton()
        {
            CancelReadyButton.SetActive(false);
        }

        /// <summary>
        /// 将一个指定Id的玩家移除房间
        /// </summary>
        /// <param name="id"></param>
        public void RemoveGamer(long id)
        {
            int seatIndex = GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                Gamer gamer = gamers[seatIndex];
                gamers[seatIndex] = null;
                seats.Remove(id);
                gamer.Dispose();
                promt.text = $"One player leave room , room have {seats.Count} players";
            }
        }

        /// <summary>
        /// 判断是否为本地玩家
        /// </summary>
        /// <param name="id"></param>
        /// <returns>是/true,不是/false</returns>
        public bool IsLocalGame(long id)
        {
            if (id == LocalGamer.UserID)
                return true;
            return false;
        }

        //得到指定Id的玩家
        public Gamer GetGamer(long id)
        {
            int seatIndex = GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                return gamers[seatIndex];
            }

            return null;
        }

        /// <summary>
        /// 得到当先房间所有位置的玩家
        /// </summary>
        /// <returns>空位的玩家返回null</returns>
        public List<Gamer> GetAllGamers()
        {
            return gamers.ToList<Gamer>();
        }

        private int GetGamerSeat(long id)
        {
            if (seats.TryGetValue(id, out var seatIndex))
            {
                return seatIndex;
            }

            return -1;
        }

        private void OnStart()
        {
            SessionComponent.Instance.Session.Send(new Actor_GameStartMention() { });
        }

        private void OnReady()
        {
            //发送准备游戏的Actor_GamerReady_Landlords消息
            //由客户端与网关的连接session发送，再转到Map服务
            SessionComponent.Instance.Session.Send(new Actor_GamerReady_TulipBubble() { });
        }

        private void OnCancelReady()
        {
            SessionComponent.Instance.Session.Send(new Actor_GamerCancelReady_TulipBubble() { });
        }

        private void OnQuit()
        {
            //发送退出房间消息
            SessionComponent.Instance.Session.Send(new C2G_ReturnLobby_Ntt());
            //切换到大厅界面
            Game.Scene.GetComponent<UIComponent>().Create(TulipUIType.TulipLobby);
            Game.Scene.GetComponent<UIComponent>().Remove(TulipUIType.TulipRoom);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.Matching = false;
            this.seats.Clear();
            for (int i = 0; i < this.gamers.Length; i++)
            {
                if (gamers[i] != null)
                {
                    gamers[i].Dispose();
                    gamers[i] = null;
                }
            }
        }


    }
}
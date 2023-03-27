using System.Collections.Generic;
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
        public readonly Dictionary<long, int> seats = new Dictionary<long, int>();
        public bool Matching { get; set; }
        public readonly Gamer[] gamers = new Gamer[5];
        public static Gamer LocalGamer { get; private set; }

        public GameObject LocalGamerPanel;

        public GameObject AllGamer;


        public Text promt;

        public void Awake()
        {
            ReferenceCollector referenceCollector = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            GameObject quitButton = referenceCollector.Get<GameObject>("Quit");
            GameObject readyButton = referenceCollector.Get<GameObject>("Ready");
            promt = referenceCollector.Get<GameObject>("MatchPrompt").GetComponent<Text>();
            //添加玩家面板
            LocalGamerPanel = referenceCollector.Get<GameObject>("LocalGamer");
            AllGamer = referenceCollector.Get<GameObject>("AllGamers");


            //readyButton.SetActive(false); //默认匹配
            Matching = true; //进入房间后取消匹配状态

            //绑定事件
            quitButton.GetComponent<Button>().onClick.Add(OnQuit);
            readyButton.GetComponent<Button>().onClick.Add(OnReady);


            //添加本地玩家
            Gamer gamer = ComponentFactory.Create<Gamer, long>(GamerComponent.Instance.MyUser.UserID);

            // AddGamer(gamer, 1);
            LocalGamer = gamer;
            gamer.AddComponent<TulipRoomGamerPanelComponent>().SetPanel(LocalGamerPanel);
        }

        public void AddGamer(Gamer gamer, int index)
        {
            seats.Add(gamer.UserID, index);
            gamers[index] = gamer;
            gamer.AddComponent<TulipRoomGamerOrderPanelComponent, GameObject, int>(AllGamer, index);
            if (gamer.UserID == LocalGamer.UserID)
            {
                LocalGamerPanel.GetComponent<ReferenceCollector>()
                    .Get<GameObject>(TulipRoomGamerOrderPanelComponent.userColor[index]).SetActive(true);
            }

            promt.text = $"One player join room,total {seats.Count} players";
        }


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

        public Gamer GetGamer(long id)
        {
            int seatIndex = GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                return gamers[seatIndex];
            }

            return null;
        }

        private int GetGamerSeat(long id)
        {
            int seatIndex;
            if (seats.TryGetValue(id, out seatIndex))
            {
                return seatIndex;
            }

            return -1;
        }


        private void OnReady()
        {
            //发送准备游戏的Actor_GamerReady_Landlords消息
            //由客户端与网关的连接session发送，再转到Map服务
            SessionComponent.Instance.Session.Send(new Actor_GamerReady_TulipBubble());
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
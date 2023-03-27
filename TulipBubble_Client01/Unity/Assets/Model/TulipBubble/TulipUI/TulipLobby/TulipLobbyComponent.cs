using System;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class TulipLobbyComponentAwakeSystem : AwakeSystem<TulipLobbyComponent>
    {
        public override void Awake(TulipLobbyComponent self)
        {
            self.Awake();
        }
    }


    /// <summary>
    /// 大厅界面组件
    /// </summary>
    public class TulipLobbyComponent : Component
    {
        //玩家等级分
        public Text rank;

        //玩家名称
        private Text name;

        public bool isMatching;

        public async void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            rank = rc.Get<GameObject>("Rank").GetComponent<Text>();
            name = rc.Get<GameObject>("Name").GetComponent<Text>();

            //获取玩家数据
            A1001_GetUserInfo_G2C GetUserInfo_Ack =
                (A1001_GetUserInfo_G2C)await SessionComponent.Instance.Session.Call(new A1001_GetUserInfo_C2G());

            //显示用户名和用户等级
            name.text = GetUserInfo_Ack.UserName;
            rank.text = GetUserInfo_Ack.Rank.ToString();

            rc.Get<GameObject>("Match").GetComponent<Button>().onClick.Add(OnStartMatchTulipBubble);
        }

        /// <summary>
        /// 匹配斗地主
        /// </summary>
        private async void OnStartMatchTulipBubble()
        {
            try
            {
                //发送开始匹配消息
                C2G_StartMatch_Req c2GStartMatchReq = new C2G_StartMatch_Req();
                G2C_StartMatch_Back g2G_StartMatch_Ack =
                    (G2C_StartMatch_Back)await SessionComponent.Instance.Session.Call(c2GStartMatchReq);

                if (g2G_StartMatch_Ack.Error == ErrorCode.ERR_UserMoneyLessError)
                {
                    Log.Error("Rank is not enough");
                    return;
                }
                //切换到房间界面
                UI landRoom = Game.Scene.GetComponent<UIComponent>().Create(TulipUIType.TulipRoom);
                Game.Scene.GetComponent<UIComponent>().Remove(TulipUIType.TulipLobby);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
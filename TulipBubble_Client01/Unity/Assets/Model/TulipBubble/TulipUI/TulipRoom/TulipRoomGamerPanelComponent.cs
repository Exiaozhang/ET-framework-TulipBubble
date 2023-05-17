using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
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
        private Text rank;

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
            this.rank = this.Panel.Get<GameObject>("Rank").GetComponent<Text>();

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
                rank.text = g2C_GetUserInfo_Ack.Rank.ToString();
            }
        }

        /// <summary>
        /// 重置面板
        /// </summary>
        public void RestPanel()
        {
            ResetPrompt();

            //this.headPhoto.gameObject.SetActive(false);

            this.name.text = "空位";
            this.rank.text = "";

            this.Panel = null;
            this.prompt = null;
            this.name = null;
            this.rank = null;
        }

        /// <summary>
        /// 重置提示
        /// </summary>
        private void ResetPrompt()
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
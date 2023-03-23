using UnityEngine;
using UnityEngine.UI;

namespace ETModel.TulipLobby
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
        //提示文本
        public Text prompt;
        //玩家名称
        private Text name;

        public bool isMatching;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            prompt = rc.Get<GameObject>("Rank").GetComponent<Text>();
            name = rc.Get<GameObject>("Name").GetComponent<Text>();


            //name.text = GetUserInfo_Ack.NickName;
        }
    }
}
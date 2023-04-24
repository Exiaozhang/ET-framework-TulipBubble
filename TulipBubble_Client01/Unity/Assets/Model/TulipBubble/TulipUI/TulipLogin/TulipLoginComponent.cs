using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class TulipLoginComponentAwakeSystem : AwakeSystem<TulipLoginComponent>
    {
        public override void Awake(TulipLoginComponent self)
        {
            self.Awake();
        }
    }


    public class TulipLoginComponent : Component
    {
        //提示文本
        public Text prompt;

        public InputField account;
        public InputField password;

        //是否正在登陆中
        public bool isLogining;
        public bool isRegistering;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            //初始化数据
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("PassWord").GetComponent<InputField>();
            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            this.isLogining = false;
            this.isRegistering = false;

            //添加事件
            rc.Get<GameObject>("Login").GetComponent<Button>().onClick.Add(() => LoginBtnOnClick());
            rc.Get<GameObject>("Register").GetComponent<Button>().onClick.Add(() => RegisterBtnOnClik());
        }

        private void LoginBtnOnClick()
        {
            if (this.isLogining || this.IsDisposed)
            {
                return;
            }
            this.isLogining = true;
            TulipHelper.Login(account.text,password.text);
        }

        private void RegisterBtnOnClik()
        {
            if (this.isRegistering || this.IsDisposed)
            {
                return;
            }
            this.isRegistering = true;
            TulipHelper.Register(account.text, password.text);
        }
    }
}
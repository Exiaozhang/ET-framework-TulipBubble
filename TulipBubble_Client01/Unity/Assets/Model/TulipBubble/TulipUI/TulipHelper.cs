namespace ETModel
{
    public class TulipHelper
    {
        //A0 01注册 02登录realm 03登录gate
        public static async ETVoid Login(string account, string password)
        {
            TulipLoginComponent login = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipLogin)
                .GetComponent<TulipLoginComponent>();

            //创建Realm session
            Session sessionRealm = Game.Scene.GetComponent<NetOuterComponent>()
                .Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            A0002_Login_R2C messageRealm = (A0002_Login_R2C)await sessionRealm.Call(new A0002_Login_C2R()
                { Account = account, Password = password });
            sessionRealm.Dispose();
            login.prompt.text = "Logining...";
            
            //判断Realm服务器返回结果
            if (messageRealm.Error == ErrorCode.ERR_AccountOrPasswordError)
            {
                login.prompt.text = "登录失败,账号或密码错误";
                login.account.text = "";
                login.password.text = "";
                login.isLogining = false;
                return;
            }
            //判断通过则登录Realm成功
            
            //创建网关 seesion
            Session sessionGate = Game.Scene.GetComponent<NetOuterComponent>().Create(messageRealm.GateAddress);
            if (SessionComponent.Instance == null)
            {
                //创建唯一Session
                Game.Scene.AddComponent<SessionComponent>().Session = sessionGate;
            }
            else
            {
                //存入SessionComponent方便我们随时使用
                SessionComponent.Instance.Session = sessionGate;
            }

            A0003_LoginGate_G2C messageGate = (A0003_LoginGate_G2C)await sessionGate.Call(new A0003_LoginGate_C2G() {GateLoginKey = messageRealm.GateLoginKey});
            
            //判断登录Gate服务器返回结果
            if (messageGate.Error == ErrorCode.ERR_ConnectGateKeyError)
            {
                login.prompt.text = "连接网关服务器超时";
                login.account.text = "";
                login.password.text = "";
                sessionGate.Dispose();
                login.isLogining = false;
                return;
            }
            //判断通过则登录Gate成功

            login.prompt.text = "";
            User user = ComponentFactory.Create<User, long>(messageGate.UserID);
            GamerComponent.Instance.MyUser = user;
            
            Log.Debug($"{messageGate.UserID}|{user.UserID}");
            
            Log.Debug("Login Success");
            
            //加载透明界面 退出当前界面
            Game.EventSystem.Run(UIEventType.TulipLoginFinish);
            
            //加载大厅界面
            Game.EventSystem.Run(UIEventType.TulipInitLobby);
        }

        public static async ETVoid Register(string account,string password)
        {
            Session session = Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            A0001_Register_R2C message = (A0001_Register_R2C)await session.Call(new A0001_Register_C2R()
            {
                Account = account,Password = password
            });
            session.Dispose();
            TulipLoginComponent login = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipLogin).GetComponent<TulipLoginComponent>();
            login.isRegistering = false;

            if (message.Error == ErrorCode.ERR_AccountAlreadyRegisted)
            {
                login.prompt.text = "注册失败，账号已被注册";
                login.account.text = "";
                login.password.text = "";
                return;
            }

            if (message.Error == ErrorCode.ERR_RepeatedAccountExist)
            {
                login.prompt.text = "注册失败，出现重复账号";
                login.account.text = "";
                login.password.text = "";
                return;
            }

            login.prompt.text = "注册成功";
        }

    }
}
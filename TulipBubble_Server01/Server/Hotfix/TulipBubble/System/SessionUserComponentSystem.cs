using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class SessionUserComponentSystem : DestroySystem<SessionUserComponent>
    {
        public override void Destroy(SessionUserComponent self)
        {
            try
            {
                //释放User对象时将User对象从管理组件中移除
                Log.Info($"Destroy User and Session");
                Game.Scene.GetComponent<UserComponent>().Remove(self.User.UserID);

                //向登录服务器发送玩家下线消息
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPendPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPendPoint);
                
                realmSession.Send(new A0005_PlayerOffline_G2R(){UserID = self.User.UserID});
                
                //服务端主动断开客户端连接
                Game.Scene.GetComponent<NetOuterComponent>().Remove(self.User.GateSessionID);

                self.User.Dispose();
                self.User = null;
            }
            catch (Exception e)
            {
                Log.Trace(e.ToString());
            }
        }
    }
}
using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class A1001_GetUserInfo_Handler : AMRpcHandler<A1001_GetUserInfo_C2G,A1001_GetUserInfo_G2C>
    {
        protected override async ETTask Run(Session session, A1001_GetUserInfo_C2G request, A1001_GetUserInfo_G2C response, Action reply)
        {

            try
            {
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_UserNotOnline;
                    reply();
                    return;
                }

                User user = session.GetComponent<SessionUserComponent>().User;
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
                UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(user.UserID);

                 response.UserName = userInfo.UserName;
                 response.Rank = userInfo.Rank;

                 reply();
            }
            catch (Exception e)
            {
                ReplyError(response,e,reply);
            }
        }
    }
}
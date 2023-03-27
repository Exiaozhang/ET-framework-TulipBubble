using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GetUserInfoInRoom_Handler : AMRpcHandler<C2G_GetUserInfoInRoom_Req, G2C_GetUserInfoInRoom_Back>
    {
        protected override async ETTask Run(Session session, C2G_GetUserInfoInRoom_Req request,
            G2C_GetUserInfoInRoom_Back response, Action reply)
        {
            try
            {
                //验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = ErrorCode.ERR_SignError;
                    reply();
                    return;
                }
                
                //查询用户信息
                //需要给Gate服务器添加数据库代理组件
                DBProxyComponent dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
                UserInfo userInfo = await dbProxyComponent.Query<UserInfo>(request.UserID);

                response.NickName = userInfo.UserName;
                response.Rank = userInfo.Rank;
                
                reply();
                
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
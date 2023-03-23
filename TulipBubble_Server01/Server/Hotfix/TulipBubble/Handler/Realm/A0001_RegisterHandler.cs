using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class A0001_RegisterHandler : AMRpcHandler<A0001_Register_C2R, A0001_Register_R2C>
    {
        protected override async ETTask Run(Session session, A0001_Register_C2R request, A0001_Register_R2C response,
            Action reply)
        {
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}'}}");;

                if (result.Count == 1)
                {
                    response.Error = ErrorCode.ERR_AccountAlreadyRegisted;
                    reply();
                    return;
                }
                else if(result.Count>1)
                {
                    response.Error = ErrorCode.ERR_RepeatedAccountExist;
                    Log.Error("出现重复账号:"+request.Account);
                    reply();
                    return;
                }

                AccountInfo newAccount = ComponentFactory.CreateWithId<AccountInfo>(RealmHelper.GenerateId());
                newAccount.Account = request.Account;
                newAccount.Password = request.Password;
                await dbProxy.Save(newAccount);

                UserInfo newUser = ComponentFactory.CreateWithId<UserInfo, string>(newAccount.Id, request.Account);
                await dbProxy.Save(newUser);

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
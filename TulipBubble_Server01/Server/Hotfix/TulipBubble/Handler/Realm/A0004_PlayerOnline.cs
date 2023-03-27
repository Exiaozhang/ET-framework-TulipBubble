using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class A0004_PlayerOnline : AMHandler<A0004_PlayerOnline_G2R>
    {
        protected override async ETTask Run(Session session, A0004_PlayerOnline_G2R message)
        {
            OnlineComponent onlineComponent = Game.Scene.GetComponent<OnlineComponent>();

            if (onlineComponent.GetGateAppId(message.UserID) == 0)
            {
                onlineComponent.Add(message.UserID,message.GateAppID);
            }
            else
            {
                Log.Error("The player has received an exception with the repeated online request from the Online Realm server");
            }
            
        }
    }
}
namespace ETModel
{
    [MessageHandler]
    public class TulipBubbleMatcherPlusone_Handler : AMHandler<Actor_TulipMatcherPlusOne_NTT>
    {
        protected override  async ETTask Run(Session session, Actor_TulipMatcherPlusOne_NTT message)
        {
            Log.Debug("Matching Player+1");
            
            await ETTask.CompletedTask;
        }
    }
}
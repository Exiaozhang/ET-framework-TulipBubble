namespace ETModel
{
    [MessageHandler]
    public class TulipBubbleReduceOne_Handler : AMHandler<Actor_TulipMatcherReduceOne_NTT>
    {
        protected override async ETTask Run(Session session, Actor_TulipMatcherReduceOne_NTT message)
        {
            Log.Debug("匹配玩家-1");

            await ETTask.CompletedTask;
        }
    }
}
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class StartMention_TulipBubble_Handler : AMActorHandler<Gamer,Actor_GameStartMention>
    {
        protected override async ETTask Run(Gamer gamer, Actor_GameStartMention message)
        {
            Game.Scene.GetComponent<TulipMatchComponent>().Waiting.TryGetValue(gamer.UserID, out Room room);
            room.GameStart();
            
        }
    }
}
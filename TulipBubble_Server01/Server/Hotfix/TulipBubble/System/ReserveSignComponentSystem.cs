using System.Data;
using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class ReserveSignComponentAwakeSystem : AwakeSystem<ReserveSignComponent>
    {
        public override void Awake(ReserveSignComponent self)
        {
            self.Awake();
        }
    }

    public static class ReserveSignComponentSystem
    {
        public static void Awake(this ReserveSignComponent self)
        {
            self.count = 4;
        }

        /// <summary>
        /// 移除一个玩家标记，并给玩家发送消息同步
        /// </summary>
        /// <param name="self"></param>
        public static void RemoveOneSign(this ReserveSignComponent self)
        {
            if (self.count == 0)
                return;
            self.count -= 1;

            Gamer gamer = self.GetParent<Gamer>();
            ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            ActorMessageSender actorMessageSender = actorProxyComponent.Get(gamer.CActorID);
            actorMessageSender.Send(new Actor_GetSignCount_Ntt()
            {
                SignCount = self.count
            });
        }


        /// <summary>
        /// 添加一个玩家标记，并给玩家发送消息同步
        /// </summary>
        /// <param name="self"></param>
        public static void AddOneSign(this ReserveSignComponent self)
        {
            if (self.count == 4)
                return;
            self.count += 1;

            Gamer gamer = self.GetParent<Gamer>();
            ActorMessageSenderComponent actorMessageSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            ActorMessageSender actorMessageSender = actorMessageSenderComponent.Get(gamer.CActorID);
            actorMessageSender.Send(new Actor_GetSignCount_Ntt()
            {
                SignCount = self.count
            });
        }
    }
}
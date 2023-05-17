using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class RoomEventCardsComponentAwakeSystem : AwakeSystem<RoomEventCardsComponent>
    {
        public override void Awake(RoomEventCardsComponent self)
        {
        }
    }

    public static class RoomEventCardsComponentSystem
    {
        /// <summary>
        /// 设定当前的事件卡
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eventCard"></param>
        public static void SetCurrentEvent(this RoomEventCardsComponent self, EventCard eventCard)
        {
            self.currentEvent = eventCard;
        }
        
        
        
    }
}
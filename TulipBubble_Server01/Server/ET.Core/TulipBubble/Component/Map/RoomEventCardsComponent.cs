namespace ETModel
{
    public class RoomEventCardsComponent : Component
    {
        //当前本回的事件
        public EventCard currentEvent;
        
        //牌堆中剩余的事件牌的数量
        public int remainderEventCardCount;

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.currentEvent = null;
            remainderEventCardCount = 0;
        }
    }
}
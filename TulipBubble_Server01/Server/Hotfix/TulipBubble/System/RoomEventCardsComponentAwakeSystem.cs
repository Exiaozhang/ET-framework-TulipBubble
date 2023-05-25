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
            Room room = self.GetParent<Room>();
            self.currentEvent = eventCard;
            self.TriggerEventCard(eventCard);

            room.Broadcast(new Actor_GetEvent_Ntt()
            {
                EventCard = self.currentEvent,
                RemindEventCount = self.remainderEventCardCount
            });
        }

        public static void TriggerEventCard(this RoomEventCardsComponent self, EventCard eventCard)
        {
            Room room = self.GetParent<Room>();
            TulipMarketEconomicsComponent tulipMarketEconomicsComponent =
                room.GetComponent<TulipMarketEconomicsComponent>();
            TulipColor eventCardTulipColor = (TulipColor)eventCard.TulipColor;
            MarketEvent eventCardEventType = (MarketEvent)eventCard.EventType;

            switch (eventCardEventType)
            {
                case MarketEvent.TulipRise:
                    tulipMarketEconomicsComponent.RiseTulipPriceLevel((int)eventCardTulipColor);
                    break;
                case MarketEvent.TulipDrop:
                    tulipMarketEconomicsComponent.DropTulipPriceLevel((int)eventCardTulipColor);
                    break;
                case MarketEvent.RiseSuddenly:
                    tulipMarketEconomicsComponent.RiseSuddenlyPriceLevel();
                    break;
                case MarketEvent.DropSuddenly:
                    tulipMarketEconomicsComponent.DropSuddenlyPriceLevel();
                    break;
                case MarketEvent.BubbleBurst:
                    break;
            }

            room.Broadcast(new Actor_GetTulipPriceLevel_Ntt()
            {
                RedPriceLevel = tulipMarketEconomicsComponent.redPrieceLevel,
                WhitePriceLevel = tulipMarketEconomicsComponent.whitePrieceLevel,
                YellowPriceLevel = tulipMarketEconomicsComponent.yellowPrieceLevel
            });
        }
    }
}
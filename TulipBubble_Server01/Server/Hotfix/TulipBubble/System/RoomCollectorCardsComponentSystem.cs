using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class RoomCollectorCardsComponentAwakeSystem : AwakeSystem<RoomCollectorCardsComponent>
    {
        public override void Awake(RoomCollectorCardsComponent self)
        {

        }
    }

    public static class RoomCollectorCardsComponentSystem
    {
        public static void RemoveCollectorCard(this RoomCollectorCardsComponent self, CollectorCard collectorCard)
        {
            if (self.lowPriceCollector == collectorCard)
            {
                self.GetNewCollectorCard(CollectorPriceLevel.low);
                return;
            }
            if (self.middlePriceCollector == collectorCard)
            {
                self.GetNewCollectorCard(CollectorPriceLevel.middle);
                return;
            }
            if (self.highPriceCollector == collectorCard)
            {
                self.GetNewCollectorCard(CollectorPriceLevel.high);
                return;
            }

        }

        private static void GetNewCollectorCard(this RoomCollectorCardsComponent self, CollectorPriceLevel collectorPriceLevel)
        {
            Room room = self.GetParent<Room>();
            DeckComponent deckComponent = room.GetComponent<DeckComponent>();


            if (collectorPriceLevel == CollectorPriceLevel.low)
            {
                self.lowPriceCollector = deckComponent.DealLowPriceCollectorCard();
                self.lowPriceCollectorCount = deckComponent.LowPriceCollectorCount;
                return;
            }
            if (collectorPriceLevel == CollectorPriceLevel.middle)
            {
                self.middlePriceCollector = deckComponent.DealMiddlePriceCollectorCard();
                self.middlePriceCollectorCount = deckComponent.MiddlePriceCollectorCount;
                return;
            }
            if (collectorPriceLevel == CollectorPriceLevel.high)
            {
                self.highPriceCollector = deckComponent.DealHighPriceCollectorCard();
                self.highPriceCollectorCount = deckComponent.HighPriceCollectorCount;
                return;
            }
        }

    }
}
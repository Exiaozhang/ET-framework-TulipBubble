namespace ETModel
{
    public class RoomCollectorCardsComponent : Component
    {
        //高价位
        public CollectorCard highPriceCollector = new CollectorCard();
        public int highPriceCollectorCount;

        //中价位
        public CollectorCard middlePriceCollector = new CollectorCard();
        public int middlePriceCollectorCount;

        //低价位
        public CollectorCard lowPriceCollector = new CollectorCard();
        public int lowPriceCollectorCount;


        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
    }
}
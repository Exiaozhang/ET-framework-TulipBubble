using System;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class MoneyComponentAwkaeSystem : AwakeSystem<MoneyComponent>
    {
        public override void Awake(MoneyComponent self)
        {
            self.Awake();
        }
    }


    public static class MoneyComponentSystem
    {
        public static void Awake(this MoneyComponent self)
        {
            self.money = 20;
        }

        public static void LoansForTulipCard(this MoneyComponent self, TulipCard tulipCard, Int32 Price)
        {
            
        }

    }
}
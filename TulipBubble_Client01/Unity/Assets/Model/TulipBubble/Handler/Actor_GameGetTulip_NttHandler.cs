using System.Linq;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetTulip_NttHandler : AMHandler<Actor_GetTulip_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetTulip_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomTulipCardsComponent tulipRoomTulipCardsComponent = uiRoom.GetComponent<TulipRoomTulipCardsComponent>();
            
            //清空进货卡牌
            tulipRoomTulipCardsComponent.ClearTulipCards(TulipMarkType.future);
            //添加进货卡牌
            tulipRoomTulipCardsComponent.AddTulipCards(message.FutureTulipCards.ToArray(),TulipMarkType.future);
            //清空现货卡牌
            tulipRoomTulipCardsComponent.ClearTulipCards(TulipMarkType.cash);
            //添加现货卡牌
            tulipRoomTulipCardsComponent.AddTulipCards(message.CashTulipCards.ToArray(),TulipMarkType.cash);
            //清空现货卡牌
            tulipRoomTulipCardsComponent.ClearTulipCards(TulipMarkType.selled);
            //添加现货卡牌
            tulipRoomTulipCardsComponent.AddTulipCards(message.CashTulipCards.ToArray(),TulipMarkType.selled);
        }
    }
}
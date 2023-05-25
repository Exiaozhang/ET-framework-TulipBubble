using System.Linq;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetReserveTulip_NttHandler : AMHandler<Actor_GetTulipReserve_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetTulipReserve_Ntt message)
        {
            TulipRoomTulipCardsComponent tulipRoomTulipCardsComponent = Game.Scene.GetComponent<UIComponent>()
                .Get(TulipUIType.TulipRoom).GetComponent<TulipRoomTulipCardsComponent>();
            tulipRoomTulipCardsComponent.UpdateSignObj(message.ReserveTulipCards.ToList());
        }
    }
}
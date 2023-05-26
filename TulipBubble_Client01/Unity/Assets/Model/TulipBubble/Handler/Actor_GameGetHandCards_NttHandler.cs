using System.Data;
namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetHandCards_NttHand : AMHandler<Actor_GetHandCard_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetHandCard_Ntt message)
        {
            TulipRoomGamerHandCardComponent tulipRoomGamerHandCardComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerHandCardComponent>();
            tulipRoomGamerHandCardComponent.SetHandCards(TulipHelper.RepeatedFieldToList(message.HandCard), TulipHelper.RepeatedFieldToList(message.LoanCard), TulipHelper.RepeatedFieldToList(message.CardPrice));
        }
    }
}
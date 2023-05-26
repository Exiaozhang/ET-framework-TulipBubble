namespace ETModel
{
    [MessageHandler]
    public class Actor_GameGetReserveObj_NttHandler : AMHandler<Actor_GetSignCount_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GetSignCount_Ntt message)
        {
            TulipRoomGamerReserveSignObjComponent tulipRoomGamerReserveSignObjComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerReserveSignObjComponent>();

            tulipRoomGamerReserveSignObjComponent.SetSignCount(message.SignCount);
        }
    }
}
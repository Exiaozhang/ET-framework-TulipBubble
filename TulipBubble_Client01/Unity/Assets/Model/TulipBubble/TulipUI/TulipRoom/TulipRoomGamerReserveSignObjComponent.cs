using System;
namespace ETModel
{
    [ObjectSystem]
    public class TulipRoomGamerReserveSignObjComponentAwakeSystem : AwakeSystem<TulipRoomGamerReserveSignObjComponent>
    {
        public override void Awake(TulipRoomGamerReserveSignObjComponent self)
        {
            self.signCount = 0;
        }
    }

    public class TulipRoomGamerReserveSignObjComponent : Component
    {
        public int signCount;

        public void SetSignCount(Int32 count)
        {
            signCount = count;
            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();
            tulipRoomGamerPanelComponent.SetSignObj(signCount);
        }

        public Int32 GetSignCount(Int32 count)
        {
            return signCount;
        }

        public void RemoveOneReserveObj()
        {
            signCount -= 1;
            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();
            tulipRoomGamerPanelComponent.SetSignObj(signCount);
        }

        
    }

}
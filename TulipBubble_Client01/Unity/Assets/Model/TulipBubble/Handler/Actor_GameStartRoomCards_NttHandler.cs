using System.Linq;
using UnityEngine;

namespace ETModel
{
    [MessageHandler]
    public class Actor_GameStartRoomCards_NttHandler : AMHandler<Actor_GameStartRoomCards_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_GameStartRoomCards_Ntt message)
        {
            UI uiRoom = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent room = uiRoom.GetComponent<TulipRoomComponent>();
            RoomCardsComponent roomCardsComponent = uiRoom.GetComponent<RoomCardsComponent>();

            //取消显示开始按钮
            GameObject startButton = uiRoom.GameObject.Get<GameObject>("StartGame");
            startButton.SetActive(false);

            //添加进货卡牌
            roomCardsComponent.AddTulipCards(message.FutureTulipCards.ToArray(),TulipMarkType.future);
            //添加现货卡牌
            roomCardsComponent.AddTulipCards(message.CashTulipCards.ToArray(),TulipMarkType.cash);
            
        }
    }
}
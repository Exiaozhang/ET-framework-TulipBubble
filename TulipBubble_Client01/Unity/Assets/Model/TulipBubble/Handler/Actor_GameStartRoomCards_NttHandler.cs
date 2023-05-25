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
            TulipRoomTulipCardsComponent tulipRoomTulipCardsComponent = uiRoom.GetComponent<TulipRoomTulipCardsComponent>();


            //取消显示开始按钮
            GameObject startButton = uiRoom.GameObject.Get<GameObject>("StartGame");
            startButton.SetActive(false);

            GameObject matchPrompt = uiRoom.GameObject.Get<GameObject>("MatchPrompt");
            matchPrompt.SetActive(false);

            //添加进货卡牌
            tulipRoomTulipCardsComponent.AddTulipCards(message.FutureTulipCards.ToArray(), TulipMarkType.future);
            //添加现货卡牌
            tulipRoomTulipCardsComponent.AddTulipCards(message.CashTulipCards.ToArray(), TulipMarkType.cash);

            //玩家面板提示开始游戏
            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();
            tulipRoomGamerPanelComponent.SetGameStart();

        }
    }
}
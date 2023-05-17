namespace ETModel
{
    [MessageHandler]
    public class Actor_GameAuthorityPlayCard_NttHandler : AMHandler<Actor_AuthorityPlayCard_Ntt>
    {
        protected override async ETTask Run(Session session, Actor_AuthorityPlayCard_Ntt message)
        {
            UI ui = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom);
            TulipRoomComponent room = ui.GetComponent<TulipRoomComponent>();
            TulipRoomGamerPanelComponent playerPanel = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();
            
            TulipRoomGameComponent roomGame = ui.GetComponent<TulipRoomGameComponent>();
            roomGame.currentTurnUserId = message.UserID;
            roomGame.stage = (GameStage)message.Stage;
            
            if (room.IsLocalGame(message.UserID))
            {
                playerPanel.SetMyTurn();
                room.Interaction.SetPassButton(true);
            }
            else
            {
                room.Interaction.SetPassButton(false);
                playerPanel.SetOtherTurn();
            }
        }
    }
}
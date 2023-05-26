namespace ETModel
{
    [MessageHandler]
    class Actor_GameNotifyPlayerBid_NttHandler : AMHandler<Actor_NotifyPlayerBid>
    {
        protected override async ETTask Run(Session session, Actor_NotifyPlayerBid message)
        {
            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();

            if (message.UserId != TulipRoomComponent.LocalGamer.UserID)
            {
                Log.Info($"{message.UserId}{message.LowestPrice}");
                return;
            }

            tulipRoomGamerPanelComponent.SetMyBidTurn(message.LowestPrice,message.BidingTulipCard);
        }
    }

}
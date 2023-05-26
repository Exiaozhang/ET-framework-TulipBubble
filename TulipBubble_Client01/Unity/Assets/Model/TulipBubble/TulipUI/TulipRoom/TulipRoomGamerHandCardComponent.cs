using System;
using System.Collections.Generic;
namespace ETModel
{
    public class TulipRoomGamerHandCardComponent : Component
    {
        public List<TulipCard> tulipCardsLibrary = new List<TulipCard>();
        public Dictionary<TulipCard, Int32> loanCardsLibrary = new Dictionary<TulipCard, int>();

        public void SetHandCards(List<TulipCard> cards, List<TulipCard> loanCards, List<Int32> prices)
        {
            tulipCardsLibrary = cards;

            int i = 0;
            loanCardsLibrary.Clear();
            foreach (var loanCard in loanCards)
            {
                Log.Info("Hello World");
                loanCardsLibrary.Add(loanCard, prices[i]);
            }

            TulipRoomGamerPanelComponent tulipRoomGamerPanelComponent = TulipRoomComponent.LocalGamer.GetComponent<TulipRoomGamerPanelComponent>();
            tulipRoomGamerPanelComponent.SetHandCards();

        }
    }
}
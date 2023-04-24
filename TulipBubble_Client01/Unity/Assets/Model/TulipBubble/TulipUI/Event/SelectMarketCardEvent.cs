using UnityEngine;

namespace ETModel
{
    [Event(ETModel.EventIdType.SelectMarketCard)]
    public class SelectMarketCardEvent : AEvent<TulipCard,GameObject>
    {
        public override void Run(TulipCard card,GameObject cardObj)
        {
            
            Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom).GetComponent<TulipRoomComponent>().Interaction.SetSelectedMarketTulipCard(card,cardObj);
        }
    }
}
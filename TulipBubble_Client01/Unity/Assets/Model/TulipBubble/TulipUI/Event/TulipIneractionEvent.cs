using System;
using UnityEngine;

namespace ETModel
{
    [Event(ETModel.EventIdType.SelectCard)]
    public class SelectCardEvent : AEvent<System.Object, GameObject>
    {
        public override void Run(System.Object card, GameObject cardObj)
        {
            Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom).GetComponent<TulipRoomComponent>()
                .Interaction.SetSelectedCard(card, cardObj);
        }
    }

    [Event(ETModel.EventIdType.HoverCard)]
    public class HoverCardEvent : AEvent<System.Object, String, Transform>
    {
        public override void Run(System.Object card, String message, Transform transform)
        {
            TulipRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom)
                .GetComponent<TulipRoomComponent>();
   
            room.Interaction.SetHoverCard(card, message, transform);
        }
    }

    [Event(ETModel.EventIdType.CancelHoverCard)]
    public class CancelHoverCardEvent : AEvent<System.Object>
    {
        public override void Run(System.Object a)
        {
            TulipRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(TulipUIType.TulipRoom)
                .GetComponent<TulipRoomComponent>();
            room.Interaction.CancelHoverCard(a);
        }
    }
}
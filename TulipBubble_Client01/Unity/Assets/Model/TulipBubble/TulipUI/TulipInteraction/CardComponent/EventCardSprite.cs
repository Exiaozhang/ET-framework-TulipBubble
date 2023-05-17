using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class EventCardSprite : MonoBehaviour
    {
        public EventCard card;
        [NonSerialized] public int deckCount;

        private void Start()
        {
            EventTrigger eventTrigger = this.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry hoverEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            hoverEntry.callback.AddListener(OnPointerEnter);
            eventTrigger.triggers.Add(hoverEntry);

            EventTrigger.Entry notHoverEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit
            };
            notHoverEntry.callback.AddListener(OnPinterLeave);
            eventTrigger.triggers.Add(notHoverEntry);
        }

        private void OnPointerEnter(BaseEventData data)
        {
            Game.EventSystem.Run(EventIdType.HoverCard, card, "剩余" + deckCount.ToString(), this.gameObject.transform);
        }

        private void OnPinterLeave(BaseEventData data)
        {
            Game.EventSystem.Run(EventIdType.CancelHoverCard, card);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class CollectorCardSprite : MonoBehaviour
    {
        public CollectorCard card;
        [NonSerialized] public int deckCount;

        /// <summary>
        /// 初始化UI事件
        /// </summary>
        private void Start()
        {
            EventTrigger eventTrigger = this.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry clickEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            clickEntry.callback.AddListener(OnClick);
            eventTrigger.triggers.Add(clickEntry);

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

        private void OnClick(BaseEventData data)
        {
            if (card == null)
                Log.Info("Error Card is Null");

            Game.EventSystem.Run(EventIdType.SelectCard, card, this.gameObject);
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
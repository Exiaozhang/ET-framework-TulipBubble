using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class HandCardSprite : MonoBehaviour
    {
        public TulipCard card;

        public bool isFurtueCard = false;

        //public bool

        private void Start()
        {
            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback = new EventTrigger.TriggerEvent();
            clickEntry.callback.AddListener(new UnityAction<BaseEventData>(OnClick));
            eventTrigger.triggers.Add(clickEntry);
        }


        private void OnClick(BaseEventData data)
        {
            if (isFurtueCard)
                return;
            if (card == null)
            {
                Log.Error("card is null");
                return;
            }

            Game.EventSystem.Run(EventIdType.SelectCard, card, this.gameObject);
        }
    }
}
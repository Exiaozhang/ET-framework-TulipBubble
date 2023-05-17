using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class TulipRoomEventCardsComponentAwakeSystem : AwakeSystem<TulipRoomEventCardsComponent>
    {
        public override void Awake(TulipRoomEventCardsComponent self)
        {
            self.Awake();
        }
    }


    public class TulipRoomEventCardsComponent : Component
    {
        private GameObject eventDeck;
        private GameObject eventDiscardPile;

        private GameObject currentEventCardObj;
        private GameObject currentEventDiscardObj;

        public void Awake()
        {
            GameObject room = this.GetParent<UI>().GameObject;
            ReferenceCollector referenceCollector = room.GetComponent<ReferenceCollector>().Get<GameObject>("Panel")
                .GetComponent<ReferenceCollector>();
            eventDiscardPile = referenceCollector.Get<GameObject>("EventDiscardPile");

            eventDeck = referenceCollector.Get<GameObject>("EventDeck");

            //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{CardHelper.EVENTATLAS}.unity3d");
            resourcesComponent.LoadBundle($"{CardHelper.EVENTCARD}.unity3d");
        }

        public void SetEventDiscardPile(EventCard eventCard)
        {
            if (eventCard == null)
                return;

            GameObject card = CardHelper.CreateCardObj(CardHelper.COLLECTORCARD, eventCard.GetName(),
                eventDiscardPile.transform, CardType.EventCard);
            currentEventCardObj = card;
        }

        public void SetEventCardPile(int count)
        {
            if (count == 0 && currentEventDiscardObj != null)
            {
                UnityEngine.Object.Destroy(currentEventDiscardObj);
                return;
            }

            if (currentEventDiscardObj != null)
            {
                EventCardSprite eventCardSprite = currentEventCardObj.GetComponent<EventCardSprite>();
                eventCardSprite.deckCount = count;
            }

            GameObject card = CardHelper.CreateCardObj(CardHelper.COLLECTORCARD, "CardBack",
                eventDeck.transform,
                CardType.EventCard);
            
            EventCardSprite eventCard = card.AddComponent<EventCardSprite>();
            eventCard.card = new EventCard();
            eventCard.deckCount = count;

            currentEventDiscardObj = card;
        }
    }
}
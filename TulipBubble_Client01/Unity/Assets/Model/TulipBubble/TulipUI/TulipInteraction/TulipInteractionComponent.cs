using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class LandInteractionComponentAwakeSystem : AwakeSystem<TulipInteractionComponent>
    {
        public override void Awake(TulipInteractionComponent self)
        {
            self.Awake();
        }
    }

    public class TulipInteractionComponent : Component
    {
        private TulipCard selectedMarketTulipCard;
        private GameObject selectedMarketTulipCardObj;
        public void Awake()
        {
            
            
        }

        public void SetSelectedMarketTulipCard(TulipCard card,GameObject cardObj)
        {
            
            if (selectedMarketTulipCard == null)
            {
                SelectedMarketTulipCard(card,cardObj);
                cardObj.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if(selectedMarketTulipCard == card)
            {
                CancelSelectedMarketTulipCard();
                cardObj.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                selectedMarketTulipCardObj.transform.GetChild(0).gameObject.SetActive(false);
                SelectedMarketTulipCard(card,cardObj);
                cardObj.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void SelectedMarketTulipCard(TulipCard card,GameObject cardObj)
        {
            selectedMarketTulipCard = card;
            selectedMarketTulipCardObj = cardObj;
        }

        private void CancelSelectedMarketTulipCard()
        {
            selectedMarketTulipCard = null;
            selectedMarketTulipCardObj = null;
        }
        
        
    }
    
}
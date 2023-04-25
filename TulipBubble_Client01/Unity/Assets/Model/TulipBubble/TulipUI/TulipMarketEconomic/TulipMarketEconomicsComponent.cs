using System.Collections;
using UnityEngine;


namespace ETModel
{
    [ObjectSystem]
    public class TulipMarketEconomicsComponentAwakeSystem : AwakeSystem<TulipMarketEconomicsComponent>
    {
        public override void Awake(TulipMarketEconomicsComponent self)
        {
            self.Awake();
        }
    }

    public class TulipMarketEconomicsComponent : Component
    {
        public int redTulipPriceLevel;
        public int whiteTulipPriceLevel;
        public int yellowTulipPriceLevel;

        private Transform _Level_1;
        private Transform _Level_2;
        private Transform _Level_3;
        private Transform _Level_4;
        private Transform _Level_5;
        private Transform _Level_6;
        private Transform _Level_7;

        private Transform _RedTulip;
        private Transform _WhiteTulip;
        private Transform _YellowTulip;

        private MarketEconomicsHelper _helper;

        public void Awake()
        {
            ReferenceCollector referenceCollector = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            _Level_7 = referenceCollector.Get<GameObject>("Level_7").transform;
            _Level_6 = referenceCollector.Get<GameObject>("Level_6").transform;
            _Level_5 = referenceCollector.Get<GameObject>("Level_5").transform;
            _Level_4 = referenceCollector.Get<GameObject>("Level_4").transform;
            _Level_3 = referenceCollector.Get<GameObject>("Level_3").transform;
            _Level_2 = referenceCollector.Get<GameObject>("Level_2").transform;
            _Level_1 = referenceCollector.Get<GameObject>("Level_1").transform;

            _RedTulip = referenceCollector.Get<GameObject>("RedTulip").transform;
            _WhiteTulip = referenceCollector.Get<GameObject>("WhiteTulip").transform;
            _YellowTulip = referenceCollector.Get<GameObject>("YellowTulip").transform;

            GameObject helperObj = new GameObject();
            helperObj.name = "MarketEconomicsHelper";
            _helper = helperObj.AddComponent<MarketEconomicsHelper>();
        }

        /// <summary>
        /// 设置所有种类郁金香价格等级
        /// </summary>
        /// <param name="redPriceLevel"></param>
        /// <param name="whitePriceLevel"></param>
        /// <param name="yellowPriceLevel"></param>
        public void SetPriceLvel(int redPriceLevel, int whitePriceLevel, int yellowPriceLevel)
        {
            if (!_RedTulip.gameObject.activeSelf)
            {
                _RedTulip.gameObject.SetActive(true);
            }

            if (!_WhiteTulip.gameObject.activeSelf)
            {
                _WhiteTulip.gameObject.SetActive(true);
            }

            if (!_YellowTulip.gameObject.activeSelf)
            {
                _YellowTulip.gameObject.SetActive(true);
            }

            redTulipPriceLevel = redPriceLevel;
            Transform redTarget = GetPriceLevelPosition(redTulipPriceLevel);
            _helper.StartCoroutine(MoveTulipToPriceLevelPosition(_RedTulip, redTarget));

            whiteTulipPriceLevel = whitePriceLevel;
            Transform whiteTarget = GetPriceLevelPosition(whiteTulipPriceLevel);
            _helper.StartCoroutine(MoveTulipToPriceLevelPosition(_WhiteTulip, whiteTarget));

            yellowTulipPriceLevel = yellowPriceLevel;
            Transform yellowTarget = GetPriceLevelPosition(yellowTulipPriceLevel);
            _helper.StartCoroutine(MoveTulipToPriceLevelPosition(_YellowTulip, yellowTarget));
        }


        private IEnumerator MoveTulipToPriceLevelPosition(Transform origin, Transform target)
        {
            while (origin.position != target.position)
            {
                origin.position = Vector3.MoveTowards(origin.position,
                    target.position, 10 * Time.deltaTime);
            }

            yield return 0;
        }

        private Transform GetPriceLevelPosition(int level)
        {
            switch (level)
            {
                case 1:
                    return _Level_1;
                case 2:
                    return _Level_2;
                case 3:
                    return _Level_3;
                case 4:
                    return _Level_4;
                case 5:
                    return _Level_5;
                case 6:
                    return _Level_6;
                case 7:
                    return _Level_7;
            }

            Log.Info("Error Can Not Find Price Position");
            return _Level_1;
        }
    }

    class MarketEconomicsHelper : MonoBehaviour
    {
    }
}
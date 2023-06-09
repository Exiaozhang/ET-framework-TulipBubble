﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class
        TulipRoomGamerOrderPanelComponentAwakeSystem : AwakeSystem<TulipRoomGamerOrderPanelComponent, GameObject, int>
    {
        public override void Awake(TulipRoomGamerOrderPanelComponent self, GameObject parent, int i)
        {
            self.Awake(parent, i);
        }
    }

    public class TulipRoomGamerOrderPanelComponent : Component
    {
        private string color;
        private Sprite spriteColor;
        public static string[] userColor = new[] { "blue", "green", "purple", "red", "yellow" };
        private GameObject orderPlayer;
        private GameObject ready;
        private GameObject hoster;
        

        public void Awake(GameObject AllGamers, int i)
        {
            //加载AB包
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"OrderPlayer.unity3d");

            //加载玩家顺序界面
            GameObject boundGameObject = (GameObject)resourcesComponent.GetAsset($"OrderPlayer.unity3d", "OrderPlayer");
            orderPlayer = UnityEngine.Object.Instantiate(boundGameObject);

            //放置界面
            orderPlayer.layer = LayerMask.NameToLayer(LayerNames.UI);
            orderPlayer.transform.SetParent(AllGamers.transform);
            orderPlayer.transform.localScale = new Vector3(1, 1, 1);
            orderPlayer.transform.localPosition =
                new Vector3(orderPlayer.transform.position.x, orderPlayer.transform.position.y, 0f);

            ReferenceCollector referenceCollector = orderPlayer.GetComponent<ReferenceCollector>();
            GameObject userInfo = referenceCollector.Get<GameObject>("UserInfo");

            color = userColor[i];
            spriteColor = Resources.Load<Sprite>($"Player_{color}");

            userInfo.GetComponent<Image>().sprite = spriteColor;
            //AllGamers.GetComponent<ReferenceCollector>().Add($"OrderPlayer_{color}", orderPlayer);

            ready = referenceCollector.Get<GameObject>("Ready");
            hoster = referenceCollector.Get<GameObject>("Hoster");
            //Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("OrderPlayer.unity3d");
        }

        public void SetGamerReady()
        {
            ready.SetActive(true);
        }

        public void SetGamerHoster()
        {
            hoster.SetActive(true);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            UnityEngine.Object.Destroy(orderPlayer);

            base.Dispose();
        }
    }
}
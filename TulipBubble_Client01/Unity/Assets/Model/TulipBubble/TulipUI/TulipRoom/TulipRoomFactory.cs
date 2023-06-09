﻿using System;
using UnityEngine;

namespace ETModel
{
    [UIFactory(TulipUIType.TulipRoom)]
    public class TulipRoomFactory : IUIFactory
    {
        public UI Create(Scene scene, string type, GameObject parent)
        {
            try
            {
                //加载AB包
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"{type}.unity3d");

                //加载大厅界面预设并生成实例
                GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
                GameObject tulipRoom = UnityEngine.Object.Instantiate(bundleGameObject);


                //设置UI层级，只有UI摄像机可以渲染
                tulipRoom.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI, GameObject>(tulipRoom);

                ui.AddComponent<TulipRoomComponent>();
                ui.AddComponent<TulipRoomTulipCardsComponent>();
                ui.AddComponent<TulipRoomCollectorCardsComponent>();
                ui.AddComponent<TulipRoomGameComponent>();
                ui.AddComponent<TulipRoomEventCardsComponent>();

                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public void Remove(string type)
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
    }
}
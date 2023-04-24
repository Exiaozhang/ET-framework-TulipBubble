using UnityEngine;

namespace ETModel
{
    public class TulipMarketEconomicsFactory
    {
        public static UI Create(string type, UI parent)
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{type}.unity3d");
            GameObject prefab = (GameObject)resourcesComponent.GetAsset($"{type}.unity3d", $"{type}");
            GameObject marketEconomics = UnityEngine.Object.Instantiate(prefab);

            marketEconomics.layer = LayerMask.NameToLayer("UI");

            UI ui = ComponentFactory.Create<UI,GameObject>(marketEconomics);
            parent.Add(ui);
            ui.GameObject.transform.SetParent(parent.GameObject.transform,false);
            ui.AddComponent<TulipMarketEconomicsComponent>();
            return ui;
        }
    }
}
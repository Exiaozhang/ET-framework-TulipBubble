using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        private void Start()
        {
            this.StartAsync().Coroutine();
        }

        private async ETVoid StartAsync()
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);
                ClientConfigHelper.SetConfigHelper();
                Game.EventSystem.Add(DLLType.Core, typeof(Core).Assembly);
                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                Game.Scene.AddComponent<GlobalConfigComponent>(); //web资源服务器设置组件
                Game.Scene.AddComponent<ResourcesComponent>(); //资源加载组件
                
                //下载Ab包
                await BundleHelper.DownloadBundle();
                
                //测试输出正确加载了Config所带的消息
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

                UnitConfig unitConfig =
                    (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
                Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");


                //添加指定与网络组件
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();

                //添加UI组件
                Game.Scene.AddComponent<UIComponent>();
                Game.Scene.AddComponent<GamerComponent>();
                
                //消息分发组件
                Game.Scene.AddComponent<MessageDispatcherComponent>();
                Game.Scene.AddComponent<GameGlobalComponent>();
                //创建TulipLogin界面
                Game.EventSystem.Run(UIEventType.TulipInitSceneStart);
                
       
        
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}
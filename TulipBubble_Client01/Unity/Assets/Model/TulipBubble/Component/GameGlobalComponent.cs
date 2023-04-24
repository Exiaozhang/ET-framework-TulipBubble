using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class GameGlobalComponentAwakeSystem : AwakeSystem<GameGlobalComponent>
    {
        public override void Awake(GameGlobalComponent self)
        {
            self.Awake();
        }
    }

    public class GameGlobalComponent : Component
    {
        public void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        public void SetFps(float fps)
        {
            Application.targetFrameRate = 60;
        }
    }
}
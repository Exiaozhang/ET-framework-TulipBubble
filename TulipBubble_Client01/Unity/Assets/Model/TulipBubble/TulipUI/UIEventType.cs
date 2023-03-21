using System;
using System.Collections.Generic;

namespace ETModel
{
    public static partial class TulipUIType
    {
        public const string TulipLogin = "TulipLogin";
    }
    
    public static partial class UIEventType
    {
        public const string TulipInitSceneStart = "TulipSceneStart";
    }

    [Event(UIEventType.TulipInitSceneStart)]
    public class InitSceneStart_CreateTulipLogin : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(TulipUIType.TulipLogin);
        }
    }


}
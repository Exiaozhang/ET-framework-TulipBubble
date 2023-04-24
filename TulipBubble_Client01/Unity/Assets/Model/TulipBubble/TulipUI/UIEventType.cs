using System;
using System.Collections.Generic;

namespace ETModel
{
    public static partial class TulipUIType
    {
        public const string TulipLogin = "TulipLogin";
        public const string TulipLobby = "TulipLobby";
        public const string SetUserInfo = "SetUserInfo";
        public const string TulipRoom = "TulipRoom";
        public const string TulipInteraction = "TulipInteraction";
        public const string TulipMarketEconomics = "TulipMarketEconomics";
    }

    public static partial class UIEventType
    {
        public const string TulipInitSceneStart = "TulipSceneStart";
        public const string TulipLoginFinish = "TulipLoginFinish";
        public const string TulipInitLobby = "TulipInitLobby";
    }

    [Event(UIEventType.TulipInitSceneStart)]
    public class InitSceneStart_CreateTulipLogin : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(TulipUIType.TulipLogin);
        }
    }
    
    [Event(UIEventType.TulipLoginFinish)]
    public class TulipLoginFinish : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(TulipUIType.TulipLogin);
        }
    }

    //初始化大厅
    [Event(UIEventType.TulipInitLobby)]
    public class TulipInitLobby : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Create(TulipUIType.TulipLobby);
        }
    }
}
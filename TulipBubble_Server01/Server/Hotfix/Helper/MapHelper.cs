using System.Collections.Generic;
using System.Net;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class MapHelper
    {
        public static Session GetGateSession()
        {
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint gateIPEndPoint = config.GateConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            //Log.Debug(gateIPEndPoint.ToString());
            Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateIPEndPoint);
            return gateSession;
        }

        /// <summary>
        /// 容器转换辅助方法
        /// </summary>
        public static class To
        {
            //数组-RepeatedField
            public static RepeatedField<T> RepeatedField<T>(T[] array)
            {
                Google.Protobuf.Collections.RepeatedField<T> a = new RepeatedField<T>();
                foreach (T b in array)
                {
                    a.Add(b);
                }

                return a;
            }
            
            //列表-RepeatedField
            public static RepeatedField<T> RepeatedField<T>(List<T> list)
            {
                RepeatedField<T> a = new RepeatedField<T>();
                foreach (var b in list)
                {
                    a.Add(b);
                }
                return a;
            }
            
            //重复字段-列表
            public static List<T> List<T>(RepeatedField<T> repeatedField)
            {
                List<T> a = new List<T>();
                foreach (var b in repeatedField)
                {
                    a.Add(b);
                }
                return a;
            }
            
            
        }
    }
}
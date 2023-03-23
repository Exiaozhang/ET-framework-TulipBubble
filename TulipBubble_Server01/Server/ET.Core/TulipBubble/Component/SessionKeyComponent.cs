using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace ETModel
{
    // 用户登录的实现过程，先到到网关gate请求登录，验证账号名密码后，就会到realm上获得一个sessionKey，这说明你是认证过的合法用户了，
    // SessionKeyComponent就是用这个sessionKey作为key，UserID作为值存起来了，这样就能识别非法请求，一概不理或作处理

    public class SessionKeyComponent : Component
    {
        private readonly Dictionary<long, long> sessionKey = new Dictionary<long, long>();

        public void Add(long key,long userId)
        {
            this.sessionKey.Add(key,userId);
            this.TimeoutRemoveKey(key);
        }

        public long Get(long key)
        {
            long userId;
            this.sessionKey.TryGetValue(key, out userId);
            return userId;
        }

        public void Remove(long key)
        {
            this.sessionKey.Remove(key);
        }

        private async void TimeoutRemoveKey(long key)
        {
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(2000);
            this.sessionKey.Remove(key);
        }
    }
}
namespace ETModel
{
    [ObjectSystem]
    public class UserInfoAwakeSystem : AwakeSystem<UserInfo, string>
    {
        public override void Awake(UserInfo self, string name)
        {
            self.Awake(name);
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : Entity
    {
        //昵称
        public string UserName { get; set; }

        //等级分
        public int Rank;

        //上次游戏角色位置 1/2/3/4/5
        public int LastPlay { get; set; }


        public void Awake(string name)
        {
            UserName = name;
            Rank = 1500;
        }
    }
}
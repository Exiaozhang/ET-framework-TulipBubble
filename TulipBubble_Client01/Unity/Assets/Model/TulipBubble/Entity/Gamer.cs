using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class GamerSystem : AwakeSystem<Gamer, long>
    {
        public override void Awake(Gamer self,long userid)
        {
            self.Awake(userid);   
        }
    }

    public class Gamer : Entity
    {
        /// <summary>
        /// 每个玩家绑定一个实体 机器人的UserID为0
        /// </summary>
        public long UserID { get; private set; }

        public void Awake(long userid)
        {
            this.UserID = userid;
        }

        public Vector3 Position
        {
            get => GameObject.transform.position;
            set => GameObject.transform.position = value;
        }

        public Quaternion Rotation
        {
            get => GameObject.transform.rotation;
            set => GameObject.transform.rotation = value;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            
            base.Dispose();
        }
    }
}
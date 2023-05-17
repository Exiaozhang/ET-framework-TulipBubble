using System.Collections.Generic;
using CSharpx;

namespace ETModel
{
    /// <summary>
    /// 玩家顺序控制
    /// </summary>
    public class OrderControllerComponent : Component
    {
        //当前回合玩家的顺序
        public List<Gamer> turnGamerOrder = new List<Gamer>();

        //当前出牌玩家
        public Gamer currentAuthority { get; set; }

        //当前玩家的顺位
        public int currentPlayerOrder { get; set; }
        
        //当前回合
        public int currentBigRound;

        //当前轮数
        public int currentRound;
        
        //当前阶段
        public GameStage stage;

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
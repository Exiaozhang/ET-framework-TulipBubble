using System.Collections.Generic;

namespace ETModel
{

    /// <summary>
    /// 玩家顺序控制
    /// </summary>
    public class OrderControllerComponent : Component
    {
        //当前回合玩家的顺序
        public List<Gamer> turnGamerOrder = new List<Gamer>();

        /// <summary>
        /// 当前出牌玩家
        /// </summary>
        public Gamer currentAuthority { get; set; }

        /// <summary>
        /// 当前行动玩家的顺位
        /// </summary>
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
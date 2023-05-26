using System.Xml.Linq;
namespace ETModel
{
    /// <summary>
    /// 房间等级
    /// </summary>
    public enum RoomLevel
    {
        Lv100
    }

    /// <summary>
    /// 身份
    /// </summary>
    public enum Identity
    {
        None
    }

    /// <summary>
    /// 牌类型
    /// </summary>
    public enum CardsType
    {
        None
    }

    /// <summary>
    /// 郁金香等级
    /// </summary>
    public enum Level
    {
        A,
        B,
        C,
        None,
        Anyone,
        X
    }

    /// <summary>
    /// 郁金香品种
    /// </summary>
    public enum Weight
    {
        one,
        two,
        three,
        None,
        Anyone,
        Y
    }

    /// <summary>
    /// 郁金香花色
    /// </summary>
    public enum TulipColor
    {
        White,
        Yellow,
        Red,
        Black
    }

    /// <summary>
    /// 市场事件
    /// </summary>
    public enum MarketEvent
    {
        TulipRise,
        TulipDrop,
        RiseSuddenly,
        DropSuddenly,
        BubbleBurst
    }

    /// <summary>
    /// 收藏家要求颜色类型
    /// </summary>
    public enum RequestColor
    {
        PureColor,
        DiverseColor
    }

    /// <summary>
    /// 游戏进行的阶段
    /// </summary>
    public enum GameStage
    {
        EventStage,
        SellStage,
        AuctionStage,
        FinishStage,
    }

    /// <summary>
    /// 付费方式
    /// </summary>
    public enum PayWay
    {
        Cash,
        Loans
    }

    /// <summary>
    /// 收藏家价格等级
    /// </summary>
    public enum CollectorPriceLevel
    {
        low,
        middle,
        high
    }
}
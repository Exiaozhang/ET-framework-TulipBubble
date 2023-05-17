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
    public enum CardType
    {
        TulipCard,
        CollectorCard,
        EventCard
    }

    /// <summary>
    /// 卡牌拥有者类型
    /// </summary>
    public enum CardBelongType
    {
        Market,
        Player,
        Error
    }

    /// <summary>
    /// 郁金香市场类型
    /// </summary>
    public enum TulipMarkType
    {
        future,
        cash,
        selled
    }
    
    /// <summary>
    /// 郁金香等级
    /// </summary>
    public enum Level
    {
        A,
        B,
        C,
        None
    }

    /// <summary>
    /// 郁金香品种
    /// </summary>
    public enum Weight
    {
        one,
        two,
        three,
        None
    }
    
    /// <summary>
    /// 郁金香花色
    /// </summary>
    public enum Color
    {
        White,
        Yellow,
        Red,
        Black
    }

    /// <summary>
    /// 游戏进行的阶段
    /// </summary>
    public enum GameStage
    {
        EventStage,
        SellStage,
        AuctionStage,
        FinishStage
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
}
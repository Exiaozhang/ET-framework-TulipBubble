using ETModel;
namespace ETModel
{
//发送给服务器预定的郁金香
	[Message(OuterOpcode.Actor_ReserveTulipCard_Ntt)]
	public partial class Actor_ReserveTulipCard_Ntt : IActorMessage {}

//发送给服务器要出售的郁金香
	[Message(OuterOpcode.Actor_SellTulipCard_Ntt)]
	public partial class Actor_SellTulipCard_Ntt : IActorMessage {}

//发送给客户端起始玩家
	[Message(OuterOpcode.Actor_AuthorityPlayCard_Ntt)]
	public partial class Actor_AuthorityPlayCard_Ntt : IActorMessage {}

//发送给客户端当前可用的收藏家
	[Message(OuterOpcode.Actor_GetCollector_Ntt)]
	public partial class Actor_GetCollector_Ntt : IActorMessage {}

//发送给客户端本回合的事件卡
	[Message(OuterOpcode.Actor_GetEvent_Ntt)]
	public partial class Actor_GetEvent_Ntt : IActorMessage {}

//发送给客户端当前市场郁金香价格
	[Message(OuterOpcode.Actor_GetTulipPriceLevel_Ntt)]
	public partial class Actor_GetTulipPriceLevel_Ntt : IActorMessage {}

//事件牌类消息
	[Message(OuterOpcode.EventCard)]
	public partial class EventCard {}

//郁金香牌类消息
	[Message(OuterOpcode.TulipCard)]
	public partial class TulipCard {}

//郁金香牌类消息
	[Message(OuterOpcode.CollectorTulipCard)]
	public partial class CollectorTulipCard {}

//收藏家牌类消息
	[Message(OuterOpcode.CollectorCard)]
	public partial class CollectorCard {}

//牌长度消息
	[Message(OuterOpcode.GamerCardNum)]
	public partial class GamerCardNum {}

//初始时房间牌
	[Message(OuterOpcode.Actor_GameStartRoomCards_Ntt)]
	public partial class Actor_GameStartRoomCards_Ntt : IActorMessage {}

	[Message(OuterOpcode.Actor_GameStartMention)]
	public partial class Actor_GameStartMention : IActorMessage {}

//获取房间内玩家信息请求
	[Message(OuterOpcode.C2G_GetUserInfoInRoom_Req)]
	public partial class C2G_GetUserInfoInRoom_Req : IRequest {}

//获取房间内玩家信息返回
	[Message(OuterOpcode.G2C_GetUserInfoInRoom_Back)]
	public partial class G2C_GetUserInfoInRoom_Back : IResponse {}

//----ET
	[Message(OuterOpcode.Actor_Test)]
	public partial class Actor_Test : IActorMessage {}

	[Message(OuterOpcode.C2M_TestRequest)]
	public partial class C2M_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestResponse)]
	public partial class M2C_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.C2G_EnterMap)]
	public partial class C2G_EnterMap : IRequest {}

	[Message(OuterOpcode.G2C_EnterMap)]
	public partial class G2C_EnterMap : IResponse {}

	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.M2C_CreateUnits)]
	public partial class M2C_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	public partial class M2C_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(OuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

	[Message(OuterOpcode.G2C_Test)]
	public partial class G2C_Test : IMessage {}

	[Message(OuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

}
namespace ETModel
{
	public static partial class OuterOpcode
	{
		 public const ushort Actor_ReserveTulipCard_Ntt = 101;
		 public const ushort Actor_SellTulipCard_Ntt = 102;
		 public const ushort Actor_AuthorityPlayCard_Ntt = 103;
		 public const ushort Actor_GetCollector_Ntt = 104;
		 public const ushort Actor_GetEvent_Ntt = 105;
		 public const ushort Actor_GetTulipPriceLevel_Ntt = 106;
		 public const ushort EventCard = 107;
		 public const ushort TulipCard = 108;
		 public const ushort CollectorTulipCard = 109;
		 public const ushort CollectorCard = 110;
		 public const ushort GamerCardNum = 111;
		 public const ushort Actor_GameStartRoomCards_Ntt = 112;
		 public const ushort Actor_GameStartMention = 113;
		 public const ushort C2G_GetUserInfoInRoom_Req = 114;
		 public const ushort G2C_GetUserInfoInRoom_Back = 115;
		 public const ushort Actor_Test = 116;
		 public const ushort C2M_TestRequest = 117;
		 public const ushort M2C_TestResponse = 118;
		 public const ushort Actor_TransferRequest = 119;
		 public const ushort Actor_TransferResponse = 120;
		 public const ushort C2G_EnterMap = 121;
		 public const ushort G2C_EnterMap = 122;
		 public const ushort UnitInfo = 123;
		 public const ushort M2C_CreateUnits = 124;
		 public const ushort Frame_ClickMap = 125;
		 public const ushort M2C_PathfindingResult = 126;
		 public const ushort C2R_Ping = 127;
		 public const ushort R2C_Ping = 128;
		 public const ushort G2C_Test = 129;
		 public const ushort C2M_Reload = 130;
		 public const ushort M2C_Reload = 131;
	}
}

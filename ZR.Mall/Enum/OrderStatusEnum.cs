namespace ZR.Mall.Enum
{
    /// <summary>
    /// 订单状态 0待付款 1待发货 2已发货 3已完成 4已取消 5已关闭
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// 待付款(订单已创建，未支付）
        /// </summary>
        None = 0,
        /// <summary>
        /// 待发货（已支付）
        /// </summary>
        TobeShipped = 1,
        /// <summary>
        /// 已发货（物流中）
        /// </summary>
        Shipped = 2,
        /// <summary>
        /// 已完成 /已收货（用户确认收货或系统自动收货）
        /// </summary>
        Completed = 3,
        /// <summary>
        /// 取消（用户主动取消、未支付超时取消）
        /// </summary>
        Cancel = 4,
        /// <summary>
        /// 已关闭（订单退款/售后完成后彻底关闭）
        /// </summary>
        Closed = 5
    }
}

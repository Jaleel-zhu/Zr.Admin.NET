namespace ZR.Mall.Enum
{
    /// <summary>
    /// 
    /// </summary>
    public enum DeliveryStatusEnum
    {
        /// <summary>
        /// 未发货
        /// </summary>
        NotDelivered = 0,

        /// <summary>
        /// 已发货（物流中）
        /// </summary>
        Delivering = 1,

        /// <summary>
        /// 已送达（物流已签收）
        /// </summary>
        Delivered = 2
    }
}

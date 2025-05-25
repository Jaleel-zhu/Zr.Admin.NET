namespace ZR.Mall.Enum
{
    public enum RefundStatusEnum
    {
        None = 0,         // 无退款
        Refunding = 1,    // 退款处理中（用户发起退款/退货中）
        Refunded = 2,     // 已退款完成（整单或部分退款）
        Returning = 3,    // 正在退货（用户已发货）
        Returned = 4      // 已退货完成（商家确认收货并退款）
    }
}

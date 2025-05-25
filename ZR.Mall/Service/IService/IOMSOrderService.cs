using ZR.Mall.Model;
using ZR.Mall.Model.Dto;

namespace ZR.Mall.Service.IService
{
    /// <summary>
    /// 订单管理service接口
    /// </summary>
    public interface IOMSOrderService : IBaseService<OMSOrder>
    {
        PagedInfo<OMSOrderDto> GetList(OMSOrderQueryDto parm);

        OMSOrder GetInfo(long Id);

        int UpdateOMSOrder(int operType, OMSOrder parm);
        int UpdateOMSOrderDeliveryInfo(OMSOrder model);
        int UpdateMerchantNote(OMSOrder model);
        PagedInfo<OMSOrderDto> ExportList(OMSOrderQueryDto parm);

        Task<dynamic> GetTotalSales(OMSOrderQueryDto dto);
        Task<dynamic> GetSaleTreandByDay(OMSOrderQueryDto dto);
        Task<dynamic> GetSaleTopProduct(OMSOrderQueryDto dto);
    }
}

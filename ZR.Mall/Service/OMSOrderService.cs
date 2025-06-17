using Infrastructure.Extensions;
using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

namespace ZR.Mall.Service
{
    /// <summary>
    /// 订单管理Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IOMSOrderService))]
    public class OMSOrderService : BaseService<OMSOrder>, IOMSOrderService
    {
        private ISkusService _shopSkusService;

        public OMSOrderService(ISkusService shopSkusService)
        {
            _shopSkusService = shopSkusService;
        }

        /// <summary>
        /// 查询订单管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OMSOrderDto> GetList(OMSOrderQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Includes(x => x.Items)
                .Where(predicate.ToExpression())
                .ToPage<OMSOrder, OMSOrderDto>(parm);

            return response;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OMSOrder GetInfo(long Id)
        {
            var response = Queryable()
                .Includes(x => x.Items)
                .Where(x => x.Id == Id)
                .First();

            return response;
        }

        /// <summary>
        /// 修改订单管理
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOMSOrder(int operType, OMSOrder model)
        {
            //发货
            if (operType == 1)
            {
                return UpdateOMSOrderDeliveryInfo(model);
            }
            //修改商家备注
            if (operType == 2)
            {
                return UpdateMerchantNote(model);
            }
            //修改地址
            if (operType == 3)
            {
                var result = Update(w => w.Id == model.Id, it => new OMSOrder()
                {
                    AddressSnapshot = model.AddressSnapshot,
                });
                return result;
            }
            return Update(model, true);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateOMSOrderDeliveryInfo(OMSOrder model)
        {
            var dbDate = Context.GetDate();
            var result = Update(w => w.Id == model.Id, it => new OMSOrder()
            {
                DeliveryCompany = model.DeliveryCompany,
                DeliveryNo = model.DeliveryNo,
                OrderStatus = Enum.OrderStatusEnum.Shipped, // 已发货
                DeliveryStatus = Enum.DeliveryStatusEnum.Delivering, // 已发货
                ShipTime = dbDate
            });
            //TODO 发送消息通知用户,订单完整日志
            return result;
        }

        /// <summary>
        /// 修改平台备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateMerchantNote(OMSOrder model)
        {
            var result = Update(w => w.OrderNo == model.OrderNo, it => new OMSOrder()
            {
                MerchantNote = model.MerchantNote,
            });
            return result;
        }

        /// <summary>
        /// 导出订单管理
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<OMSOrderDto> ExportList(OMSOrderQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .Select((it) => new OMSOrderDto()
                {
                }, true)
                .ToPage(parm);

            return response;
        }

        /// <summary>
        /// 总销售额和订单数
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetTotalSales(OMSOrderQueryDto dto)
        {
            var result = await Queryable()
                //.Where(o => o.OrderStatus == Enum.OrderStatusEnum.Completed)
                .WhereIF(dto.BeginCreateTime != null, o => o.PayTime >= dto.BeginCreateTime)
                .WhereIF(dto.BeginCreateTime != null, o => o.PayTime <= dto.EndCreateTime)
                .Select(o => new
                {
                    TotalSales = SqlFunc.AggregateSum(o.PayAmount),
                    OrderCount = SqlFunc.AggregateCount(o.Id)
                })
                .FirstAsync();
            return result;
        }

        /// <summary>
        /// 销售趋势（按天）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<dynamic> GetSaleTreandByDay(OMSOrderQueryDto dto)
        {
            var trend = await Queryable()
            .WhereIF(dto.BeginCreateTime == null, o => o.PayTime >= DateTime.Now.AddDays(-7))
            .WhereIF(dto.BeginCreateTime != null, o => o.PayTime >= dto.BeginCreateTime)
            .WhereIF(dto.BeginCreateTime != null, o => o.PayTime <= dto.EndCreateTime)
            .GroupBy(o => SqlFunc.ToDateShort(o.PayTime))
            .OrderBy(o => SqlFunc.ToDateShort(o.PayTime))
            .Select(o => new
            {
                Date = SqlFunc.ToDateShort(o.PayTime),
                TotalSales = SqlFunc.AggregateSum(o.PayAmount),
                OrderCount = SqlFunc.AggregateCount(o.Id)
            })
            .ToListAsync();

            return trend;
        }

        /// <summary>
        /// 销售排名前10的商品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<dynamic> GetSaleTopProduct(OMSOrderQueryDto dto)
        {
            var topProducts = await Context.Queryable<OMSOrderItem>()
                .InnerJoin<OMSOrder>((oi, o) => oi.OrderId == o.Id)
                .WhereIF(dto.BeginCreateTime != null, (oi, o) => o.PayTime >= dto.BeginCreateTime)
                .WhereIF(dto.BeginCreateTime != null, (oi, o) => o.PayTime <= dto.EndCreateTime)
                .GroupBy((oi, o) => oi.ProductId)
                .OrderBy((oi, o) => SqlFunc.AggregateSum(oi.Quantity), OrderByType.Desc)
                .Select((oi, o) => new
                {
                    oi.ProductId,
                    TotalSold = SqlFunc.AggregateSum(oi.Quantity),
                    TotalSales = SqlFunc.AggregateSum(oi.Quantity * oi.TotalPrice)
                })
                .Take(10)
                .MergeTable()
                .LeftJoin<Product>((it, p) => it.ProductId == p.ProductId)
                .Select((it, p) => new
                {
                    p.ProductName,
                    it.TotalSold,
                    it.TotalSales
                })
                .ToListAsync();

            return topProducts;
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<OMSOrder> QueryExp(OMSOrderQueryDto parm)
        {
            var predicate = Expressionable.Create<OMSOrder>();
            if (parm.OrderStatus == null && parm.BeginCreateTime == null)
            {
                predicate = predicate.And(it => it.CreateTime >= DateTime.Now.AddDays(-7).ToShortDateString().ParseToDateTime());
            }
            else
            {
                predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime >= parm.BeginCreateTime);
                predicate = predicate.AndIF(parm.EndCreateTime != null, it => it.CreateTime <= parm.EndCreateTime);
            }
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.OrderNo), it => it.OrderNo == parm.OrderNo);
            predicate = predicate.AndIF(parm.UserId != null, it => it.UserId == parm.UserId);
            predicate = predicate.AndIF(parm.OrderStatus != null, it => it.OrderStatus == parm.OrderStatus);

            //predicate = predicate.AndIF(parm.ConfirmStatus != null, it => it.ConfirmStatus == parm.ConfirmStatus);
            predicate = predicate.AndIF(parm.DeliveryNo.IsNotEmpty(), it => it.DeliveryNo == parm.DeliveryNo);
            return predicate;
        }
    }
}
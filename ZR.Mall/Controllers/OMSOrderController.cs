using Microsoft.AspNetCore.Mvc;
using ZR.Common;
using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

//创建时间：2025-05-30
namespace ZR.Mall.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    [Route("shopping/Order")]
    [ApiExplorerSettings(GroupName = "shopping")]
    public class OMSOrderController : BaseController
    {
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 订单管理接口
        /// </summary>
        private readonly IOMSOrderService _OMSOrderService;

        public OMSOrderController(IOMSOrderService OMSOrderService)
        {
            _OMSOrderService = OMSOrderService;
        }

        /// <summary>
        /// 查询订单管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "oms:order:list")]
        public IActionResult QueryOMSOrder([FromQuery] OMSOrderQueryDto parm)
        {
            var response = _OMSOrderService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询订单管理详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "oms:order:query")]
        public IActionResult GetOMSOrder(long Id)
        {
            var response = _OMSOrderService.GetInfo(Id);

            var info = response.Adapt<OMSOrderDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 更新订单管理
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Log(Title = "订单管理", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateOMSOrder([FromBody] OMSOrderDto parm)
        {
            var modal = parm.Adapt<OMSOrder>().ToUpdate(HttpContext);
            var response = _OMSOrderService.UpdateOMSOrder(parm.OperType, modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除订单管理
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{ids}")]
        [ActionPermissionFilter(Permission = "oms:order:delete")]
        [Log(Title = "订单管理", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteOMSOrder([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<long>(ids);

            return ToResponse(_OMSOrderService.Delete(idArr));
        }

        /// <summary>
        /// 导出订单管理
        /// </summary>
        /// <returns></returns>
        [Log(Title = "订单管理", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "oms:order:export")]
        public IActionResult Export([FromQuery] OMSOrderQueryDto parm)
        {
            var list = _OMSOrderService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "订单管理", "订单管理");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 查询销售总
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSales")]
        [ActionPermissionFilter(Permission = "oms:sale:query")]
        public async Task<IActionResult> GetSales(OMSOrderQueryDto dto)
        {
            var response = await _OMSOrderService.GetTotalSales(dto);

            return SUCCESS(response);
        }

        /// <summary>
        /// 查询销售趋势
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSalesTrade")]
        [ActionPermissionFilter(Permission = "oms:sale:query")]
        public async Task<IActionResult> GetSalesTrade(OMSOrderQueryDto dto)
        {
            var response =  await _OMSOrderService.GetSaleTreandByDay(dto);

            return SUCCESS(response);
        }

        /// <summary>
        /// 查询销售前10的商品
        /// </summary>
        /// <returns></returns>
        [HttpGet("getSaleTopProduct")]
        [ActionPermissionFilter(Permission = "oms:sale:query")]
        public async Task<IActionResult> GetSaleTopProduct(OMSOrderQueryDto dto)
        {
            var response = await _OMSOrderService.GetSaleTopProduct(dto);

            return SUCCESS(response);
        }
    }
}
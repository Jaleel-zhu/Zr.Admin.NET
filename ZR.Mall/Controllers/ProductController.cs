using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using ZR.Common;
using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

//创建时间：2025-05-23
namespace ZR.Mall.Controllers
{
    /// <summary>
    /// 商品管理
    /// </summary>
    [Route("shopping/product")]
    [ApiExplorerSettings(GroupName = "shopping")]
    public class ProductController : BaseController
    {
        /// <summary>
        /// 商品管理接口
        /// </summary>
        private readonly IProductService _ShoppingProductService;

        public ProductController(IProductService ShoppingProductService)
        {
            _ShoppingProductService = ShoppingProductService;
        }

        /// <summary>
        /// 查询商品管理列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "shop:product:list")]
        public IActionResult QueryShoppingProduct([FromQuery] ShoppingProductQueryDto parm)
        {
            var response = _ShoppingProductService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询商品管理详情
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        [HttpGet("{ProductId}")]
        public IActionResult GetShoppingProduct(int ProductId)
        {
            var response = _ShoppingProductService.GetInfo(ProductId);

            var info = response.Adapt<ProductDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加商品管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "shop:product:add")]
        [Log(Title = "商品管理", BusinessType = BusinessType.INSERT)]
        public IActionResult AddShoppingProduct([FromBody] ProductDto parm)
        {
            if (parm == null) { return ToResponse(ResultCode.PARAM_ERROR, "参数错误"); }
            var response = _ShoppingProductService.AddShoppingProduct(parm.ToCreate());

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新商品管理
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "shop:product:edit")]
        [Log(Title = "商品编辑", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateShoppingProduct([FromBody] ProductDto parm)
        {
            var response = _ShoppingProductService.UpdateShoppingProduct(parm.ToUpdate());

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新商品管理
        /// </summary>
        /// <returns></returns>
        [HttpPut("edit")]
        [ActionPermissionFilter(Permission = "shop:product:edit")]
        [Log(Title = "商品编辑", BusinessType = BusinessType.UPDATE)]
        public IActionResult EditInfo([FromBody] ProductDto parm)
        {
            var response = _ShoppingProductService.UpdateInfo(parm.ToUpdate());

            return ToResponse(response);
        }

        /// <summary>
        /// 删除商品管理
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{ids}")]
        [ActionPermissionFilter(Permission = "shop:product:delete")]
        [Log(Title = "商品管理", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteShoppingProduct([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);
            var result = _ShoppingProductService.Deleteable()
                .Where(f => f.IsDelete == 0)
                .In(ids)
                .IsLogic()
                .ExecuteCommand();
            return SUCCESS(result);
        }

        /// <summary>
        /// 保存排序
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="value">排序值</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "shop:product:edit")]
        [HttpGet("ChangeSort")]
        [Log(Title = "保存排序", BusinessType = BusinessType.UPDATE)]
        public IActionResult ChangeSort(int id = 0, int value = 0)
        {
            if (id <= 0) { return ToResponse(ApiResult.Error(101, "请求参数错误")); }
            var response = _ShoppingProductService.Update(w => w.ProductId == id, it => new Product()
            {
                SortId = value,
            });

            return ToResponse(response);
        }

        /// <summary>
        /// 导出商品管理
        /// </summary>
        /// <returns></returns>
        [Log(Title = "商品管理", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "shop:product:export")]
        public IActionResult Export([FromQuery] ShoppingProductQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _ShoppingProductService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "商品管理", "商品管理");
            return ExportExcel(result.Item2, result.Item1);
        }
    }
}
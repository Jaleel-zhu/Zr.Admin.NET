using Infrastructure.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZR.Common;
using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

namespace ZR.Mall.Controllers
{
    /// <summary>
    /// 商品分类Controller
    /// </summary>
    [Route("shopping/category")]
    [ApiExplorerSettings(GroupName = "shopping")]
    public class CategoryController : BaseController
    {
        /// <summary>
        /// 商品分类接口
        /// </summary>
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 查询商品分类列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "shop:category:list")]
        public IActionResult QueryCategory([FromQuery] ShoppingCategoryQueryDto parm)
        {
            var response = _categoryService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询商品分类列表树
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("treeList")]
        //[AllowAnonymous]
        public IActionResult QueryTreeCategory([FromQuery] ShoppingCategoryQueryDto parm)
        {
            var response = _categoryService.GetTreeList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpGet("{CategoryId}")]
        [AllowAnonymous]
        public IActionResult GetCategory(int CategoryId)
        {
            var response = _categoryService.GetFirst(x => x.CategoryId == CategoryId);

            return SUCCESS(response);
        }

        /// <summary>
        /// 添加商城目录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "shop:category:add")]
        [Log(Title = "商城分类", BusinessType = BusinessType.INSERT)]
        public IActionResult AddCategory([FromBody] CategoryDto parm)
        {
            var modal = parm.Adapt<Category>().ToCreate(HttpContext);
            var response = _categoryService.AddCategory(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 更新商城分类
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "shop:category:edit")]
        [Log(Title = "商城分类", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateShoppingCategory([FromBody] CategoryDto parm)
        {
            var modal = parm.Adapt<Category>().ToUpdate(HttpContext);
            var response = _categoryService.Update(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除商城分类
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "shop:category:delete")]
        [Log(Title = "商城分类", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteCategory(string ids)
        {
            int[] idsArr = Tools.SpitIntArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _categoryService.Delete(idsArr);

            return ToResponse(response);
        }

        /// <summary>
        /// 导出目录
        /// </summary>
        /// <returns></returns>
        [Log(Title = "商城分类", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "shop:category:export")]
        public IActionResult Export([FromQuery] ShoppingCategoryQueryDto parm)
        {
            parm.PageNum = 1;
            parm.PageSize = 100000;
            var list = _categoryService.ExportList(parm).Result;
            if (list == null || list.Count <= 0)
            {
                return ToResponse(ResultCode.FAIL, "没有要导出的数据");
            }
            var result = ExportExcelMini(list, "商品分类", "商品分类");
            return ExportExcel(result.Item2, result.Item1);
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <returns></returns>
        [HttpGet("CategoryList")]
        public IActionResult CategoryList()
        {
            var response = _categoryService.GetAll();
            return SUCCESS(response);
        }

        /// <summary>
        /// 保存排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "shop:category:edit")]
        [HttpGet("ChangeSort")]
        [Log(Title = "保存排序", BusinessType = BusinessType.UPDATE)]
        public IActionResult ChangeSort(int id = 0, int value = 0)
        {
            if (id <= 0) { return ToResponse(ApiResult.Error(101, "请求参数错误")); }
            var response = _categoryService.Update(w => w.CategoryId == id, it => new Category()
            {
                CategoryId = id,
                OrderNum = value
            });
            return ToResponse(response);
        }
    }
}
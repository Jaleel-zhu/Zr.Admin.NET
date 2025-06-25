using Microsoft.AspNetCore.Mvc;
using ZR.Common;
using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

//创建时间：2025-06-23
namespace ZR.Mall.Controllers
{
    /// <summary>
    /// 规格模板
    /// </summary>
    [Route("shopping/SpecTemplate")]
    public class SpecTemplateController : BaseController
    {
        /// <summary>
        /// 规格模板接口
        /// </summary>
        private readonly ISpecTemplateService _MmsSpecTemplateService;

        public SpecTemplateController(ISpecTemplateService MmsSpecTemplateService)
        {
            _MmsSpecTemplateService = MmsSpecTemplateService;
        }

        /// <summary>
        /// 查询规格模板列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "spectpl:list")]
        public IActionResult QueryMmsSpecTemplate([FromQuery] SpecTemplateQueryDto parm)
        {
            var response = _MmsSpecTemplateService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询规格模板列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("tplList")]
        public IActionResult QuerySpecTemplate([FromQuery] SpecTemplateQueryDto parm)
        {
            parm.PageSize = 50;
            var response = _MmsSpecTemplateService.GetList(parm).Result;
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询规格模板详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public IActionResult GetMmsSpecTemplate(long Id)
        {
            var response = _MmsSpecTemplateService.GetInfo(Id);
            
            var info = response.Adapt<SpecTemplateDto>();
            return SUCCESS(info);
        }

        /// <summary>
        /// 添加规格模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionPermissionFilter(Permission = "spectpl:add")]
        [Log(Title = "规格模板", BusinessType = BusinessType.INSERT)]
        public IActionResult AddMmsSpecTemplate([FromBody] SpecTemplateDto parm)
        {
            var modal = parm.Adapt<SpecTemplate>().ToCreate(HttpContext);

            var response = _MmsSpecTemplateService.AddMmsSpecTemplate(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新规格模板
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "spectpl:edit")]
        [Log(Title = "规格模板", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateMmsSpecTemplate([FromBody] SpecTemplateDto parm)
        {
            var modal = parm.Adapt<SpecTemplate>().ToUpdate(HttpContext);
            var response = _MmsSpecTemplateService.UpdateMmsSpecTemplate(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除规格模板
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{ids}")]
        [ActionPermissionFilter(Permission = "spectpl:delete")]
        [Log(Title = "规格模板", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteMmsSpecTemplate([FromRoute]string ids)
        {
            var idArr = Tools.SplitAndConvert<long>(ids);

            return ToResponse(_MmsSpecTemplateService.Delete(idArr));
        }

    }
}
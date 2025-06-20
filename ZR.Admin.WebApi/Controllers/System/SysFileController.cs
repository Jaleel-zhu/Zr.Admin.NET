using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using ZR.Model.System;
using ZR.Model.System.Dto;

namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 文件存储Controller
    /// </summary>
    [Route("tool/file")]
    [ApiExplorerSettings(GroupName = "sys")]
    public class SysFileController : BaseController
    {
        /// <summary>
        /// 文件存储接口
        /// </summary>
        private readonly ISysFileService _SysFileService;

        public SysFileController(ISysFileService SysFileService)
        {
            _SysFileService = SysFileService;
        }

        /// <summary>
        /// 查询文件存储列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "tool:file:list")]
        public IActionResult QuerySysFile([FromQuery] SysFileQueryDto parm)
        {
            var predicate = Expressionable.Create<SysFile>();

            predicate = predicate.AndIF(parm.BeginCreate_time != null, it => it.Create_time >= parm.BeginCreate_time);
            predicate = predicate.AndIF(parm.EndCreate_time != null, it => it.Create_time <= parm.EndCreate_time);
            predicate = predicate.AndIF(parm.StoreType != null, m => m.StoreType == parm.StoreType);
            predicate = predicate.AndIF(parm.FileId != null, m => m.Id == parm.FileId);
            predicate = predicate.AndIF(parm.ClassifyType != null, m => m.ClassifyType == parm.ClassifyType);

            var response = _SysFileService.GetPages(predicate.ToExpression(), parm, x => x.Id, OrderByType.Desc);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询文件存储列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("listByGroup")]
        public IActionResult QueryFile([FromQuery] SysFileQueryDto parm)
        {
            var predicate = Expressionable.Create<SysFile>();

            predicate = predicate.AndIF(parm.StoreType != null, m => m.StoreType == parm.StoreType);
            predicate = predicate.AndIF(parm.FileId != null, m => m.Id == parm.FileId);
            predicate = predicate.AndIF(parm.ClassifyType != null, m => m.ClassifyType == parm.ClassifyType);
            predicate = predicate.AndIF(parm.CategoryId > 0, m => m.CategoryId == parm.CategoryId);
            predicate = predicate.And(m => m.FileType.StartsWith("image/", StringComparison.OrdinalIgnoreCase));

            var response = _SysFileService.GetPages(predicate.ToExpression(), parm, x => x.Id, OrderByType.Desc);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询文件存储详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ActionPermissionFilter(Permission = "tool:file:query")]
        public IActionResult GetSysFile(long Id)
        {
            var response = _SysFileService.GetFirst(x => x.Id == Id);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "tool:file:edit")]
        [Log(Title = "文件存储", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateFile([FromBody] SysFileDto parm)
        {
            var modal = parm.Adapt<SysFile>().ToUpdate(HttpContext);
            modal.ClassifyType ??= "";
            var response = _SysFileService.UpdateFile(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除文件存储
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "tool:file:delete")]
        [Log(Title = "文件存储", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteSysFile(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);
            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _SysFileService.Delete(idsArr);
            //TODO 删除本地资源

            return ToResponse(response);
        }

        /// <summary>
        /// 文件存储导出
        /// </summary>
        /// <returns></returns>
        [Log(BusinessType = BusinessType.EXPORT, IsSaveResponseData = false, Title = "文件存储")]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "tool:file:export")]
        public IActionResult Export()
        {
            var list = _SysFileService.GetAll();

            string sFileName = ExportExcel(list, "SysFile", "文件存储");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }
    }
}
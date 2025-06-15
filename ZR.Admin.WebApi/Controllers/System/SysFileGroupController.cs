using Microsoft.AspNetCore.Mvc;
using ZR.Model.System.Model;
using ZR.Model.System.Model.Dto;

//创建时间：2025-06-08
namespace ZR.Admin.WebApi.Controllers.System
{
    /// <summary>
    /// 文件分组
    /// </summary>
    [Route("tool/FileGroup")]
    public class SysFileGroupController : BaseController
    {
        /// <summary>
        /// 文件分组接口
        /// </summary>
        private readonly ISysFileGroupService _SysFileGroupService;

        public SysFileGroupController(ISysFileGroupService SysFileGroupService)
        {
            _SysFileGroupService = SysFileGroupService;
        }

        /// <summary>
        /// 查询文件分组列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [ActionPermissionFilter(Permission = "filegroup:list")]
        public IActionResult QuerySysFileGroup([FromQuery] SysFileGroupQueryDto parm)
        {
            var response = _SysFileGroupService.GetList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 查询文件分组列表树
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet("treeList")]
        //[ActionPermissionFilter(Permission = "filegroup:list")]
        public IActionResult QueryTreeSysFileGroup([FromQuery] SysFileGroupQueryDto parm)
        {
            var response = _SysFileGroupService.GetTreeList(parm);
            return SUCCESS(response);
        }

        /// <summary>
        /// 添加文件分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ActionPermissionFilter(Permission = "filegroup:add")]
        [Log(Title = "文件分组", BusinessType = BusinessType.INSERT)]
        public IActionResult AddSysFileGroup([FromBody] SysFileGroupDto parm)
        {
            var modal = parm.Adapt<SysFileGroup>().ToCreate(HttpContext);

            var response = _SysFileGroupService.AddSysFileGroup(modal);

            return SUCCESS(response);
        }

        /// <summary>
        /// 更新文件分组
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [ActionPermissionFilter(Permission = "filegroup:edit")]
        [Log(Title = "文件分组", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateSysFileGroup([FromBody] SysFileGroupDto parm)
        {
            var modal = parm.Adapt<SysFileGroup>().ToUpdate(HttpContext);
            var response = _SysFileGroupService.UpdateSysFileGroup(modal);

            return ToResponse(response);
        }

        /// <summary>
        /// 删除文件分组
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete/{ids}")]
        [ActionPermissionFilter(Permission = "filegroup:delete")]
        [Log(Title = "文件分组", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteSysFileGroup([FromRoute] string ids)
        {
            var idArr = Tools.SplitAndConvert<int>(ids);

            return ToResponse(_SysFileGroupService.Delete(idArr));
        }

        /// <summary>
        /// 保存排序
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="value">排序值</param>
        /// <returns></returns>
        [ActionPermissionFilter(Permission = "filegroup:edit")]
        [HttpGet("ChangeSort")]
        [Log(Title = "保存排序", BusinessType = BusinessType.UPDATE)]
        public IActionResult ChangeSort(int id = 0, int value = 0)
        {
            if (id <= 0) { return ToResponse(ApiResult.Error(101, "请求参数错误")); }
            var response = _SysFileGroupService.Update(w => w.GroupId == id, it => new SysFileGroup()
            {
                Sort = value,
            });

            return ToResponse(response);
        }
    }
}